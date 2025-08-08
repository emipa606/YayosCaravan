using HarmonyLib;
using RimWorld.Planet;

namespace caravanVisual.HarmonyPatches;

[HarmonyPatch(typeof(Caravan), "TickInterval")]
public class Caravan_TickInterval
{
    private static caravanData caravanData;

    [HarmonyPriority(0)]
    public static void Postfix(Caravan __instance, int delta)
    {
        caravanData = dataUtility.GetData(__instance);
        caravanData.TryAddPrevPos(delta);
    }
}