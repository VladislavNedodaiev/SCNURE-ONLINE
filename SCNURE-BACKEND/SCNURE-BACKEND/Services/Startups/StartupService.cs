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
        Task<Data.Entities.Startup> AddStartup(string title, string description, User creator);
        Task<Data.Entities.Startup> UpdateStartup(Data.Entities.Startup startup);
        Task<List<Data.Entities.TeamMember>> GetTeamMembers(int startupId);
        Task EditTeamMember(EditTeamMemberRequest editTeamMemberRequest, User editor);
    }

    public class StartupServiceImpl : IStartupService
    {
        private readonly SCContext _dbcontext;

        public StartupServiceImpl(SCContext sCContext)
        {
            _dbcontext = sCContext;
        }

        public Task<Data.Entities.Startup> GetStartupById(int startupId)
        {
            return _dbcontext.Startups.FindAsync(startupId);
        }

        public async Task<List<Data.Entities.Startup>> GetAllStartups()
        {
            return _dbcontext.Startups.AsQueryable().ToList();
        }

        public async Task<Data.Entities.Startup> AddStartup(string title, string description, Data.Entities.User creator)
        {
            _dbcontext.Users.Update(creator);
            var dbStartup = new Data.Entities.Startup()
            {
                Title = title,
                Description = description
            };
            var addedStartup = _dbcontext.Startups.Add(dbStartup).Entity;
            var dbTeamMember = new Data.Entities.TeamMember()
            {
                StartupId = addedStartup.StartupId,
                UserId = creator.UserId,
                Role = "",
                EditAccess = true
            };
            _dbcontext.TeamMembers.Add(dbTeamMember);
            _dbcontext.SaveChanges();
            return _dbcontext.Startups.Find(addedStartup.StartupId);
        }

        public async Task<Data.Entities.Startup> UpdateStartup(Data.Entities.Startup startup)
        {
            var result = _dbcontext.Startups.Update(startup).Entity;
            _dbcontext.SaveChanges();
            return result;
        }

        public async Task<List<Data.Entities.TeamMember>> GetTeamMembers(int startupId)
        {
            return _dbcontext
                .TeamMembers
                .AsQueryable()
                .Where(
                    teamMember => teamMember.StartupId == startupId
                )
                .ToList();
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
            _dbcontext.SaveChanges();
        }
    }
}
