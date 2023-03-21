namespace LibraryRestAPI.DTOs;

public class Status
{
    public int Code { get; set; } = 0;
    public string Message { get; set; } = "";
}

public class EmptyAPIResponse
{
    public Status Status { get; set; } = new();
}

public class APIResponse<T> : EmptyAPIResponse
{
    public T? Data { get; set; }
}


public class Pagination
{
    public int Page { get; set; }
    public int Size { get; set; }
    public int Total { get; set; }
}

public class PaginatedListAPIResponse<DtoType> : APIResponse<List<DtoType>>
{
    public Pagination Pagination { get; set; } = new Pagination();
}