namespace HealthDiary.API.Model.Main
{
    public class BMI
    {
        public int Id { get; set; }
        public decimal MinValue { get; set; }
        public decimal MaxValue { get; set; }
        public string? Description { get; set; }
        public string? IndexColor { get; set; }
    }
}
