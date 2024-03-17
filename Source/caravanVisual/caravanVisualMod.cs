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
            if (settings == null)
            {
                settings = GetSettings<caravanVisualSettings>();
            }

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
        var listing_Standard = new Listing_Standard();
        listing_Standard.Begin(rect);
        listing_Standard.Gap();
        listing_Standard.CheckboxLabeled("caravanVisual_swinganimation".Translate(), ref Settings.SwingAnimation,
            "caravanVisual_swinganimation_tooltip".Translate());
        listing_Standard.CheckboxLabeled("caravanVisual_showanimal".Translate(), ref Settings.ShowAnimal,
            "caravanVisual_showanimal_tooltip".Translate());
        listing_Standard.Gap();
        listing_Standard.Label("caravanVisual_zoomoutmode".Translate(), -1f,
            "caravanVisual_zoomoutmode_tooltip".Translate());
        foreach (var zoomMode in (caravanComponent.en_zoomMode[])Enum.GetValues(typeof(caravanComponent.en_zoomMode)))
        {
            if (listing_Standard.RadioButton(zoomMode.ToString().CapitalizeFirst(), zoomMode == Settings.ZoomMode))
            {
                Settings.ZoomMode = zoomMode;
            }
        }

        listing_Standard.Gap();
        Settings.PawnCount =
            (int)Math.Round(listing_Standard.SliderLabeled("caravanVisual_maxpawncount".Translate(Settings.PawnCount),
                Settings.PawnCount,
                0, 25, 0.5F, "caravanVisual_maxpawncount_tooltip".Translate()), 1);
        listing_Standard.Gap();
        Settings.PawnScale = (float)Math.Round((decimal)listing_Standard.SliderLabeled(
            "caravanVisual_pawnscale".Translate(Settings.PawnScale.ToStringPercent()), Settings.PawnScale,
            0f, 2f, 0.5F, "caravanVisual_pawnscale_tooltip".Translate()), 3);
        listing_Standard.Gap();
        Settings.ZoomScale = (float)Math.Round((decimal)listing_Standard.SliderLabeled(
            "caravanVisual_pawnzoomoutscale".Translate(Settings.ZoomScale.ToStringPercent()), Settings.ZoomScale,
            0f, 2f, 0.5F, "caravanVisual_pawnzoomoutscale_tooltip".Translate()), 3);
        listing_Standard.Gap();
        Settings.Spacing = (float)Math.Round((decimal)listing_Standard.SliderLabeled(
            "caravanVisual_spacingbetweenpawns".Translate(Settings.Spacing.ToStringPercent()), Settings.Spacing,
            0f, 5f, 0.5F, "caravanVisual_spacingbetweenpawns_tooltip".Translate()), 2);

        if (currentVersion != null)
        {
            listing_Standard.Gap();
            GUI.contentColor = Color.gray;
            listing_Standard.Label("caravanVisual_ModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listing_Standard.End();
    }
}