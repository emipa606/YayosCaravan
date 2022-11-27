using HarmonyLib;
using RimWorld.Planet;

namespace caravanVisual;

[HarmonyPatch(typeof(WorldDynamicDrawManager), "DrawDynamicWorldObjects")]
public class Patch_WorldDynamicDrawManager_DrawDynamicWorldObjects
{
    [HarmonyPriority(0)]
    public static void Postfix(Caravan __instance)
    {
        if (caravanVisualMod.instance.Settings.ZoomMode == caravanComponent.en_zoomMode.vanilla)
        {
            return;
        }

        if (!(ExpandableWorldObjectsUtility.TransitionPct >= 1f))
        {
            return;
        }

        foreach (var c in dataUtility.dic_caravan.Keys)
        {
            if (c.def.expandingIcon)
            {
                c.Draw();
            }
        }
    }
}