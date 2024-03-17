using System.Reflection;
using HarmonyLib;
using Verse;

namespace caravanVisual;

public class HarmonyPatches : Mod
{
    public HarmonyPatches(ModContentPack content) : base(content)
    {
        new Harmony("com.yayo.caravanVisual").PatchAll(Assembly.GetExecutingAssembly());
    }
}