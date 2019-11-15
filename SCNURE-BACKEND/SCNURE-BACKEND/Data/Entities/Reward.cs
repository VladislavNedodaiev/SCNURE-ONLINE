using System;
using System.Collections.Generic;

namespace SCNURE_BACKEND.Data.Entities
{
    public partial class Reward
    {
        public int RewardId { get; set; }
        public int AchievementId { get; set; }
        public int UserId { get; set; }
        public DateTime PostDate { get; set; }

        public virtual Achievement Achievement { get; set; }
        public virtual User User { get; set; }
    }
}
