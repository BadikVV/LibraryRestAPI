using LibraryRestAPI.Entities;
using LibraryRestAPI.Infrastructure.Contexts;
using LibraryRestAPI.Interfaces;
using System.Linq.Expressions;
using LibraryRestAPI.DTOs;

namespace LibraryRestAPI.Infrastructure.Repositories;

public class BookInSubscriptionRepository : IBookInSubscriptionRepository
{
    private readonly DataDbContext _dbcontext;
    public BookInSubscriptionRepository(DataDbContext context)
    {
        _dbcontext = context;
    }
    
    public Guid Add(BookInSubscription bookInSubscription)
    {
        _dbcontext.BookInSubscriptions.Add(bookInSubscription);
        _dbcontext.SaveChanges();
        return bookInSubscription.Id;
    }

    public void Update(BookInSubscription bookInSubscription)
    {
        _dbcontext.SaveChanges();
    }

    public void Delete(Guid bookId)
    {
        var bookInSubscription = GetById(bookId);
        if (bookInSubscription != null)
        {
            _dbcontext.BookInSubscriptions.Remove(bookInSubscription);
            _dbcontext.SaveChanges();
        }
    }

    public List<BookInSubscription> GetList(int pageSize, int pageNumber, string? orderDirection = "asc", 
        Expression<Func<BookInSubscription, object>>? orderExpression = null,
        Expression<Func<BookInSubscription, bool>>? expression = null)
    {
        IQueryable<BookInSubscription> query;
        // IQueryable<Book> queryBook = _dbcontext.Books;
        // IQueryable<Subscription> querySubscriptions = _dbcontext.Subscriptions;

        
        if (orderExpression == null)
            query = _dbcontext.BookInSubscriptions.OrderBy(b => b.SubscriptionId);
        else
        {
            if (orderDirection != "asc" && orderDirection != "desc")
                orderDirection = "asc";
            if (orderDirection == "asc")
                query = _dbcontext.BookInSubscriptions.OrderBy(orderExpression);
            else
                query = _dbcontext.BookInSubscriptions.OrderByDescending(orderExpression);
        }

        if (expression != null)
        {
            // foreach (var book in queryBook)
            // {
            //     if (book.Author.Contains(expression.ToString()) || book.Title.Contains(expression.ToString())
            //                                                     || book.Genre.Contains(expression.ToString()))
            //         query = query.Where(b => b.BookId == book.Id);
            // }
            //
            // foreach (var subscription in querySubscriptions)
            // {
            //     if (subscription.Name.Contains(expression.ToString()) || subscription.Surname.Contains(expression.ToString())
            //                                                             || subscription.Patronymic.Contains(expression.ToString()))
            //         query = query.Where(b => b.SubscriptionId == subscription.Id);
            // }
            query = query.Where(expression);
        }
                
        return query.Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }
    
    public BookInSubscription? GetById(Guid id)
    {
        return _dbcontext.BookInSubscriptions.FirstOrDefault(b => b.Id == id);
    }

    public int Count(Expression<Func<BookInSubscription, bool>>? expression = null)
    {
        if (expression != null)
            return _dbcontext.BookInSubscriptions.Where(expression).Count();
        return _dbcontext.BookInSubscriptions.Count();
    }
}