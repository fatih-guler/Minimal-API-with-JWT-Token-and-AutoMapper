namespace MinimalAPI.Services
{
    public interface ITokenService
    {
        string GetToken(User user);
    }
}
