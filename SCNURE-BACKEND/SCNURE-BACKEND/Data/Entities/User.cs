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
        public sbyte ShowPhone { get; set; }
        public DateTime? Birthday { get; set; }
        public sbyte ShowBirthday { get; set; }
        public DateTime RegisterDate { get; set; }
        public string Email { get; set; }
        public sbyte ShowEmail { get; set; }
        public sbyte Admin { get; set; }
        public sbyte Membership { get; set; }
        public sbyte Ban { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Participant> Participants { get; set; }
        public virtual ICollection<Reward> Rewards { get; set; }
        public virtual ICollection<TeamMember> TeamMembers { get; set; }
    }
}
