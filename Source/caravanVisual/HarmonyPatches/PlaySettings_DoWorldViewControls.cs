using HarmonyLib;
using RimWorld;
using Verse;

namespace caravanVisual.HarmonyPatches;

[HarmonyPatch(typeof(PlaySettings), nameof(PlaySettings.DoPlaySettingsGlobalControls))]
public class PlaySettings_DoWorldViewControls
{
    public static void Postfix(ref WidgetRow row, bool worldView)
    {
        if (!worldView)
        {
            return;
        }

        row.ToggleableIcon(ref caravanVisualMod.instance.Settings.ToggleVisibility,
            TexCommand.GatherSpotActive, "caravanVisual_toggleVisibility".Translate());
    }
}