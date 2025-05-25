using HarmonyLib;
using RimWorld.Planet;

namespace caravanVisual.HarmonyPatches;

[HarmonyPatch(typeof(Caravan), nameof(Caravan.Tick))]
public class Caravan_Tick
{
    private static caravanData caravanData;

    [HarmonyPriority(0)]
    public static void Postfix(Caravan __instance)
    {
        caravanData = dataUtility.GetData(__instance);
        caravanData.TryAddPrevPos();
    }
}