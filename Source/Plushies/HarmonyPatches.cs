using HarmonyLib;
using RimWorld;
using Verse;

namespace Plushies
{
    [DefOf]
    public static class PlushiesThingDefOf
    {
        public static ThingDef Plushie;

        static PlushiesThingDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(PlushiesThingDefOf));
        }
    }

    [DefOf]
    public static class PlushiesThoughtDefOf
    {
        public static ThoughtDef PlushieCuddle;

        static PlushiesThoughtDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(PlushiesThoughtDefOf));
        }
    }

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
    // public IEnumerable<Building_Bed> ContainedBeds
    // {
    //   get
    //   {
    //     List<Thing> things = this.ContainedAndAdjacentThings;
    //     for (int i = 0; i < things.Count; ++i)
    //     {
    //       if (things[i] is Building_Bed buildingBed)
    //         yield return buildingBed;
    //     }
    //   }
    // }
        static void Postfix(Pawn actor)
        {
            if (actor.needs.mood == null)
                return;

            Log.Message("Plushies mod saw ApplyBedThoughts run!");
            var roomThings = actor.GetRoom().ContainedAndAdjacentThings;
            var hasPlushie = roomThings.Any(thing => thing.def.defName == "Plushie");
            Log.Message($"Room has plushie? {hasPlushie}");
            if (hasPlushie) {
                actor.needs.mood.thoughts.memories.TryGainMemory(PlushiesThoughtDefOf.PlushieCuddle);
            }
        }
    }
}

