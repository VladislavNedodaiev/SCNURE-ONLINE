using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCNURE_BACKEND.Data.Dtos.Startups
{
	public class RateStartupDto
	{
		public int StartupId { get; set; }
		public bool IsRatePositive { get; set; }
	}
}
