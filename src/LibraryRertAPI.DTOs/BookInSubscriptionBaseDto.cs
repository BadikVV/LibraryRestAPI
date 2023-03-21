namespace LibraryRestAPI.DTOs;

public class BookInSubscriptionBaseDto
{
    public Guid BookId { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
    public Guid SubscriptionId { get; set; }
    public bool Status { get; set; }
}