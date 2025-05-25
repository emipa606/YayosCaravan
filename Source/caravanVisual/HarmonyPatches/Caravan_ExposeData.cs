using HarmonyLib;
using RimWorld.Planet;

namespace caravanVisual.HarmonyPatches;

[HarmonyPatch(typeof(Caravan), nameof(Caravan.ExposeData))]
public class Caravan_ExposeData
{
    private static void Postfix(Caravan __instance)
    {
        dataUtility.GetData(__instance).ExposeData();
    }
}