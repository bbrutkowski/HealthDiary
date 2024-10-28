namespace HealthDiary.API.Model.Main
{
    public class Avatar
    {
        public int Id { get; set; }
        public byte[]? Picture { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
