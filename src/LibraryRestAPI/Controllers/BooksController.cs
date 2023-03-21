using LibraryRestAPI.DTOs;
using LibraryRestAPI.Services;
using Microsoft.AspNetCore.Mvc;
using LibraryRestAPI.Entities;
namespace LibraryRestAPI.Controllers;

[Route("api/[controller]/")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly BookService _service;
    public BooksController(BookService service)
    {
        _service = service;
    }

    [HttpPost]
    public BookDto? Create(BookCreateDto createDto)
    {
        var bookId = _service.Create(createDto);
        return _service.GetById(bookId);
    }

    [HttpGet("{id}")]
    public APIResponse<BookDto> GetById(Guid id)
    {
        var response = new APIResponse<BookDto>();
        var book = _service.GetById(id);
        response.Data = book;
        return response;
    }

    [HttpGet("list")]
    public PaginatedListAPIResponse<BookDto> GetList([FromQuery]int pageSize, int pageNumber, string? search, 
        string? orderBy, string? orderDirection)
    {
        return _service.GetList(pageSize,pageNumber,search, orderBy, orderDirection);
    }

    [HttpDelete("{id}")]
    public void Delete(Guid id)
    {
        _service.Delete(id);
    }

    [HttpPut("{id}")]
    public BookDto? Update(Guid id, BookUpdateDto updateDto)
    {
        return _service.Update(id, updateDto);
    }
}