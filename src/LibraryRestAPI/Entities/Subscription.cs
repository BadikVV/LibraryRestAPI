namespace LibraryRestAPI.Entities;

public class Subscription
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Patronymic { get; set; }
    public string PhoneNumber { get; set; }
    public Guid Id { get; set; }
    
}