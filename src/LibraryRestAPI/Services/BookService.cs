using System.Linq.Expressions;
using LibraryRestAPI.DTOs;
using LibraryRestAPI.Entities;
using LibraryRestAPI.Interfaces;

namespace LibraryRestAPI.Services;

public class BookService
{
    private readonly IBookRepository _repository;
    public BookService(IBookRepository repository)
    {
        _repository = repository;
    }
    
    public BookDto? GetById(Guid id)
    {
        var book = _repository.GetById(id);
        if (book == null) return null;
        
        var bookDto = new BookDto()
        {
            Author = book.Author,
            Title = book.Title,
            Genre = book.Genre,
            Publisher = book.Publisher,
            ReleaseYear = book.ReleaseYear,
            PagesNumber = book.PagesNumber,
            Id = book.Id
        };
        return bookDto;
    }

    public BookDto? Update(Guid id, BookUpdateDto bookUpdateDto)
    {
        var book = _repository.GetById(id);
        if (book == null) return null;

        // Update
        book.Author = bookUpdateDto.Author;
        book.Genre = bookUpdateDto.Genre;
        book.Publisher = bookUpdateDto.Publisher;
        book.Title = bookUpdateDto.Title;
        book.PagesNumber = bookUpdateDto.PagesNumber;
        book.ReleaseYear = bookUpdateDto.ReleaseYear;
        _repository.Update(book);
        
        //Create DTO to return
        var bookDto = new BookDto()
        {
            Author = book.Author,
            Genre = book.Genre,
            Publisher = book.Publisher,
            Title = book.Title,
            PagesNumber = book.PagesNumber,
            ReleaseYear = book.ReleaseYear,
            Id = book.Id
        };
        return bookDto;
    }

    public PaginatedListAPIResponse<BookDto> GetList(int pageSize, int pageNumber, string? search,
        string? orderBy = "Title", string? orderDirection = "asc")
    {
        Expression<Func<Book, object>>? orderExpression = null;
        if (!string.IsNullOrEmpty(orderBy))
        {
            orderExpression = PredicateBuilder.ToLambda<Book>(orderBy);
        }
        List<Book> booksList;
        var count = 0;
        PaginatedListAPIResponse<BookDto> paginatedBooks = new();
        if (search == null)
        {
            booksList = _repository.GetList(pageSize, pageNumber, orderDirection, orderExpression);
            count = _repository.Count();
        } 
        else
        {
            booksList = _repository.GetList(pageSize, pageNumber, orderDirection, PredicateBuilder.ToLambda<Book>(orderBy),
                book => 
                book.Author.Contains(search) || book.Genre.Contains(search)|| 
                book.Title.Contains(search) || book.Publisher.Contains(search));
            count = _repository.Count(book => 
                book.Author.Contains(search) || book.Genre.Contains(search)|| 
                book.Title.Contains(search) || book.Publisher.Contains(search));
        }

        var booksDtoList = booksList.Select(book => new BookDto()
            {
                Author = book.Author,
                Genre = book.Genre,
                Publisher = book.Publisher,
                Title = book.Title,
                PagesNumber = book.PagesNumber,
                ReleaseYear = book.ReleaseYear,
                Id = book.Id
            })
            .ToList();
        paginatedBooks.Data = booksDtoList;
        paginatedBooks.Pagination.Page = pageNumber;
        paginatedBooks.Pagination.Size = pageSize;
        paginatedBooks.Pagination.Total = count;
        return paginatedBooks;
    }

    public void Delete(Guid id)
    {
        _repository.Delete(id);
    }

    public Guid Create(BookCreateDto bookCreateDto)
    {
        var book = new Book()
        {
            Author = bookCreateDto.Author,
            Title = bookCreateDto.Title,
            Genre = bookCreateDto.Genre,
            Publisher = bookCreateDto.Publisher,
            ReleaseYear = bookCreateDto.ReleaseYear,
            PagesNumber = bookCreateDto.PagesNumber
        };
        _repository.AddBook(book);
        return book.Id;
    }
}