using System;
using System.Collections.Generic;

namespace SCNURE_BACKEND.Data.Entities
{
    public partial class Achievement
    {
        public Achievement()
        {
            Rewards = new HashSet<Reward>();
        }

        public int AchievementId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Photo { get; set; }

        public virtual ICollection<Reward> Rewards { get; set; }
    }
}
