namespace HealthDiary.API.Model.Main
{
    public class WeightGoal
    {
        public int Id { get; set; }
        public bool IsSet { get; set; }
        public decimal CurrentWeight { get; set; }
        public decimal TargetWeight { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime TargetDate { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
