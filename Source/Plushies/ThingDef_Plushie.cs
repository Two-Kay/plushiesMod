﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace Plushies
{
    class ThingDef_Plushie : ThingDef
    {
        public override void PostLoad()
        {
            base.PostLoad();
            Log.Message("Yayy plushie code loaded such!");
        }
    }
}
