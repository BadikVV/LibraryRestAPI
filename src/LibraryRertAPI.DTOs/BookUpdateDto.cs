namespace LibraryRestAPI.DTOs;

public class BookUpdateDto
{
    public string Author { get; set; }
    public string Title { get; set; }
    public string Genre { get; set; }
    public string Publisher { get; set; }
    public DateTime? ReleaseYear { get; set; }
    public uint PagesNumber { get; set; }
}

