﻿using SCNURE_BACKEND.Data.Dtos;
using SCNURE_BACKEND.Data.Dtos.Startups;
using SCNURE_BACKEND.Helpers;
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

        public StartupResponseDto MapRemoteStartup(Data.Entities.Startup remoteStartup, int likesCount, int dislikeCount, int currentUserRate)
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
                Email = remoteStartup.Email,
				LikesCount = likesCount,
				DislikesCount = dislikeCount,
				CurrentUserRate = currentUserRate
            };
            return result;
        }

        public List<StartupResponseDto> MapRemoteStartups(IEnumerable<Entities.Startup> remoteStartups)
        {
			return remoteStartups.Select(x =>
			{
				int likesCount = x.Likes.Where(l => l.Value == LikeType.Like).Count();
				int dislikesCount = x.Likes.Where(l => l.Value == LikeType.Dislike).Count();
				return MapRemoteStartup(x, likesCount, dislikesCount, 0);
			})
			.ToList();
        }
    }
}
