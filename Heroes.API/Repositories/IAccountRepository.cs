using Heroes.API.Models;

namespace Heroes.API.Repositories
{
    public interface IAccountRepository
    {
        Task<string> LoginAsync(SignInModel signInModel);
        Task<string> SignUpAsync(SignUpModel signUpModel);
    }
}