﻿using SCNURE_BACKEND.Data.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCNURE_BACKEND.Data.Entities.ClientEntities.Startup
{
    public class RemoteStartupMapper
    {
        public RemoteStartupMapper()
        {

        }

        public StartupResponseDto MapRemoteStartup(Data.Entities.Startup remoteStartup)
        {
            var result = new StartupResponseDto()
            {
                StartupId = remoteStartup.StartupId,
                Title = remoteStartup.Title,
                FoundationYear = remoteStartup.FoundationYear,
                PublicationDate = remoteStartup.PublicationDate,
                Photo = remoteStartup.Photo,
                Description = remoteStartup.Description,
                Website = remoteStartup.Website,
                Phone = remoteStartup.Phone,
                Email = remoteStartup.Email
            };
            return result;
        }

        public List<StartupResponseDto> MapRemoteStartups(IEnumerable<Entities.Startup> remoteStartups)
        {
            return remoteStartups.Select(x => MapRemoteStartup(x)).ToList();
        }
    }
}
