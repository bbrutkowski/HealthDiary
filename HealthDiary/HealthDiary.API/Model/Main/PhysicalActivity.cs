namespace HealthDiary.API.Model.Main
{
    public class PhysicalActivity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal MET {  get; set; }
        public decimal AverageSpeed { get; set; }
    }
}
