using Heroes.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Heroes.API.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<TrainerModel> _userManager;
        private readonly SignInManager<TrainerModel> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountRepository(UserManager<TrainerModel> userManager,
            SignInManager<TrainerModel> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<string> SignUpAsync(SignUpModel signUpModel)
        {
            TrainerModel trainer = new()
            {
                Name = signUpModel.Name,
                Email = signUpModel.Email,
                UserName = signUpModel.Email
            };
            var result = await _userManager.CreateAsync(trainer, signUpModel.Password);
            if (!result.Succeeded)
            {
                return null;
            }
            return trainer.Id;
        }

        public async Task<string> LoginAsync(SignInModel signInModel)
        {
            var result = await _signInManager.PasswordSignInAsync(signInModel.Email, signInModel.Password, false, false);
            if (!result.Succeeded)
            {
                return null;
            }
            var user = await _userManager.FindByEmailAsync(signInModel.Email);
            return CreateNewToken(user);
        }

        private string CreateNewToken(TrainerModel user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var authSigninKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                            issuer: _configuration["JWT:ValidIssuer"],
                            audience: _configuration["JWT:ValidAudience"],
                            expires: DateTime.Now.AddDays(1),
                            claims: authClaims,
                            signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256Signature)
                            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}