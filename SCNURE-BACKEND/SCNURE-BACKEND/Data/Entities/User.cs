using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SCNURE_BACKEND.Data.Entities
{
    public partial class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            Likes = new HashSet<Like>();
            Notifications = new HashSet<Notification>();
            Participants = new HashSet<Participant>();
            Rewards = new HashSet<Reward>();
            TeamMembers = new HashSet<TeamMember>();
        }

        public int UserId { get; set; }
        public string Verification { get; set; }
        public string Login { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Photo { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public bool ShowPhone { get; set; }
        public DateTime? Birthday { get; set; }
        public bool ShowBirthday { get; set; }
        public DateTime RegisterDate { get; set; }
        public string Email { get; set; }
        public bool ShowEmail { get; set; }
        public bool Admin { get; set; }
        public bool Membership { get; set; }
        public bool Ban { get; set; }

		[JsonIgnore]
        public virtual ICollection<Comment> Comments { get; set; }
		[JsonIgnore]
		public virtual ICollection<Like> Likes { get; set; }
		[JsonIgnore]
		public virtual ICollection<Notification> Notifications { get; set; }
		[JsonIgnore]
		public virtual ICollection<Participant> Participants { get; set; }
		[JsonIgnore]
		public virtual ICollection<Reward> Rewards { get; set; }
		[JsonIgnore]
		public virtual ICollection<TeamMember> TeamMembers { get; set; }
    }
}
