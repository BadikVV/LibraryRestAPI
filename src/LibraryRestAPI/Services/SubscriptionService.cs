using System.Linq.Expressions;
using LibraryRestAPI.DTOs;
using LibraryRestAPI.Entities;
using LibraryRestAPI.Interfaces;

namespace LibraryRestAPI.Services;

public class SubscriptionService
{
    private readonly ISubscriptionRepository _repository;

    public SubscriptionService(ISubscriptionRepository repository)
    {
        _repository = repository;
    }

    public Guid Create(SubscriptionCreateDto subscriptionCreateDto)
    {
        var subscription = new Subscription()
        {
            Name = subscriptionCreateDto.Name,
            Surname = subscriptionCreateDto.Surname,
            Patronymic = subscriptionCreateDto.Patronymic,
            PhoneNumber = subscriptionCreateDto.PhoneNumber
        };
        _repository.Add(subscription);
        return subscription.Id;
    }

    public SubscriptionDto? Update(Guid id, SubscriptionUpdateDto subscriptionUpdateDto)
    {
        var subscription = _repository.GetById(id);
        if (subscription == null)
            return null;
        subscription.Name = subscriptionUpdateDto.Name;
        subscription.Surname = subscriptionUpdateDto.Surname;
        subscription.Patronymic = subscriptionUpdateDto.Patronymic;
        subscription.PhoneNumber = subscriptionUpdateDto.PhoneNumber;
        _repository.Update(subscription);

        var subscriptionDto = new SubscriptionDto()
        {
            Name = subscription.Name,
            Surname = subscription.Surname,
            Patronymic = subscription.Patronymic,
            PhoneNumber = subscription.PhoneNumber,
            Id = subscription.Id
        };
        return subscriptionDto;
    }

    public SubscriptionDto? GetById(Guid id)
    {
        var subscription = _repository.GetById(id);
        if (subscription == null)
            return null;

        var subscriptionDto = new SubscriptionDto()
        {
            Name = subscription.Name,
            Surname = subscription.Surname,
            Patronymic = subscription.Patronymic,
            PhoneNumber = subscription.PhoneNumber,
            Id = subscription.Id
        };
        return subscriptionDto;
    }

    public void Delete(Guid id)
    {
        _repository.Delete(id);
    }

    public PaginatedListAPIResponse<SubscriptionDto> GetList(int pageSize, int pageNumber, string? search,
        string? orderBy ="Name", string? orderDirection ="asc")
    {
        Expression<Func<Subscription, object>>? orderExpression = null;
        if (!string.IsNullOrEmpty(orderBy))
            orderExpression = PredicateBuilder.ToLambda<Subscription>(orderBy);

        List<Subscription> subscriptionsList;
        var count = 0;
        PaginatedListAPIResponse<SubscriptionDto> paginatedSubscriptions = new();
        if (search == null)
        {
            subscriptionsList = _repository.GetList(pageSize, pageNumber, orderDirection, orderExpression);
            count = _repository.Count();
        }
        else
        {
            subscriptionsList = _repository.GetList(pageSize, pageNumber, orderDirection,
                PredicateBuilder.ToLambda<Subscription>(orderBy),
                subscription => subscription.Name.Contains(search) || subscription.Surname.Contains(search) ||
                                subscription.Patronymic.Contains(search) || subscription.PhoneNumber.Contains(search));
            count = _repository.Count(subscription =>
                subscription.Name.Contains(search) || subscription.Surname.Contains(search) ||
                subscription.Patronymic.Contains(search) || subscription.PhoneNumber.Contains(search));
        }

        var subscriptionsDtoList = subscriptionsList.Select(subscription => new SubscriptionDto()
            {
                Name = subscription.Name,
                Surname = subscription.Surname,
                Patronymic = subscription.Patronymic,
                PhoneNumber = subscription.PhoneNumber,
                Id = subscription.Id
            })
            .ToList();
        paginatedSubscriptions.Data = subscriptionsDtoList;
        paginatedSubscriptions.Pagination.Page = pageNumber;
        paginatedSubscriptions.Pagination.Size = pageSize;
        paginatedSubscriptions.Pagination.Total = count;
        return paginatedSubscriptions;
    }
}