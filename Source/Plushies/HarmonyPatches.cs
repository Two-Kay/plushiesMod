using HarmonyLib;
using RimWorld;
using Verse;

namespace Plushies
{
    [StaticConstructorOnStartup]
    static class HarmonyPatches
    {
        // this static constructor runs to create a HarmonyInstance and install a patch.
        static HarmonyPatches()
        {
            var harmony = new Harmony("rimworld.cute.plushies");
            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(Toils_LayDown))]
    [HarmonyPatch("ApplyBedThoughts")]
    class BedThoughtsPatch
    {
        static void Postfix()
        {
            Log.Message("Plushies mod saw ApplyBedThoughts run!");
        }
    }
}

