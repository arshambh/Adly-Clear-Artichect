using AutoMapper;

namespace Adly.Application.Common.MappingConfigurations;

/// <summary>
/// این زمانی استفاده می شود که اگر کلاسی همه پروپرتی هاش نظیر به نظیر بود فقط از این کلاس ارث بری کنه
/// </summary>
/// <typeparam name="TSource"></typeparam>

public interface ICreateApplicationMapper<TSource>
{
    void Map(Profile profile)
    {
        profile.CreateMap(typeof(TSource), GetType()).ReverseMap();
    }
}