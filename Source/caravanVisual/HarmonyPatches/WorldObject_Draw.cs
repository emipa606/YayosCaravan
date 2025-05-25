using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace caravanVisual.HarmonyPatches;

[StaticConstructorOnStartup]
[HarmonyPatch(typeof(WorldObject), nameof(WorldObject.Draw))]
public class WorldObject_Draw
{
    //private static MaterialPropertyBlock propertyBlock = AccessTools.StaticFieldRefAccess<MaterialPropertyBlock>(AccessTools.Field(typeof(WorldObject), "propertyBlock")).Invoke();
    private static readonly MaterialPropertyBlock propertyBlock0 = new();
    private static readonly MaterialPropertyBlock propertyBlock = new();
    private static float scale2 = 1f; // 수정금지 동물크기
    private static List<Pawn> pawns = [];
    private static int counter;
    private static Vector3 position;
    private static int gapTick = 100;
    private static Caravan caravan;
    private static caravanData caravanData;
    private static int tryN;
    private static Vector3 velocity;
    private static float scale => caravanVisualMod.instance.Settings.PawnScale; // 폰 크기

    [HarmonyPriority(0)]
    public static bool Prefix(WorldObject __instance)
    {
        if (!caravanVisualMod.instance.Settings.ToggleVisibility)
        {
            return true;
        }

        if (__instance is not Caravan instance)
        {
            return true;
        }

        caravan = instance;
        if (!caravan.Faction.IsPlayer)
        {
            return true;
        }

        caravanData = dataUtility.GetData(caravan);

        var averageTileSize = Find.WorldGrid.averageTileSize;
        var transitionPct = ExpandableWorldObjectsUtility.TransitionPct;

        pawns.Clear();
        pawns.AddRange(caravan.PawnsListForReading);
        if (!caravanVisualMod.instance.Settings.ShowAnimal)
        {
            pawns.RemoveAll(a => a.RaceProps is { Humanlike: false });
        }

        pawns = pawns.OrderByDescending(a => a.RaceProps is { Humanlike: true })
            .ThenByDescending(a => a.GetStatValueForPawn(StatDefOf.MoveSpeed, a)).ToList();
        if (caravanVisualMod.instance.Settings.PawnCount > 0 &&
            pawns.Count > caravanVisualMod.instance.Settings.PawnCount)
        {
            pawns.RemoveRange(caravanVisualMod.instance.Settings.PawnCount,
                pawns.Count - caravanVisualMod.instance.Settings.PawnCount);
        }

        caravanData.PrevPos ??= [];

        gapTick = Mathf.Max(1,
            Mathf.RoundToInt(caravan.TicksPerMove / 20f * caravanVisualMod.instance.Settings.Spacing));

        counter = 0;


        tryN = caravan.PawnsListForReading.Count;


        while (caravanData.Materials.Count < caravan.PawnsListForReading.Count && tryN >= 0)
        {
            tryN--;
            caravanData.Materials.Add(new Material(ShaderDatabase.WorldOverlayTransparentLit));
        }

        var longitude = Find.WorldGrid.LongLatOf(caravan.Tile).x;
        for (var i = 0; i < pawns.Count; i++)
        {
            if (i == 0)
            {
                // leader

                caravanData.Materials[i].renderQueue = WorldMaterials.DynamicObjectRenderQueue;
                caravanData.Materials[i].color = Color.white;
                var rect = ExpandableWorldObjectsUtility.ExpandedIconScreenRect(caravan);
                rect.size *= scale * 6f;
                caravanData.Materials[i].mainTexture = PortraitsCache.Get(pawns[i],
                    new Vector2(Mathf.Min(512f, rect.width), Mathf.Min(512f, rect.height)),
                    caravanComponent.GetRot(caravan),
                    default, 0.5f);

                if (caravanVisualMod.instance.Settings.ZoomMode != caravanComponent.ZoomMode.none &&
                    caravan.def.expandingIcon && transitionPct > 0f)
                {
                    // fade out

                    var color = caravanData.Materials[i].color;
                    var num = 1f - transitionPct;
                    propertyBlock0.SetColor(ShaderPropertyIDs.Color,
                        new Color(color.r, color.g, color.b, color.a * num));
                }

                caravanComponent.DrawQuadTangentialToPlanet(caravan.pather.MovingNow ? pawns[i].thingIDNumber : -1,
                    instance.DrawPos, scale * 3f * averageTileSize, 0.015f, caravanData.Materials[i], longitude,
                    propertyBlock0);
            }
            else
            {
                // followers

                caravanData.Materials[i].renderQueue = WorldMaterials.DynamicObjectRenderQueue;
                caravanData.Materials[i].color = Color.white;
                var rect = ExpandableWorldObjectsUtility.ExpandedIconScreenRect(caravan);

                scale2 = pawns[i].RaceProps is { Humanlike: true }
                    ? 1f
                    : // 사람 크기
                    0.6f; // 동물 크기

                rect.size *= scale * 6f * scale2;

                //rect.width = Mathf.Min(rect.width, 128);
                //rect.height = Mathf.Min(rect.height, 128);
                position = instance.DrawPos;
                velocity = Vector3.zero;
                counter = i * gapTick;
                if (caravanData.PrevPos.Count > 0)
                {
                    if (caravanData.PrevPos.Count <= counter)
                    {
                        counter = caravanData.PrevPos.Count - 1;
                    }

                    position = caravanData.PrevPos[counter];

                    velocity = caravanData.PrevPos[Mathf.Max(0, counter - 3)] - position;
                }
                //pos += new Vector3(0f, 0f, 0.1f * i);

                caravanData.Materials[i].mainTexture = PortraitsCache.Get(pawns[i],
                    new Vector2(Mathf.Min(512f, rect.width), Mathf.Min(512f, rect.height)),
                    caravanComponent.GetRot(velocity), default,
                    0.5f);


                if (caravanVisualMod.instance.Settings.ZoomMode == caravanComponent.ZoomMode.vanilla &&
                    caravan.def.expandingIcon && transitionPct > 0f)
                {
                    // fade out
                    caravanComponent.DrawQuadTangentialToPlanet(
                        caravan.pather.MovingNow ? pawns[i].thingIDNumber : -1, position,
                        scale * 3f * averageTileSize * scale2, 0.015f, caravanData.Materials[i], longitude,
                        propertyBlock0);
                }
                else
                {
                    caravanComponent.DrawQuadTangentialToPlanet(
                        caravan.pather.MovingNow ? pawns[i].thingIDNumber : -1, position,
                        scale * 3f * averageTileSize * scale2, 0.015f, caravanData.Materials[i], longitude,
                        propertyBlock);
                }
            }
        }

        counter++;
        if (caravanData.PrevPos.Count > counter)
        {
            caravanData.PrevPos.RemoveRange(counter, caravanData.PrevPos.Count - counter);
        }

        return false;
    }
}