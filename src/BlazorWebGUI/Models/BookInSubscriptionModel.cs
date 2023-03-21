namespace BlazorWebGUI.Models;

public class BookInSubscriptionModel
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
    public bool Status { get; set; }
}