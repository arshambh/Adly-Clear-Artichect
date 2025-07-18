using Adly.Domain.Common;
using Adly.Domain.Common.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Ardalis.GuardClauses;

namespace Adly.Domain.Entities.Ad;

public class AdEntity : BaseEntity<Guid>
{
    private readonly List<ImageValueObjects> _images = new();
    private readonly List<LogValueObject> _changeLog = new();

    public string? Title { get; private set; }
    public string? Description { get; private set; }

    public Guid UserId { get; private set; }

    public IReadOnlyList<ImageValueObjects> Images => _images.AsReadOnly();
    public IReadOnlyList<LogValueObject> ChangeLogs => _changeLog.AsReadOnly();

    public Guid CategoryId { get; private set; }
    public Guid LocationId { get; private set; }

    public AdState CurrentState { get; private set; }

    public enum AdState
    {
        Pending,
        Rejected,
        Approved,
        Deleted,
        Expired,
    }


    public DomainResult ChangeState(AdState state, string? additionalMessage = null)
    {
        if (CurrentState == AdState.Approved && state is AdState.Rejected or AdState.Pending)
            return new DomainResult(false, "This ad is already approved!");

        CurrentState = state;
        _changeLog.Add(new LogValueObject(DateTime.Now, "Ad State Changed", additionalMessage));
        return DomainResult.None;

    }



    private AdEntity()
    {

    }




    public static AdEntity Create(string? title, string? description, Guid? userId, Guid? categoryId, Guid? locationId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);

        // if (userId == Guid.Empty || userId == null)
        //     throw new InvalidOperationException("User Id Must Have A Value...");
        //
        // if (categoryId == Guid.Empty || categoryId == null)
        //     throw new InvalidOperationException("Category Id Must Have A Value...");

        Guard.Against.NullOrEmpty(userId, "Invalid User ID");
        Guard.Against.NullOrEmpty(categoryId, "Invalid Category ID");
        Guard.Against.NullOrEmpty(locationId, "Invalid Location ID");


        var ad = new AdEntity
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = description,
            UserId = userId.Value,
            CategoryId = categoryId.Value,
            CurrentState = AdState.Pending,
            LocationId = locationId.Value
        };


        ad._changeLog.Add(new LogValueObject(DateTime.Now, "Ad Created"));

        return ad;
    }


    public static AdEntity Create(Guid? id, string? title, string? description, Guid? userId, Guid? categoryId, Guid? locationId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);

        // if (userId == Guid.Empty || userId == null)
        //     throw new InvalidOperationException("User Id Must Have A Value...");
        //
        // if (categoryId == Guid.Empty || categoryId == null)
        //     throw new InvalidOperationException("Category Id Must Have A Value...");
        //
        // if (id == Guid.Empty || id == null)
        //     throw new InvalidOperationException("Id Must Have A Value...");


        Guard.Against.NullOrEmpty(id, "Invalid ID");
        Guard.Against.NullOrEmpty(userId, "Invalid User ID");
        Guard.Against.NullOrEmpty(categoryId, "Invalid Category ID");
        Guard.Against.NullOrEmpty(locationId, "Invalid Location ID");

        var ad = new AdEntity
        {
            Id = id.Value,
            Title = title,
            Description = description,
            UserId = userId.Value,
            CategoryId = categoryId.Value,
            CurrentState = AdState.Pending,
            LocationId = locationId.Value
        };
        ad._changeLog.Add(new LogValueObject(DateTime.Now, "Ad Created"));
        return ad;
    }


    public void Edit(string title, string description, Guid? categoryId, Guid? locationId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);

        Guard.Against.NullOrEmpty(categoryId, "Invalid Category ID");
        Guard.Against.NullOrEmpty(locationId, "Invalid Location ID");


        Title = title;
        Description = description;
        CategoryId = categoryId.Value;
        LocationId = locationId.Value;

        _changeLog.Add(new LogValueObject(DateTime.Now, "Ad Edited"));
        CurrentState = AdState.Pending;
    }
}