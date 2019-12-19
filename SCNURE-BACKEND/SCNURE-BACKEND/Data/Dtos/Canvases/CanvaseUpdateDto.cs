using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCNURE_BACKEND.Data.Dtos.Canvases
{
	public class CanvaseUpdateDto
	{
		[Required]
		public int StartupId { get; set; }
		[Required]
		public string Problem { get; set; }
		[Required]
		public string Solution { get; set; }
		[Required]
		public string CustomerSegments { get; set; }
		[Required]
		public string Uvp { get; set; }
		[Required]
		public string Kpi { get; set; }
		[Required]
		public string AnfairAdvantage { get; set; }
		[Required]
		public string Channels { get; set; }
		[Required]
		public string RevenueStreams { get; set; }
		[Required]
		public string CostStructure { get; set; }
	}
}
