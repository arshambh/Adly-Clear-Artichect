using Adly.Application.Common.MappingConfigurations;
using Adly.Domain.Entities.Ad;
using AutoMapper;

namespace Adly.Application.Feature.Ad.Queries;

public record GetAdDetailByIdQueryResult(
    Guid AdId,
    string Title,
    string Description,
    AdEntity.AdState CurrentState,
    string LocationName,
    Guid LocationId,
    string CategoryName,
    Guid CategoryId,
    Guid OwnerId,
    string OwnerUserName,
    string OwnerPhoneNumber):ICreateApplicationMapper<AdEntity>
{
    public record AdDetailImageModel(string ImageName, string ImageUrl);
    public AdDetailImageModel[] AdImages { get; set; }


    public void Map(Profile profile)
    {
        profile.CreateMap<AdEntity, GetAdDetailByIdQueryResult>()
            .ForCtorParam(nameof(AdId),opt=>opt.MapFrom(x=>x.Id))
            .ForCtorParam(nameof(Title),opt=>opt.MapFrom(x=>x.Title))
            .ForCtorParam(nameof(Description),opt=>opt.MapFrom(x=>x.Description))
            .ForCtorParam(nameof(CurrentState),opt=>opt.MapFrom(x=>x.CurrentState))
            .ForCtorParam(nameof(LocationName),opt=>opt.MapFrom(x=>x.Location.Name))
            .ForCtorParam(nameof(LocationName),opt=>opt.MapFrom(x=>x.LocationId))
            .ForCtorParam(nameof(CategoryName),opt=>opt.MapFrom(x=>x.Category.Name))
            .ForCtorParam(nameof(CategoryId),opt=>opt.MapFrom(x=>x.CategoryId))
            .ForCtorParam(nameof(OwnerId),opt=>opt.MapFrom(x=>x.UserId))
            .ForCtorParam(nameof(OwnerUserName),opt=>opt.MapFrom(x=>x.User.UserName))
            .ForCtorParam(nameof(OwnerPhoneNumber),opt=>opt.MapFrom(x=>x.User.PhoneNumber))
            .ReverseMap();



    }










}