namespace Isitar.TimeTracking.Application.User.Queries.UserLocale
{
    using AutoMapper;
    using Common.Mappings;
    using Domain.Entities;

    public class UserLocaleVm : IMapFrom<User>
    {
        public string Locale { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserLocaleVm>()
                .ForMember(u => u.Locale,
                    opts => opts.MapFrom(u => u.Locale));
        }
    }
}