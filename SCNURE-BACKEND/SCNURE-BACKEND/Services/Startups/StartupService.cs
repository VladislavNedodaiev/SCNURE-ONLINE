﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SCNURE_BACKEND.Data;
using SCNURE_BACKEND.Data.Dtos.Comments;
using SCNURE_BACKEND.Data.Dtos.Mappers;
using SCNURE_BACKEND.Data.Dtos.Startups;
using SCNURE_BACKEND.Data.Dtos.TeamMembers;
using SCNURE_BACKEND.Data.Entities;
using SCNURE_BACKEND.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCNURE_BACKEND.Services.Users
{
    public interface IStartupService
    {
        Task<Data.Entities.Startup> GetStartupById(int startupId);
        Task<List<Data.Entities.Startup>> GetAllStartups();
		Task<List<Data.Entities.Startup>> GetMyStartups(int userId);
		Task RateStartup(RateStartupDto rateStartupDtom, int userId);
		Task RemoveRate(int startupId, int userId);
        Task<Data.Entities.Startup> AddStartup(string title, string description, User creator);
        Task<Data.Entities.Startup> UpdateStartup(Data.Entities.Startup startup);
        Task<List<TeamMember>> GetTeamMembers(int startupId);
        Task EditTeamMember(EditTeamMemberRequest editTeamMemberRequest, User editor);
		Task AddTeamMember(AddTeamMemberRequest addTeamMemberRequest);
		Task RemoveTeamMember(int userId, int startupId);
		Task<ResponseComment> AddComment(int startupId, int userId, string text);
	}

    public class StartupServiceImpl : IStartupService
    {
        private readonly SCContext dbContext;

        public StartupServiceImpl(SCContext sCContext)
        {
            dbContext = sCContext;
        }

        public async Task<Data.Entities.Startup> GetStartupById(int startupId)
        {
            return await dbContext.Startups.Include(s => s.Likes).SingleOrDefaultAsync(s => s.StartupId == startupId);
        }

        public async Task<List<Data.Entities.Startup>> GetAllStartups()
        {
            return await dbContext.Startups
				.Include(s => s.Likes)
				.AsQueryable()
				.ToListAsync();
        }

        public async Task<Data.Entities.Startup> AddStartup(string title, string description, User creator)
        {
            dbContext.Users.Update(creator);
            var dbStartup = new Data.Entities.Startup()
            {
                Title = title,
                Description = description
            };
            var addedStartup = dbContext.Startups.Add(dbStartup).Entity;
            var dbTeamMember = new Data.Entities.TeamMember()
            {
                StartupId = addedStartup.StartupId,
                UserId = creator.UserId,
                Role = "",
                EditAccess = true
            };
            dbContext.TeamMembers.Add(dbTeamMember);
            await dbContext.SaveChangesAsync();
            return dbContext.Startups.Find(addedStartup.StartupId);
        }

        public async Task<Data.Entities.Startup> UpdateStartup(Data.Entities.Startup startup)
        {
            var result = dbContext.Startups.Update(startup).Entity;
            await dbContext.SaveChangesAsync();
            return result;
        }

        public async Task<List<TeamMember>> GetTeamMembers(int startupId)
        {
            return await dbContext
                .TeamMembers
                .AsQueryable()
                .Where(
                    teamMember => teamMember.StartupId == startupId
                )
                .ToListAsync();
        }

        public async Task EditTeamMember(EditTeamMemberRequest editTeamMemberRequest, User editor)
        {
            var teamMembers = await GetTeamMembers(editTeamMemberRequest.StartupId);
            var editorAsTeamMember = teamMembers.FirstOrDefault(x => x.UserId == editor.UserId);

            if (!editorAsTeamMember.EditAccess && !editor.Admin)
                throw new ArgumentException("NO_EDIT_ACCESS");

            var userAsTeamMember = editorAsTeamMember;
            if (editor.UserId != editTeamMemberRequest.UserId)
            {
                userAsTeamMember = teamMembers.FirstOrDefault(x => x.UserId == editTeamMemberRequest.UserId);
            }

            userAsTeamMember.Role = editTeamMemberRequest.Role;
            userAsTeamMember.EditAccess = editTeamMemberRequest.HasEditAccess;
            dbContext.SaveChanges();
        }

		public async Task AddTeamMember(AddTeamMemberRequest addTeamMemberRequest)
		{
			var user = await dbContext.Users.SingleOrDefaultAsync(u => u.Login == addTeamMemberRequest.Login);
			if (user == null)
				throw new ArgumentException("User was not found");

			var teamMember = new TeamMember
			{
				EditAccess = addTeamMemberRequest.HasEditAccess,
				Role = addTeamMemberRequest.Role,
				StartupId = addTeamMemberRequest.StartupId,
				UserId = user.UserId
			};

			await dbContext.TeamMembers.AddAsync(teamMember);
			await dbContext.SaveChangesAsync();
		}

		public async Task RemoveTeamMember(int userId, int startupId)
		{
			var user = await dbContext.Users.SingleOrDefaultAsync(u => u.UserId == userId);
			if (user == null)
				throw new ArgumentException("User was not found");

			var teamMember = await dbContext.TeamMembers.FindAsync(startupId, userId);

			dbContext.TeamMembers.Remove(teamMember);
			await dbContext.SaveChangesAsync();
		}

		public async Task<List<Data.Entities.Startup>> GetMyStartups(int userId)
		{
			var user = await dbContext.Users.FindAsync(userId);
			if (user == null)
				throw new ArgumentException("User wasn't found");

			return await dbContext.Startups
				.Include(s => s.Likes)
				.Include(s => s.TeamMembers)
				.Where(s => s.TeamMembers.Any(tm => tm.UserId == userId))
				.ToListAsync();
		}

		public async Task RateStartup(RateStartupDto rateStartupDto, int userId)
		{
			var user = await dbContext.Users.FindAsync(userId);
			if (user == null)
				throw new ArgumentException("User wasn't found");

			var startup = await dbContext.Startups.FindAsync(rateStartupDto.StartupId);
			if (startup == null)
				throw new ArgumentException("Startup wasn't found");

			string newRateValue = rateStartupDto.IsRatePositive ? LikeType.Like : LikeType.Dislike;
			var rate = await dbContext.Likes.SingleOrDefaultAsync(l => l.UserId == userId && l.StartupId == startup.StartupId);
			
			if (rate != null && rate.Value == newRateValue)
				throw new ArgumentException("You have already rated this startup");

			if (rate != null)
			{
				rate.Value = newRateValue;

				dbContext.Update(rate);
				await dbContext.SaveChangesAsync();
			}
			else
			{

				var like = new Like
				{
					StartupId = startup.StartupId,
					UserId = user.UserId,
					Value = newRateValue
				};

				await dbContext.Likes.AddAsync(like);
				await dbContext.SaveChangesAsync();
			}
		}

		public async Task RemoveRate(int startupId, int userId)
		{
			var user = await dbContext.Users.FindAsync(userId);
			if (user == null)
				throw new ArgumentException("User wasn't found");

			var startup = await dbContext.Startups.FindAsync(startupId);
			if (startup == null)
				throw new ArgumentException("Startup wasn't found");

			var like = await dbContext.Likes.SingleOrDefaultAsync(l => l.StartupId == startupId && l.UserId == userId);

			dbContext.Likes.Remove(like);
			await dbContext.SaveChangesAsync();
		}

		public async Task<ResponseComment> AddComment(int startupId, int userId, string text)
		{
			var user = await dbContext.Users.FindAsync(userId);
			if (user == null)
				throw new ArgumentException("User wasn't found");

			var startup = await dbContext.Startups.FindAsync(startupId);
			if (startup == null)
				throw new ArgumentException("Startup wasn't found");

			var dbComment = new Comment()
			{
				UserId = userId,
				StartupId = startupId,
				Text = text,
			};

			dbContext.Comments.Add(dbComment);

			return new ResponseComment(user, dbComment);
		}
	}
}
