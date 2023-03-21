using LibraryRestAPI.DTOs;
using LibraryRestAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryRestAPI.Controllers;

[Route("api/[controller]/")]
[ApiController]
public class SubscriptionController : ControllerBase
{
    private readonly SubscriptionService _service;
    public SubscriptionController(SubscriptionService service)
    {
        _service = service;
    }

    [HttpPost]
    public SubscriptionDto? Create(SubscriptionCreateDto createDto)
    {
        var subscriptionId = _service.Create(createDto);
        return _service.GetById(subscriptionId);
    }

    [HttpGet("{id}")]
    public APIResponse<SubscriptionDto>? GetById(Guid id)
    {
        return new APIResponse<SubscriptionDto>
        { 
            Data = _service.GetById(id)
        };
    }

    [HttpGet("list")]
    public PaginatedListAPIResponse<SubscriptionDto> GetList([FromQuery]int pageSize, int pageNumber, string? search,
    string? orderBy, string? orderDirection)
    {
        return _service.GetList(pageSize, pageNumber,search, orderBy, orderDirection);
    }

    [HttpDelete("{id}")]
    public void Delete(Guid id)
    {
        _service.Delete(id);
    }
    
    [HttpPut("{id}")]
    public SubscriptionDto? Update(Guid id, SubscriptionUpdateDto updateDto)
    {
        return _service.Update(id, updateDto);
    }
}