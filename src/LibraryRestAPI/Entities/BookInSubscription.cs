namespace LibraryRestAPI.Entities;

public class BookInSubscription
{
    public Guid BookId { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
    public Guid SubscriptionId { get; set; }
    public bool Status { get; set; }
    public Guid Id { get; set; }
}