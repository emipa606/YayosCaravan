using System;
using Mlie;
using UnityEngine;
using Verse;

namespace caravanVisual;

[StaticConstructorOnStartup]
internal class caravanVisualMod : Mod
{
    /// <summary>
    ///     The instance of the settings to be read by the mod
    /// </summary>
    public static caravanVisualMod instance;

    private static string currentVersion;

    /// <summary>
    ///     The private settings
    /// </summary>
    private caravanVisualSettings settings;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="content"></param>
    public caravanVisualMod(ModContentPack content) : base(content)
    {
        instance = this;
        currentVersion =
            VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    /// <summary>
    ///     The instance-settings for the mod
    /// </summary>
    internal caravanVisualSettings Settings
    {
        get
        {
            settings ??= GetSettings<caravanVisualSettings>();

            return settings;
        }
        set => settings = value;
    }

    /// <summary>
    ///     The title for the mod-settings
    /// </summary>
    /// <returns></returns>
    public override string SettingsCategory()
    {
        return "Yayos Caravan";
    }

    /// <summary>
    ///     The settings-window
    ///     For more info: https://rimworldwiki.com/wiki/Modding_Tutorials/ModSettings
    /// </summary>
    /// <param name="rect"></param>
    public override void DoSettingsWindowContents(Rect rect)
    {
        var listingStandard = new Listing_Standard();
        listingStandard.Begin(rect);
        listingStandard.Gap();
        listingStandard.CheckboxLabeled("caravanVisual_swinganimation".Translate(), ref Settings.SwingAnimation,
            "caravanVisual_swinganimation_tooltip".Translate());
        listingStandard.CheckboxLabeled("caravanVisual_showanimal".Translate(), ref Settings.ShowAnimal,
            "caravanVisual_showanimal_tooltip".Translate());
        listingStandard.Gap();
        listingStandard.Label("caravanVisual_zoomoutmode".Translate(), -1f,
            "caravanVisual_zoomoutmode_tooltip".Translate());
        foreach (var zoomMode in (caravanComponent.ZoomMode[])Enum.GetValues(typeof(caravanComponent.ZoomMode)))
        {
            if (listingStandard.RadioButton(zoomMode.ToString().CapitalizeFirst(), zoomMode == Settings.ZoomMode))
            {
                Settings.ZoomMode = zoomMode;
            }
        }

        listingStandard.Gap();
        Settings.PawnCount =
            (int)Math.Round(listingStandard.SliderLabeled("caravanVisual_maxpawncount".Translate(Settings.PawnCount),
                Settings.PawnCount,
                0, 25, 0.5F, "caravanVisual_maxpawncount_tooltip".Translate()), 1);
        listingStandard.Gap();
        Settings.PawnScale = (float)Math.Round((decimal)listingStandard.SliderLabeled(
            "caravanVisual_pawnscale".Translate(Settings.PawnScale.ToStringPercent()), Settings.PawnScale,
            0f, 2f, 0.5F, "caravanVisual_pawnscale_tooltip".Translate()), 3);
        listingStandard.Gap();
        Settings.ZoomScale = (float)Math.Round((decimal)listingStandard.SliderLabeled(
            "caravanVisual_pawnzoomoutscale".Translate(Settings.ZoomScale.ToStringPercent()), Settings.ZoomScale,
            0f, 2f, 0.5F, "caravanVisual_pawnzoomoutscale_tooltip".Translate()), 3);
        listingStandard.Gap();
        Settings.Spacing = (float)Math.Round((decimal)listingStandard.SliderLabeled(
            "caravanVisual_spacingbetweenpawns".Translate(Settings.Spacing.ToStringPercent()), Settings.Spacing,
            0f, 5f, 0.5F, "caravanVisual_spacingbetweenpawns_tooltip".Translate()), 2);

        if (currentVersion != null)
        {
            listingStandard.Gap();
            GUI.contentColor = Color.gray;
            listingStandard.Label("caravanVisual_ModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard.End();
    }
}