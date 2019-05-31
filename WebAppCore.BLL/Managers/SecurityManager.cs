using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebAppCore.BLL.Helpers;
using WebAppCore.BLL.Models;

namespace WebAppCore.BLL.Managers
{
    public static class SecurityManager
    {
        public static PasswordModel EncryptPassword(string password)
        {
            var saltBytes = new byte[128];
            using (var keyGenerator = RandomNumberGenerator.Create())
            {
                keyGenerator.GetBytes(saltBytes);
            }
            var hashBytes = KeyDerivation.Pbkdf2(password, saltBytes, KeyDerivationPrf.HMACSHA512, 10000, 128);

            return new PasswordModel(Convert.ToBase64String(saltBytes), Convert.ToBase64String(hashBytes));
        }

        public static bool ValidatePassword(string password, string passportHash, string passportSalt)
        {
            if (!string.IsNullOrWhiteSpace(password) && !string.IsNullOrWhiteSpace(passportHash) && !string.IsNullOrWhiteSpace(passportSalt))
            {
                try
                {
                    var hashBytes = KeyDerivation.Pbkdf2(password, Convert.FromBase64String(passportSalt),
                        KeyDerivationPrf.HMACSHA512, 10000, 128);
                    return passportHash.Equals(Convert.ToBase64String(hashBytes));
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        public static string GenerateToken(ICollection<Claim> claims, IOptions<TokenAuthentication> options)
        {
            var identity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                options.Value.Issuer,
                options.Value.Audience,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(options.Value.LifeTime)),
                signingCredentials: new SigningCredentials(GetSymmetricSecurityKey(options.Value.Key), SecurityAlgorithms.HmacSha256));

            return $"Bearer {new JwtSecurityTokenHandler().WriteToken(jwt)}";
        }

        public static SymmetricSecurityKey GetSymmetricSecurityKey(string key)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
        }
    }
}