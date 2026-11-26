

namespace Traningsbok.Models
{
    public class GoalNotification
    {
        public int Id { get; set; }
        public int WeeklyGoalId { get; set; }
        public int Year { get; set; }
        public int Week { get; set; }
        public int ThresholdPercentage { get; set; }

    }
}
