namespace DomainData.Models.Interfaces
{
    public interface ISoftDeletableTimeStamp
    {
        DateTime? DeletedAt { get; set; }
    }
}
