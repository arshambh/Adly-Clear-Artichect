using Adly.Domain.Common;
using Ardalis.GuardClauses;

namespace Adly.Domain.Entities.Ad;

public sealed class LocationEntity : BaseEntity<Guid>
{
    public string Name { get; private set; }

    private List<AdEntity> _ads = new List<AdEntity>();

    public IReadOnlyList<AdEntity> Ads => _ads.AsReadOnly();

    public LocationEntity(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }

    private LocationEntity()
    {

    }

    public void EditLocationName(string newLocationName)
    {
        Guard.Against.NullOrEmpty(newLocationName);
        this.Name = newLocationName;
    }

}