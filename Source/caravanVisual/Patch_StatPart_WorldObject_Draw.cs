using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace caravanVisual;

[StaticConstructorOnStartup]
[HarmonyPatch(typeof(WorldObject), "Draw")]
public class Patch_StatPart_WorldObject_Draw
{
    //private static MaterialPropertyBlock propertyBlock = AccessTools.StaticFieldRefAccess<MaterialPropertyBlock>(AccessTools.Field(typeof(WorldObject), "propertyBlock")).Invoke();
    private static readonly MaterialPropertyBlock propertyBlock0 = new MaterialPropertyBlock();
    private static readonly MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
    private static float scale2 = 1f; // 수정금지 동물크기
    private static Dictionary<WorldObject, List<Vector3>> dic_pos = new Dictionary<WorldObject, List<Vector3>>();
    private static List<Pawn> ar_pawn = new List<Pawn>();
    private static int n;
    private static Vector3 pos;
    private static int gapTick = 100;
    private static Caravan caravan;
    private static caravanData cd;
    private static int tryN;
    private static Vector3 vel;
    private static float scale => caravanVisualMod.instance.Settings.PawnScale; // 폰 크기

    [HarmonyPriority(0)]
    public static bool Prefix(WorldObject __instance)
    {
        if (__instance is not Caravan instance)
        {
            return true;
        }

        caravan = instance;
        if (!caravan.Faction.IsPlayer)
        {
            return true;
        }

        cd = dataUtility.GetData(caravan);

        var averageTileSize = Find.WorldGrid.averageTileSize;
        var transitionPct = ExpandableWorldObjectsUtility.TransitionPct;

        ar_pawn.Clear();
        ar_pawn.AddRange(caravan.PawnsListForReading);
        if (!caravanVisualMod.instance.Settings.ShowAnimal)
        {
            ar_pawn.RemoveAll(a => a.RaceProps is { Humanlike: false });
        }

        ar_pawn = ar_pawn.OrderByDescending(a => a.RaceProps is { Humanlike: true })
            .ThenByDescending(a => a.GetStatValueForPawn(StatDefOf.MoveSpeed, a)).ToList();
        if (caravanVisualMod.instance.Settings.PawnCount > 0 &&
            ar_pawn.Count > caravanVisualMod.instance.Settings.PawnCount)
        {
            ar_pawn.RemoveRange(caravanVisualMod.instance.Settings.PawnCount,
                ar_pawn.Count - caravanVisualMod.instance.Settings.PawnCount);
        }

        if (cd.prevPos == null)
        {
            cd.prevPos = new List<Vector3>();
        }

        gapTick = Mathf.Max(1,
            Mathf.RoundToInt(caravan.TicksPerMove / 20f * caravanVisualMod.instance.Settings.Spacing));

        n = 0;


        tryN = caravan.PawnsListForReading.Count;


        while (cd.m.Count < caravan.PawnsListForReading.Count && tryN >= 0)
        {
            tryN--;
            cd.m.Add(new Material(ShaderDatabase.WorldOverlayTransparentLit));
        }

        var longitude = Find.WorldGrid.LongLatOf(caravan.Tile).x;
        for (var i = 0; i < ar_pawn.Count; i++)
        {
            if (i == 0)
            {
                // leader

                cd.m[i].renderQueue = WorldMaterials.DynamicObjectRenderQueue;
                cd.m[i].color = Color.white;
                var rect = ExpandableWorldObjectsUtility.ExpandedIconScreenRect(caravan);
                rect.size *= scale * 6f;
                cd.m[i].mainTexture = PortraitsCache.Get(ar_pawn[i],
                    new Vector2(Mathf.Min(512f, rect.width), Mathf.Min(512f, rect.height)),
                    caravanComponent.getRot(caravan),
                    default, 0.5f);

                if (caravanVisualMod.instance.Settings.ZoomMode != caravanComponent.en_zoomMode.none &&
                    caravan.def.expandingIcon && transitionPct > 0f)
                {
                    // fade out

                    var color = cd.m[i].color;
                    var num = 1f - transitionPct;
                    propertyBlock0.SetColor(ShaderPropertyIDs.Color,
                        new Color(color.r, color.g, color.b, color.a * num));
                }

                caravanComponent.DrawQuadTangentialToPlanet(caravan.pather.MovingNow ? ar_pawn[i].thingIDNumber : -1,
                    instance.DrawPos, scale * 3f * averageTileSize, 0.015f, cd.m[i], longitude, propertyBlock0);
            }
            else
            {
                // followers

                cd.m[i].renderQueue = WorldMaterials.DynamicObjectRenderQueue;
                cd.m[i].color = Color.white;
                var rect = ExpandableWorldObjectsUtility.ExpandedIconScreenRect(caravan);

                scale2 = ar_pawn[i].RaceProps is { Humanlike: true }
                    ? 1f
                    : // 사람 크기
                    0.6f; // 동물 크기

                rect.size *= scale * 6f * scale2;

                //rect.width = Mathf.Min(rect.width, 128);
                //rect.height = Mathf.Min(rect.height, 128);
                pos = instance.DrawPos;
                vel = Vector3.zero;
                n = i * gapTick;
                if (cd.prevPos.Count > 0)
                {
                    if (cd.prevPos.Count <= n)
                    {
                        n = cd.prevPos.Count - 1;
                    }

                    pos = cd.prevPos[n];

                    vel = cd.prevPos[Mathf.Max(0, n - 3)] - pos;
                }
                //pos += new Vector3(0f, 0f, 0.1f * i);

                cd.m[i].mainTexture = PortraitsCache.Get(ar_pawn[i],
                    new Vector2(Mathf.Min(512f, rect.width), Mathf.Min(512f, rect.height)),
                    caravanComponent.getRot(vel), default,
                    0.5f);


                if (caravanVisualMod.instance.Settings.ZoomMode == caravanComponent.en_zoomMode.vanilla &&
                    caravan.def.expandingIcon && transitionPct > 0f)
                {
                    // fade out
                    caravanComponent.DrawQuadTangentialToPlanet(
                        caravan.pather.MovingNow ? ar_pawn[i].thingIDNumber : -1, pos,
                        scale * 3f * averageTileSize * scale2, 0.015f, cd.m[i], longitude, propertyBlock0);
                }
                else
                {
                    caravanComponent.DrawQuadTangentialToPlanet(
                        caravan.pather.MovingNow ? ar_pawn[i].thingIDNumber : -1, pos,
                        scale * 3f * averageTileSize * scale2, 0.015f, cd.m[i], longitude, propertyBlock);
                }
            }
        }

        n++;
        if (cd.prevPos.Count > n)
        {
            cd.prevPos.RemoveRange(n, cd.prevPos.Count - n);
        }

        return false;
    }
}