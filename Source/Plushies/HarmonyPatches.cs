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
        public static ThoughtDef PlushieCuddleChild;
        public static ThoughtDef PlushieCuddleChildKind;

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

    // [HarmonyPatch(typeof(Toils_LayDown))]
    // [HarmonyPatch("ApplyBedRelatedEffects")]
    // class BabyBedThoughtsPatch {
    //     static void Postfix(
    //         Pawn p,
    //         Building_Bed bed,
    //         bool asleep,
    //         bool gainRest,
    //         bool deathrest) {
            
    //         Log.Message($"{p.ageTracker.CurLifeStage.developmentalStage} {p.mindState.applyBedThoughtsTick}")
    //         if (p.ageTracker.CurLifeStage.developmentalStage <= DevelopmentalStage.Baby && p.mindState.applyBedThoughtsTick == 0)
    //         {
    //             p.mindState.applyBedThoughtsTick = Find.TickManager.TicksGame + Rand.Range(2500, 10000);
    //             p.mindState.applyBedThoughtsOnLeave = false;
    //         }
    //     }
    // }


    [HarmonyPatch(typeof(Toils_LayDown))]
    [HarmonyPatch("ApplyBedThoughts")]
    class BedThoughtsPatch
    {
        static void Postfix(Pawn actor)
        {
            Log.Message("BedThoughtsPatch");
            // These are here since it seems hard to figure out what properties in Rimworld
            // are actually nullable
            if (actor.needs == null || actor.needs.mood == null || actor.needs.mood.thoughts == null || 
                actor.needs.mood.thoughts.memories == null)
                return;

            Log.Message("Has memories");

            var bed = actor.CurrentBed();
            if (bed == null)
                return;

            Log.Message("Has bed");

            // This can be null in the case of a sleeping spot
            var comp = bed.GetComp<CompAffectedByFacilities>();
            if (comp == null)
                return;

            Log.Message("Has comp");

            //var hasPlushie = comp.LinkedFacilitiesListForReading.Any(thing => thing.def.defName == "Plushie");
            var hasPlushie = comp.LinkedFacilitiesListForReading.Any(thing => thing.def.defName.StartsWith("Plushie"));
            if (hasPlushie) {
                Log.Message("Has plushie");
                if (actor.story != null && actor.story.traits != null && actor.story.traits.HasTrait(TraitDefOf.Kind)) {
                    if (!actor.ageTracker.Adult) {
                        actor.needs.mood.thoughts.memories.TryGainMemory(PlushiesThoughtDefOf.PlushieCuddleChildKind);
                    } else {
                        actor.needs.mood.thoughts.memories.TryGainMemory(PlushiesThoughtDefOf.PlushieCuddleKind);
                    }
                } else {
                    if (!actor.ageTracker.Adult) {
                        actor.needs.mood.thoughts.memories.TryGainMemory(PlushiesThoughtDefOf.PlushieCuddleChild);
                    } else {
                        actor.needs.mood.thoughts.memories.TryGainMemory(PlushiesThoughtDefOf.PlushieCuddle);
                    }
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
                        if (!actor.ageTracker.Adult) {
                            actor.needs.mood.thoughts.memories.TryGainMemory(PlushiesThoughtDefOf.PlushieCuddleChildKind);
                        } else {
                            actor.needs.mood.thoughts.memories.TryGainMemory(PlushiesThoughtDefOf.PlushieCuddleKind);
                        }
                    } else {
                        if (!actor.ageTracker.Adult) {
                            actor.needs.mood.thoughts.memories.TryGainMemory(PlushiesThoughtDefOf.PlushieCuddleChild);
                        } else {
                            actor.needs.mood.thoughts.memories.TryGainMemory(PlushiesThoughtDefOf.PlushieCuddle);
                        }
                    }
                }
            }
        }
    }
}

