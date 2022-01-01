namespace MinimalAPI.Services
{
    public class MyAutoMapper: Profile
    {
        public MyAutoMapper(IDataProtector provider)
        {
            CreateMap<User, UserViewModel>()
                .ForMember(u => u.FullName, opt => opt.MapFrom(u2 => u2.Name + " " + u2.LastName))
                .ForMember(u => u.PasswordHash, opt => opt.MapFrom(u2 =>provider.Unprotect(u2.PasswordHash)));

            CreateMap<UserViewModel, User>()
                .ForMember(u => u.PasswordHash, opt => opt.MapFrom(u2 => provider.Protect(u2.Password)));

        }
    }
}
