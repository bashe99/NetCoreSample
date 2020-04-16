using System.Threading.Tasks;

namespace HelloServices.DataModels
{
    public interface IJWTUtility
    {
        string GetToken(User user);

        Task<bool> LoginAsync(User user);
    }
}
