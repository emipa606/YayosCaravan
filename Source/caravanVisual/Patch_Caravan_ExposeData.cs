using HarmonyLib;
using RimWorld.Planet;

namespace caravanVisual;

[HarmonyPatch(typeof(Caravan))]
[HarmonyPatch("ExposeData")]
public class Patch_Caravan_ExposeData
{
    private static void Postfix(Caravan __instance)
    {
        dataUtility.GetData(__instance).ExposeData();
    }
}