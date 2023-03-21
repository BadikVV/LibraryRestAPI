using LibraryRestAPI.DTOs;
using LibraryRestAPI.Entities;
using System.Linq.Expressions;

namespace LibraryRestAPI.Interfaces;

public interface IBookInSubscriptionRepository
{
    public Guid Add(BookInSubscription bookInSubscription);
    public void Update(BookInSubscription bookInSubscription);
    public void Delete(Guid bookId);
    public  List<BookInSubscription> GetList(int pageSize, int pageNumber, string? orderDirection = "asc", 
        Expression<Func<BookInSubscription, object>>? orderExpression = null,
        Expression<Func<BookInSubscription, bool>>? expression = null);
    public BookInSubscription? GetById(Guid id);
    public int Count(Expression<Func<BookInSubscription, bool>>? expression = null);
}