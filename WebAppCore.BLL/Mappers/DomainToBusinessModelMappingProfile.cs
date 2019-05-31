using AutoMapper;
using MongoDB.Bson;
using WebAppCore.BLL.Models;
using WebAppCore.Domain.Entities;

namespace WebAppCore.BLL.Mappers
{
    public class DomainToBusinessModelMappingProfile : Profile
    {
        public DomainToBusinessModelMappingProfile()
        {
            CreateMap<ObjectId, string>().ConvertUsing(o => o.ToString());
            CreateMap<User<ObjectId>, UserModel>();
            CreateMap<User<ObjectId>, UpdateUserModel>();
            CreateMap<User<ObjectId>, RegisterUserModel>();
            CreateMap<User<ObjectId>, UserViewModel>();
            CreateMap<Photo<ObjectId>, PhotoViewModel>()
                .ForMember(model => model.DateUploaded, map => map.MapFrom(obj => obj.CreatedAt));
            CreateMap<Album<ObjectId>, AlbumViewModel>();
        }
    }
}