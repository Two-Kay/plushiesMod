using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;
using RimWorld;

namespace Plushies
{
    class JoyGiver_CuddlePlushie : JoyGiver
    {
		private static List<Thing> candidates = new List<Thing>();
		public override Job TryGiveJob(Pawn pawn)
		{
			bool allowedOutside = JoyUtility.EnjoyableOutsideNow(pawn, null);
			Job result;
			try
			{
				JoyGiver_CuddlePlushie.candidates.AddRange(pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Art).Where(delegate (Thing thing)
				{
					if (thing.Faction != Faction.OfPlayer || thing.IsForbidden(pawn) || (!allowedOutside && !thing.Position.Roofed(thing.Map)) || !pawn.CanReserveAndReach(thing, PathEndMode.Touch, Danger.None, 1, -1, null, false) || !thing.IsPoliticallyProper(pawn))
					{
						return false;
					}
					CompArt compArt = thing.TryGetComp<CompArt>();
					if (compArt == null)
					{
						Log.Error("No CompArt on thing being considered for viewing: " + thing, false);
						return false;
					}
					if (thing.def.defName != "Plushie")
					{
						return false;
					}
					Room room = thing.GetRoom(RegionType.Set_Passable);
					return room != null && ((room.Role != RoomRoleDefOf.Bedroom && room.Role != RoomRoleDefOf.Barracks && room.Role != RoomRoleDefOf.PrisonCell && room.Role != RoomRoleDefOf.PrisonBarracks && room.Role != RoomRoleDefOf.Hospital) || (pawn.ownership != null && pawn.ownership.OwnedRoom != null && pawn.ownership.OwnedRoom == room));
				}));
				Thing t;
				if (!JoyGiver_CuddlePlushie.candidates.TryRandomElementByWeight((Thing target) => Math.Max(target.GetStatValue(StatDefOf.Beauty, true), 0.5f), out t))
				{
					result = null;
				}
				else
				{
					result = JobMaker.MakeJob(this.def.jobDef, t);
				}
			}
			finally
			{
				JoyGiver_CuddlePlushie.candidates.Clear();
			}
			return result;
		}
	}
}
