using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SCNURE_BACKEND.Data.Dtos;
using SCNURE_BACKEND.Data.Entities;
using SCNURE_BACKEND.Data.Repositories.Users;
using SCNURE_BACKEND.Helpers;

namespace SCNURE_BACKEND.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUsersRepository _usersRepository;
		private readonly JwtSettings _jwtSettings;

		public UserService(IUsersRepository usersRepository, IOptions<JwtSettings> jwtSettings)
        {
            _usersRepository = usersRepository;
			_jwtSettings = jwtSettings.Value;
		}

        public async Task<User> AuthenticateAsync(string loginOrEmail, string password)
        {
            if (string.IsNullOrEmpty(loginOrEmail) || string.IsNullOrEmpty(password))
                return null;

            var user = await _usersRepository.GetByLoginOrEmailAsync(loginOrEmail);

            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _usersRepository.GetByIdAsync(id);
        }

		public async Task<User> RegisterAsync(RegisterDto userDto)
		{
			var user = await CreateUserFromDtoAsync(userDto);
			if (user != null)
			{
				var tokenHandler = new JwtSecurityTokenHandler();
				var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
				var tokenDescriptor = new SecurityTokenDescriptor
				{
					Subject = new ClaimsIdentity(new Claim[]
					{
							new Claim(ClaimTypes.Name, user.UserId.ToString())
					}),
					Expires = DateTime.UtcNow.AddDays(2),
					SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
				};
				var token = tokenHandler.CreateToken(tokenDescriptor);
				var emailTokenString = tokenHandler.WriteToken(token);

				user.Verification = emailTokenString;
				await _usersRepository.UpdateAsync(user);
				return user;
			}
			return user;
		}

		public async Task ConfirmUserEmailAsync(string token)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
			var validations = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(key),
				ValidateIssuer = false,
				ValidateAudience = false
			};
			var claims = tokenHandler.ValidateToken(token, validations, out var securityToken);
			var user = await _usersRepository.GetByIdAsync(Convert.ToInt32(claims.Identity.Name));
			if (user != null)
			{
				user.Verification = null;
				await _usersRepository.UpdateAsync(user);
			}
			else
			{
				throw new ArgumentException("UserId wasn't found");
			}
		}

		private async Task<User> CreateUserFromDtoAsync(RegisterDto userDto)
		{
			if (await _usersRepository.IsLoginTaken(userDto.Login))
				throw new ArgumentException("Username \"" + userDto.Login + "\" is already taken");

			if (userDto.Password != userDto.PasswordConfirmation)
				throw new ArgumentException("Password and passwordConfirmation don't match");

			byte[] passwordHash, passwordSalt;
			CreatePasswordHash(userDto.Password, out passwordHash, out passwordSalt);

			User user = new User()
			{
				Login = userDto.Login,
				PasswordHash = passwordHash,
				PasswordSalt = passwordSalt,
				Email = userDto.Email
			};

			await _usersRepository.AddAsync(user);

			return user;
		}

		private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

    }
}
