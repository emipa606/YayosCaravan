using HarmonyLib;
using RimWorld.Planet;

namespace caravanVisual.HarmonyPatches;

[HarmonyPatch(typeof(WorldDynamicDrawManager), "DrawDynamicWorldObjects")]
public class WorldDynamicDrawManager_DrawDynamicWorldObjects
{
    [HarmonyPriority(0)]
    public static void Postfix()
    {
        if (!caravanVisualMod.instance.Settings.ToggleVisibility)
        {
            return;
        }

        if (caravanVisualMod.instance.Settings.ZoomMode == caravanComponent.ZoomMode.vanilla)
        {
            return;
        }

        foreach (var c in dataUtility.CaravanDatas.Keys)
        {
            if (c.def.expandingIcon)
            {
                c.Draw();
            }
        }
    }
}