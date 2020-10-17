using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

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
            // These are here since it seems hard to figure out what properties in Rimworld
            // are actually nullable
            if (actor.needs == null || actor.needs.mood == null || actor.needs.mood.thoughts == null || 
                actor.needs.mood.thoughts.memories == null)
                return;

            var bed = actor.CurrentBed();
            if (bed == null)
                return;

            var facilities = bed.GetComp<CompAffectedByFacilities>().LinkedFacilitiesListForReading;
            var hasPlushie = facilities.Any(thing => thing.def.defName == "Plushie");
            if (hasPlushie) {
                if (actor.story != null && actor.story.traits != null && actor.story.traits.HasTrait(TraitDefOf.Kind)) {
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

    [HarmonyPatch(typeof(Pawn_JobTracker))]
    [HarmonyPatch("EndCurrentJob")]
    class PlushieCuddleJobThoughts
    {
        static void Prefix(JobCondition condition, Pawn_JobTracker __instance, Pawn ___pawn)
        {
            JobDef jobDef = __instance.curJob != null ? __instance.curJob.def : null;
            if (jobDef != null && jobDef.defName == "CuddlePlushie") {
                if (condition == JobCondition.Succeeded) {
                    var actor = ___pawn;
                    if (actor.story.traits.HasTrait(TraitDefOf.Kind)) {
                        actor.needs.mood.thoughts.memories.TryGainMemory(PlushiesThoughtDefOf.PlushieCuddleKind);
                    } else {
                        actor.needs.mood.thoughts.memories.TryGainMemory(PlushiesThoughtDefOf.PlushieCuddle);
                    }
                }
            }
        }
    }
}

