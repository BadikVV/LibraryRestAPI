using System.Globalization;
using System.Linq.Expressions;
using LibraryRestAPI.DTOs;
using LibraryRestAPI.Entities;
using LibraryRestAPI.Interfaces;

namespace LibraryRestAPI.Services;

public class BookInSubscriptionService
{
    private readonly IBookInSubscriptionRepository _repository;
    private readonly IBookRepository _bookRepository;
    private readonly ISubscriptionRepository _subscriptionRepository;
    public BookInSubscriptionService(IBookInSubscriptionRepository repository, IBookRepository bookRepository,ISubscriptionRepository subscriptionRepository)
    {
        _repository = repository;
        _bookRepository = bookRepository;
        _subscriptionRepository = subscriptionRepository;
    }

    public Guid Create(BookInSubscriptionCreateDto createDto)
    {
        var bookInSubscription = new BookInSubscription()
        {
            BookId = createDto.BookId,
            SubscriptionId = createDto.SubscriptionId,
            Start = createDto.Start,
            End = createDto.End,
            Status = createDto.Status,
        };
        _repository.Add(bookInSubscription);
        return bookInSubscription.Id;
    }

    public BookInSubscriptionDto? Update(Guid id, BookInSubscriptionUpdateDto updateDto)
    {
        var bookInSubscription = _repository.GetById(id);
        if (bookInSubscription == null)
            return null;
        bookInSubscription.BookId = updateDto.BookId;
        bookInSubscription.SubscriptionId = updateDto.SubscriptionId;
        bookInSubscription.Start = updateDto.Start;
        bookInSubscription.End = updateDto.End;
        bookInSubscription.Status = updateDto.Status;
        _repository.Update(bookInSubscription);

        var bookInSubscriptionDto = new BookInSubscriptionDto()
        {
            BookId = bookInSubscription.BookId,
            SubscriptionId = bookInSubscription.SubscriptionId,
            Start = bookInSubscription.Start,
            End = bookInSubscription.End,
            Status = bookInSubscription.Status
        };
        return bookInSubscriptionDto;
    }

    public BookInSubscriptionDto? GetById(Guid id)
    {
        var bookInSubscription = _repository.GetById(id);
        if (bookInSubscription == null)
            return null;

        var bookInSubscriptionDto = new BookInSubscriptionDto()
        {
            BookId = bookInSubscription.BookId,
            SubscriptionId = bookInSubscription.SubscriptionId,
            Start = bookInSubscription.Start,
            End = bookInSubscription.End,
            Status = bookInSubscription.Status,
            Id = bookInSubscription.Id
        };
        return bookInSubscriptionDto;
    }

    public void Delete(Guid id)
    {
        _repository.Delete(id);
    }

    public PaginatedListAPIResponse<BookInSubscriptionDto> GetList(int pageSize, int pageNumber, string? search,
        string? orderBy = "SubscriptionId", string? orderDirection = "asc")
    {
        Expression<Func<BookInSubscription, object>>? orderExpression = null;
        if (!string.IsNullOrEmpty(orderBy))
            orderExpression = PredicateBuilder.ToLambda<BookInSubscription>(orderBy);

        var count = 0;
        List<BookInSubscription> listBooksInSubscriptions;
        PaginatedListAPIResponse<BookInSubscriptionDto> paginatedBooksInSubsriptions = new();
        List<Guid> bookListId = new();
        List<Guid> subscriptionsListId = new();
        if (search == null)
        {
            listBooksInSubscriptions = _repository.GetList(pageSize, pageNumber, orderDirection, orderExpression);
            count = _repository.Count();
        }
        else
        {
             bookListId = _bookRepository
                .GetList(_bookRepository.Count(), 1)
                .Where(book => 
                    book.Author.Contains(search) || book.Genre.Contains(search)|| 
                    book.Title.Contains(search) || book.Publisher.Contains(search))
                .Select(b => b.Id)
                .ToList();
             
            subscriptionsListId = _subscriptionRepository
                .GetList(_subscriptionRepository.Count(), 1)
                .Where(subscription => subscription.Name.Contains(search) || subscription.Surname.Contains(search) ||
                                       subscription.Patronymic.Contains(search) || subscription.PhoneNumber.Contains(search))
                .Select(s => s.Id)
                .ToList();

            listBooksInSubscriptions = _repository.GetList(pageSize, pageNumber, orderDirection,
                PredicateBuilder.ToLambda<BookInSubscription>(orderBy),
                bookInSubscription => 
                    bookListId.Contains(bookInSubscription.BookId) ||
                    subscriptionsListId.Contains(bookInSubscription.SubscriptionId));
            count = _repository.Count(bookInSubscription =>
                bookListId.Contains(bookInSubscription.BookId) ||
                subscriptionsListId.Contains(bookInSubscription.SubscriptionId));
        }
        
        var bookInSubscriptionsDtoList = listBooksInSubscriptions.Select(bookInSubscription => new BookInSubscriptionDto()
            {
                BookId = bookInSubscription.BookId,
                SubscriptionId = bookInSubscription.SubscriptionId,
                Start = bookInSubscription.Start,
                End = bookInSubscription.End,
                Status = bookInSubscription.Status,
                Id = bookInSubscription.Id
            })
            .ToList();
        paginatedBooksInSubsriptions.Data = bookInSubscriptionsDtoList;
        paginatedBooksInSubsriptions.Pagination.Page = pageNumber;
        paginatedBooksInSubsriptions.Pagination.Size = pageSize;
        paginatedBooksInSubsriptions.Pagination.Total = count;
        return paginatedBooksInSubsriptions;
    }
}