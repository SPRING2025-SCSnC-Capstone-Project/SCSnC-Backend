namespace Domain.Common;

public class BaseEntity
{
    //create base entity class
    public static Guid Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
}