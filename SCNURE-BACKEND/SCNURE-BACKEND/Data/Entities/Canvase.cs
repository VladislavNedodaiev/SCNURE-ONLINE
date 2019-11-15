using System;
using System.Collections.Generic;

namespace SCNURE_BACKEND.Data.Entities
{
    public partial class Canvase
    {
        public int StartupId { get; set; }
        public string Problem { get; set; }
        public string Solution { get; set; }
        public string CustomerSegments { get; set; }
        public string Uvp { get; set; }
        public string Kpi { get; set; }
        public string AnfairAdvantage { get; set; }
        public string Channels { get; set; }
        public string RevenueStreams { get; set; }
        public string CostStructure { get; set; }

        public virtual Startup Startup { get; set; }
    }
}
