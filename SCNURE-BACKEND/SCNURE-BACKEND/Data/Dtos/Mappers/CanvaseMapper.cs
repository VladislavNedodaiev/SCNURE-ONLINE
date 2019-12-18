using SCNURE_BACKEND.Data.Dtos.Canvases;
using SCNURE_BACKEND.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCNURE_BACKEND.Data.Dtos.Mappers
{
	public static class CanvaseMapper
	{
		public static void UpdateCanvaseFromDto(this Canvase canvase, CanvaseUpdateDto dto)
		{
			canvase.AnfairAdvantage = dto.AnfairAdvantage;
			canvase.Channels = dto.Channels;
			canvase.CostStructure = dto.CostStructure;
			canvase.CustomerSegments = dto.CustomerSegments;
			canvase.Kpi = dto.Kpi;
			canvase.Problem = dto.Problem;
			canvase.RevenueStreams = dto.RevenueStreams;
			canvase.Solution = dto.Solution;
			canvase.Uvp = dto.Uvp;
		}

		public static CanvaseDto ToDto(this Canvase canvase)
		{
			return new CanvaseDto()
			{
				AnfairAdvantage = canvase.AnfairAdvantage,
				Uvp = canvase.Uvp,
				StartupId = canvase.StartupId,
				Solution = canvase.Solution,
				Channels = canvase.Channels,
				CostStructure = canvase.CostStructure,
				CustomerSegments = canvase.CustomerSegments,
				Kpi = canvase.Kpi,
				Problem = canvase.Problem,
				RevenueStreams = canvase.RevenueStreams
			};
		}
	}
}
