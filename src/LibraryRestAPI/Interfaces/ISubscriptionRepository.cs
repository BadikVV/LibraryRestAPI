using LibraryRestAPI.Entities;
using System.Linq.Expressions;

namespace LibraryRestAPI.Interfaces;

public interface ISubscriptionRepository
{
    public Guid Add(Subscription subscription);
    public Subscription? GetById(Guid id);
    public void Delete(Guid id);
    public void Update(Subscription subscription);
    public List<Subscription> GetList(int pageSize, int pageNumber, string? orderDirection = "asc", 
    Expression<Func<Subscription, object>>? orderExpression = null,
        Expression<Func<Subscription, bool>> ? expression = null);
    public int Count(Expression<Func<Subscription, bool>>? expression = null);
}