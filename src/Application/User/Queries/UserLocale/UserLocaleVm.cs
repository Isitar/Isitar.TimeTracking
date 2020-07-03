namespace Isitar.TimeTracking.Application.User.Queries.UserLocale
{
    using Common.Mappings;
    using Domain.Entities;

    public class UserLocaleVm : IMapFrom<User>
    {
        public string Locale { get; set; }
    }
}