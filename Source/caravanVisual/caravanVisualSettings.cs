using Verse;

namespace caravanVisual;

/// <summary>
///     Definition of the settings for the mod
/// </summary>
internal class caravanVisualSettings : ModSettings
{
    public int PawnCount;
    public float PawnScale = 1f;
    public bool ShowAnimal = true;
    public float Spacing = 1f;
    public bool SwingAnimation = true;
    public caravanComponent.en_zoomMode ZoomMode = caravanComponent.en_zoomMode.bigLeader;
    public float ZoomScale = 1f;

    /// <summary>
    ///     Saving and loading the values
    /// </summary>
    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref SwingAnimation, "SwingAnimation", true);
        Scribe_Values.Look(ref ShowAnimal, "ShowAnimal", true);
        Scribe_Values.Look(ref ZoomMode, "ZoomMode", caravanComponent.en_zoomMode.bigLeader);
        Scribe_Values.Look(ref PawnCount, "PawnCount");
        Scribe_Values.Look(ref PawnScale, "PawnScale", 1f);
        Scribe_Values.Look(ref ZoomScale, "ZoomScale", 1f);
        Scribe_Values.Look(ref Spacing, "Spacing", 1f);
    }
}