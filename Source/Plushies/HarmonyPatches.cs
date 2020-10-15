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
        public static ThoughtDef PlushieCuddleKind;

        static PlushiesThoughtDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(PlushiesThoughtDefOf));
        }
    }

    [StaticConstructorOnStartup]
    static class HarmonyPatches
    {
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
        static void Postfix(Pawn actor)
        {
            if (actor.needs.mood == null)
                return;

            var roomThings = actor.GetRoom().ContainedAndAdjacentThings;
            var hasPlushie = roomThings.Any(thing => thing.def.defName == "Plushie");
            if (hasPlushie) {
                if (actor.story.traits.HasTrait(TraitDefOf.Kind)) {
                    actor.needs.mood.thoughts.memories.TryGainMemory(PlushiesThoughtDefOf.PlushieCuddleKind);
                } else {
                    actor.needs.mood.thoughts.memories.TryGainMemory(PlushiesThoughtDefOf.PlushieCuddle);
                }
            }
        }
    }
}

