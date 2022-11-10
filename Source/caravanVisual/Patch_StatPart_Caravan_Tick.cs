using HarmonyLib;
using RimWorld.Planet;

namespace caravanVisual;

[HarmonyPatch(typeof(Caravan), "Tick")]
public class Patch_StatPart_Caravan_Tick
{
    private static caravanData cd;

    [HarmonyPriority(0)]
    public static void Postfix(Caravan __instance)
    {
        cd = dataUtility.GetData(__instance);
        cd.tryAddPrevPos();
    }
}