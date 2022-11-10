using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace caravanVisual;

[HarmonyPatch(typeof(ExpandableWorldObjectsUtility), "ExpandableWorldObjectsOnGUI")]
public class Patch_ExpandableWorldObjectsUtility_ExpandableWorldObjectsOnGUI
{
    private static readonly List<WorldObject> tmpWorldObjects = AccessTools
        .StaticFieldRefAccess<List<WorldObject>>(AccessTools.Field(typeof(ExpandableWorldObjectsUtility),
            "tmpWorldObjects")).Invoke();

    private static List<Pawn> ar_pawn = new List<Pawn>();

    public static bool Prefix()
    {
        if (caravanVisualMod.instance.Settings.ZoomMode == caravanComponent.en_zoomMode.vanilla)
        {
            return true;
        }

        if (ExpandableWorldObjectsUtility.TransitionPct == 0f)
        {
            return false;
        }

        tmpWorldObjects.Clear();
        tmpWorldObjects.AddRange(Find.WorldObjects.AllWorldObjects);
        SortByExpandingIconPriority(tmpWorldObjects);
        var worldTargeter = Find.WorldTargeter;
        List<WorldObject> worldObjectsUnderMouse = null;
        if (worldTargeter.IsTargeting)
        {
            worldObjectsUnderMouse = GenWorldUI.WorldObjectsUnderMouse(UI.MousePositionOnUI);
        }

        foreach (var tmpObject in tmpWorldObjects)
        {
            try
            {
                if (!tmpObject.def.expandingIcon || tmpObject.HiddenBehindTerrainNow())
                {
                    continue;
                }

                var expandingIconColor = tmpObject.ExpandingIconColor;
                expandingIconColor.a = ExpandableWorldObjectsUtility.TransitionPct;
                if (worldTargeter.IsTargetedNow(tmpObject, worldObjectsUnderMouse))
                {
                    var num = GenMath.LerpDouble(-1f, 1f, 0.7f, 1f, Mathf.Sin(Time.time * 8f));
                    expandingIconColor.r *= num;
                    expandingIconColor.g *= num;
                    expandingIconColor.b *= num;
                }

                GUI.color = expandingIconColor;
                var rect = ExpandableWorldObjectsUtility.ExpandedIconScreenRect(tmpObject);
                if (tmpObject.ExpandingIconFlipHorizontal)
                {
                    rect.x = rect.xMax;
                    rect.width *= -1f;
                }

                if (tmpObject is Caravan caravan)
                {
                    // yayo

                    switch (caravanVisualMod.instance.Settings.ZoomMode)
                    {
                        case caravanComponent.en_zoomMode.bigLeader:

                            ar_pawn.Clear();
                            ar_pawn.AddRange(caravan.PawnsListForReading);
                            ar_pawn = ar_pawn.OrderByDescending(a => a.RaceProps is { Humanlike: true })
                                .ThenByDescending(a => a.GetStatValueForPawn(StatDefOf.MoveSpeed, a)).ToList();

                            var p = ar_pawn[0];
                            var finalScale = 1.5f * caravanVisualMod.instance.Settings.ZoomScale;
                            rect.position -= rect.size * (finalScale - 1f) * 0.5f;
                            rect.size *= finalScale;
                            GUI.color = new Color(1f, 1f, 1f, expandingIconColor.a);
                            Texture t = PortraitsCache.Get(p, new Vector2(512f, 512f),
                                caravanComponent.getRot(caravan));
                            Widgets.DrawTextureRotated(rect, t, caravan.ExpandingIconRotation);
                            break;
                        case caravanComponent.en_zoomMode.none:
                            break;
                    }
                }
                else
                {
                    Widgets.DrawTextureRotated(rect, tmpObject.ExpandingIcon, tmpObject.ExpandingIconRotation);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error while drawing {tmpObject.ToStringSafe()}: {ex}");
            }
        }

        tmpWorldObjects.Clear();
        GUI.color = Color.white;
        return false;
    }


    private static void SortByExpandingIconPriority(List<WorldObject> worldObjects)
    {
        worldObjects.SortBy(delegate(WorldObject x)
        {
            var num = x.ExpandingIconPriority;
            if (x.Faction is { IsPlayer: true })
            {
                num += 0.001f;
            }

            return num;
        }, x => x.ID);
    }

    private static Material OverrideMaterialIfNeeded(Material original, Pawn pawn, bool portrait = false)
    {
        var baseMat = !portrait && pawn.IsInvisible() ? InvisibilityMatPool.GetInvisibleMat(original) : original;
        return pawn.Drawer.renderer.graphics.flasher.GetDamagedMat(baseMat);
    }
}