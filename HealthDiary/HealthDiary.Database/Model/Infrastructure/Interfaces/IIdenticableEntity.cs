namespace HealthDiary.Database.Model.Infrastructure.Interfaces
{
    public interface IIdenticableEntity<TKey>
    {
        TKey Id { get; set; }
        bool IsNew();
    }
}
