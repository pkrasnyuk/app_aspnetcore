using AutoMapper;

namespace WebAppCore.BLL.Mappers
{
    public class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainToBusinessModelMappingProfile());
                cfg.AddProfile(new BusinessModelToDomainMappingProfile());
            });
        }
    }
}