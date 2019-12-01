using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SCNURE_BACKEND.Data;
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
        Task<Data.Entities.Startup> AddStartup(string title, string description, User creator);
        Task<Data.Entities.Startup> UpdateStartup(Data.Entities.Startup startup);
        Task<List<TeamMember>> GetTeamMembers(int startupId);
        Task EditTeamMember(EditTeamMemberRequest editTeamMemberRequest, User editor);
		Task AddTeamMember(AddTeamMemberRequest addTeamMemberRequest);
		Task RemoveTeamMember(int userId, int startupId);
	}

    public class StartupServiceImpl : IStartupService
    {
        private readonly SCContext dbcontext;

        public StartupServiceImpl(SCContext sCContext)
        {
            dbcontext = sCContext;
        }

        public Task<Data.Entities.Startup> GetStartupById(int startupId)
        {
            return dbcontext.Startups.FindAsync(startupId);
        }

        public async Task<List<Data.Entities.Startup>> GetAllStartups()
        {
            return await dbcontext.Startups.AsQueryable().ToListAsync();
        }

        public async Task<Data.Entities.Startup> AddStartup(string title, string description, Data.Entities.User creator)
        {
            dbcontext.Users.Update(creator);
            var dbStartup = new Data.Entities.Startup()
            {
                Title = title,
                Description = description
            };
            var addedStartup = dbcontext.Startups.Add(dbStartup).Entity;
            var dbTeamMember = new Data.Entities.TeamMember()
            {
                StartupId = addedStartup.StartupId,
                UserId = creator.UserId,
                Role = "",
                EditAccess = true
            };
            dbcontext.TeamMembers.Add(dbTeamMember);
            await dbcontext.SaveChangesAsync();
            return dbcontext.Startups.Find(addedStartup.StartupId);
        }

        public async Task<Data.Entities.Startup> UpdateStartup(Data.Entities.Startup startup)
        {
            var result = dbcontext.Startups.Update(startup).Entity;
            await dbcontext.SaveChangesAsync();
            return result;
        }

        public async Task<List<Data.Entities.TeamMember>> GetTeamMembers(int startupId)
        {
            return await dbcontext
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
            dbcontext.SaveChanges();
        }

		public async Task AddTeamMember(AddTeamMemberRequest addTeamMemberRequest)
		{
			var user = await dbcontext.Users.SingleOrDefaultAsync(u => u.Login == addTeamMemberRequest.Login);
			if (user == null)
				throw new ArgumentException("User was not found");

			var teamMember = new TeamMember
			{
				EditAccess = addTeamMemberRequest.HasEditAccess,
				Role = addTeamMemberRequest.Role,
				StartupId = addTeamMemberRequest.StartupId,
				UserId = user.UserId
			};

			await dbcontext.TeamMembers.AddAsync(teamMember);
			await dbcontext.SaveChangesAsync();
		}

		public async Task RemoveTeamMember(int userId, int startupId)
		{
			var user = await dbcontext.Users.SingleOrDefaultAsync(u => u.UserId == userId);
			if (user == null)
				throw new ArgumentException("User was not found");

			var teamMember = await dbcontext.TeamMembers.FindAsync(startupId, userId);

			dbcontext.TeamMembers.Remove(teamMember);
			await dbcontext.SaveChangesAsync();
		}

		public async Task<List<Data.Entities.Startup>> GetMyStartups(int userId)
		{
			var user = await dbcontext.Users.FindAsync(userId);
			if (user == null)
				throw new ArgumentException("User wasn't found");

			return await dbcontext.Startups
				.Include(s => s.TeamMembers)
				.Where(s => s.TeamMembers.Any(tm => tm.UserId == userId))
				.ToListAsync();
		}
	}
}
