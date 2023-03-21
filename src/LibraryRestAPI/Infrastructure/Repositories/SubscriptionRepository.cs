using System.Linq.Expressions;
using LibraryRestAPI.Entities;
using LibraryRestAPI.Infrastructure.Contexts;
using LibraryRestAPI.Interfaces;

namespace LibraryRestAPI.Infrastructure.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly DataDbContext _dbcontext;

    public SubscriptionRepository(DataDbContext context)
    {
        _dbcontext = context;
    }

    public Guid Add(Subscription subscription)
    {
        _dbcontext.Subscriptions.Add(subscription);
        _dbcontext.SaveChanges();
        return subscription.Id;
    }

    public Subscription? GetById(Guid id)
    {
        return _dbcontext.Subscriptions.FirstOrDefault(s => s.Id == id);
    }

    public void Delete(Guid id)
    {
        var subscription = GetById(id);
        if (subscription != null)
        {
            _dbcontext.Subscriptions.Remove(subscription);
            _dbcontext.SaveChanges();
        }
    }

    public void Update(Subscription updateSubscription)
    {
        _dbcontext.SaveChanges();
    }

    public List<Subscription> GetList(int pageSize, int pageNumber, string orderDirection = "asc", 
    Expression<Func<Subscription, object>>? orderExpression = null,
        Expression<Func<Subscription, bool>> ? expression = null)
    {
        IQueryable<Subscription>? query;

        if (orderExpression == null)
            query = _dbcontext.Subscriptions.OrderBy(b => b.Name);
        else
        {
            if (orderDirection != "asc" && orderDirection != "desc")
                orderDirection = "asc";
            if (orderDirection == "asc")
                query = _dbcontext.Subscriptions.OrderBy(orderExpression);
            else
                query = _dbcontext.Subscriptions.OrderByDescending(orderExpression);
        }

        if (expression != null)
            query = query.Where(expression);

        return query.Skip((pageNumber - 1) * pageSize)
                     .Take(pageSize)
                     .ToList();
    }

    public int Count(Expression<Func<Subscription, bool>>? expression = null)
    {
        if (expression != null)
            return _dbcontext.Subscriptions.Where(expression).Count();
        return _dbcontext.Subscriptions.Count();
    }
}