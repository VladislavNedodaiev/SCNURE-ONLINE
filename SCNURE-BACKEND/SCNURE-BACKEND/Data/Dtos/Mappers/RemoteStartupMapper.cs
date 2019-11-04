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

        public ClientStartup MapRemoteStartup(Data.Entities.Startup remoteStartup)
        {
            var result = new ClientStartup()
            {
                StartupId = remoteStartup.StartupId,
                Title = remoteStartup.Title,
                FoundationYear = remoteStartup.FoundationYear,
                PublicationDate = remoteStartup.PublicationDate,
                Photo = remoteStartup.Photo, //TODO: use actual url
                Description = remoteStartup.Description,
                Website = remoteStartup.Website,
                Phone = remoteStartup.Phone,
                Email = remoteStartup.Email
            };
            return result;
        }

        public List<ClientStartup> MapRemoteStartups(IEnumerable<Entities.Startup> remoteStartups)
        {
            return remoteStartups.Select(x => MapRemoteStartup(x)).ToList();
        }
    }
}
