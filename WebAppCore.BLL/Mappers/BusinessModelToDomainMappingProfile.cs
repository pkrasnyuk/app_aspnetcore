using AutoMapper;
using MongoDB.Bson;
using WebAppCore.BLL.Models;
using WebAppCore.Domain.Entities;

namespace WebAppCore.BLL.Mappers
{
    public class BusinessModelToDomainMappingProfile : Profile
    {
        public BusinessModelToDomainMappingProfile()
        {
            CreateMap<string, ObjectId>().ConvertUsing(ObjectId.Parse);
            CreateMap<RegisterUserModel, User<ObjectId>>();
            CreateMap<UserViewModel, User<ObjectId>>();
            CreateMap<UpdateUserModel, User<ObjectId>>();
            CreateMap<PhotoViewModel, Photo<ObjectId>>()
                .ForMember(obj => obj.CreatedAt, map => map.MapFrom(model => model.DateUploaded));
            CreateMap<PhotoModel, Photo<ObjectId>>();
            CreateMap<AlbumModel, Album<ObjectId>>();
            CreateMap<AlbumViewModel, Album<ObjectId>>();
        }
    }
}