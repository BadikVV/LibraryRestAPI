using System.Linq.Expressions;
using LibraryRestAPI.Entities;
using LibraryRestAPI.Infrastructure.Contexts;
using LibraryRestAPI.Interfaces;

namespace LibraryRestAPI.Infrastructure.Repositories;

public class BookRepository : IBookRepository
{
    private readonly DataDbContext _dbcontext;
    public BookRepository(DataDbContext context)
    {
        _dbcontext = context;
    }

    public Guid AddBook(Book book)
    {
        _dbcontext.Books.Add(book);
        _dbcontext.SaveChanges();
        return book.Id;
    }

    public Book? GetById(Guid id)
    {
       return _dbcontext.Books.FirstOrDefault(b => b.Id == id);
    }

    public List<Book> GetList(int pageSize, int pageNumber, string? orderDirection = "asc", 
        Expression<Func<Book, object>>? orderExpression = null,
        Expression<Func<Book, bool>> ? expression = null)
    {
        IQueryable<Book>? query = null;
        
        // Sorting
        if (orderExpression == null)
        {
            query = _dbcontext.Books.OrderBy(b => b.Title);
        }
        else
        {
            if (orderDirection != "asc" && orderDirection != "desc")
                orderDirection = "asc";
            if (orderDirection == "asc")
                query = _dbcontext.Books.OrderBy(orderExpression);
            else
                query = _dbcontext.Books.OrderByDescending(orderExpression);
        }

        //expression
        if (expression != null)
            query = query.Where(expression);
        
        return query.Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
    }

    public void Delete(Guid id)
    {
        var book = GetById(id);
        if (book != null)
        {
            _dbcontext.Books.Remove(book);
            _dbcontext.SaveChanges();
        }
    }

    public void Update(Book book)
    {
        _dbcontext.SaveChanges();
    }

    public int Count(Expression<Func<Book, bool>>? expression = null)
    {
        if (expression != null)
            return _dbcontext.Books.Where(expression).Count();
        return _dbcontext.Books.Count();
    }
}