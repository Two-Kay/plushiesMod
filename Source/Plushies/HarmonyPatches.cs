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

    [DefOf]
    public static class PlushiesRulePackDefOf
    {
        public static RulePackDef ArtDescription_Plushie;

        static PlushiesRulePackDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(PlushiesRulePackDefOf));
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

            var bed = actor.CurrentBed();
            var facilities = bed.GetComp<CompAffectedByFacilities>().LinkedFacilitiesListForReading;
            var hasPlushie = facilities.Any(thing => thing.def.defName == "Plushie");
            if (hasPlushie) {
                if (actor.story.traits.HasTrait(TraitDefOf.Kind)) {
                    actor.needs.mood.thoughts.memories.TryGainMemory(PlushiesThoughtDefOf.PlushieCuddleKind);
                } else {
                    actor.needs.mood.thoughts.memories.TryGainMemory(PlushiesThoughtDefOf.PlushieCuddle);
                }
            }
        }
    }

    [HarmonyPatch(typeof(TaleTextGenerator))]
    [HarmonyPatch("GenerateTextFromTale")]
    class RemoveTaleFromPlushies
    {
        static void Prefix(ref Tale tale, RulePackDef extraInclude)
        {
            if (extraInclude.defName == "ArtDescription_Plushie") {
                tale = null;
            }
        }
    }
}

