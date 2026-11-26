using System;
using System.Collections.Generic;
using System.Text;

namespace Traningsbok.Models
{
    internal class WeeklyGoal
    {
        public int Id { get; set; }
        public int AthleteId { get; set; }
        public int TrainingTypeId { get; set; }
        public int TargetMinutes { get; set; }
    }
}
