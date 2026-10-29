using HarmonyLib;
using RimWorld.Planet;

namespace caravanVisual.HarmonyPatches;

[HarmonyPatch(typeof(Caravan), nameof(Caravan.ExposeData))]
public static class Caravan_ExposeData
{
    public static void Postfix(Caravan __instance)
    {
        dataUtility.GetData(__instance).ExposeData();
    }
}