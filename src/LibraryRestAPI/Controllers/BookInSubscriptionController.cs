using LibraryRestAPI.DTOs;
using LibraryRestAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryRestAPI.Controllers;

[Route("api/[controller]/")]
[ApiController]
public class BookInSubscriptionController : ControllerBase
{
    private readonly BookInSubscriptionService _service;
    public BookInSubscriptionController(BookInSubscriptionService service)
    {
        _service = service;
    }

    [HttpPost]
    public BookInSubscriptionDto? Create(BookInSubscriptionCreateDto createDto)
    {
        var bookInSubscriptionId = _service.Create(createDto);
        return _service.GetById(bookInSubscriptionId);
    }

    [HttpGet("{id}")]
    public BookInSubscriptionDto? GetById(Guid id)
    {
        return _service.GetById(id);
    }

    [HttpGet("list")]
    public PaginatedListAPIResponse<BookInSubscriptionDto> GetList([FromQuery]int pageSize, int pageNumber, string? search, 
        string? orderBy, string? orderDirection)
    {
        return _service.GetList(pageSize,pageNumber, search, orderBy, orderDirection);
    }

    [HttpDelete("{id}")]
    public void Delete(Guid id)
    {
        _service.Delete(id);
    }
    
    [HttpPut("{id}")]
    public BookInSubscriptionDto? Update(Guid id, BookInSubscriptionUpdateDto updateDto)
    {
        return _service.Update(id, updateDto);
    }
}