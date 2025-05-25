using System.Reflection;
using HarmonyLib;
using Verse;

namespace caravanVisual;

[StaticConstructorOnStartup]
public static class Main
{
    static Main()
    {
        new Harmony("com.yayo.caravanVisual").PatchAll(Assembly.GetExecutingAssembly());
    }
}