using System.Reflection;
using HarmonyLib;
using Verse;

namespace caravanVisual;

public class HarmonyPatches : Mod
{
    public HarmonyPatches(ModContentPack content) : base(content)
    {
        var harmony = new Harmony("com.yayo.caravanVisual");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }
}