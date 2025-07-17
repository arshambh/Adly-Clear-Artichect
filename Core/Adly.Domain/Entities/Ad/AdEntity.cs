using Adly.Domain.Common;
using Adly.Domain.Common.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Adly.Domain.Entities.Ad;

public class AdEntity: BaseEntity<Guid>
{
    private readonly List<ImageValueObjects> _images = new();

    public string? Name { get; private set; }
    public string? Description { get; private set; }

    public Guid UserId { get; private set; }

    public IReadOnlyList<ImageValueObjects> Images => _images.AsReadOnly();

    public Guid CategoryId { get; private set; }

    private AdEntity()
    {
        
    }

    public static AdEntity Create(string? name, string? description, Guid? userId, Guid? categoryId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);

        if (userId == Guid.Empty || userId == null)
            throw new InvalidOperationException("User Id Must Have A Value...");

        if (categoryId == Guid.Empty || categoryId == null)
            throw new InvalidOperationException("Category Id Must Have A Value...");




        return new AdEntity
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            UserId = userId.Value,
            CategoryId = categoryId.Value
        };
    }


    public static AdEntity Create(Guid? id,string? name, string? description, Guid? userId, Guid? categoryId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);

        if (userId == Guid.Empty || userId == null)
            throw new InvalidOperationException("User Id Must Have A Value...");

        if (categoryId == Guid.Empty || categoryId == null)
            throw new InvalidOperationException("Category Id Must Have A Value...");

        if (id == Guid.Empty || id == null)
            throw new InvalidOperationException("Id Must Have A Value...");


        return new AdEntity
        {
            Id = id.Value,
            Name = name,
            Description = description,
            UserId = userId.Value,
            CategoryId = categoryId.Value
        };
    }




}