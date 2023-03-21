using System.Linq.Expressions;
using LibraryRestAPI.Entities;

namespace LibraryRestAPI.Interfaces;

public interface IBookRepository
{
    public Guid AddBook(Book book);
    public Book? GetById(Guid id);
    public List<Book>GetList(int pageSize, int pageNumber, string? orderDirection = "asc", 
        Expression<Func<Book, object>>? orderExpression = null,
        Expression<Func<Book, bool>> ? expression = null);
    public int Count(Expression<Func<Book, bool>>? expression = null);
    public void Delete(Guid id);
    public void Update(Book book);
}