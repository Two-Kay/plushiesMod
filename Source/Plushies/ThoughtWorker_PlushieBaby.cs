using Verse;
using RimWorld;

namespace Plushies
{
  public class ThoughtWorker_PlushieBaby : ThoughtWorker
  {
    protected override ThoughtState CurrentStateInternal(Pawn p)
    {
      if (!ModsConfig.BiotechActive)
        return (ThoughtState) false;


      Building_Bed bed = p.CurrentBed();
      if (bed == null || !bed.def.building.bed_crib)
        return (ThoughtState) false;

      var comp = bed.GetComp<CompAffectedByFacilities>();
      if (comp == null)
          return (ThoughtState) false;

      var hasPlushie = comp.LinkedFacilitiesListForReading.Any(thing => thing.def.defName.StartsWith("Plushie"));
      if (hasPlushie) {
        return ThoughtState.ActiveDefault;
      } else {
        return (ThoughtState) false;
      }
    }
  }
}
