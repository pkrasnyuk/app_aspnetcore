using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using WebAppCore.BLL.Exceptions;
using WebAppCore.BLL.Helpers;
using WebAppCore.BLL.Interfaces;
using WebAppCore.BLL.Models;
using WebAppCore.BLL.Validations;
using WebAppCore.DAL.Interfaces;
using WebAppCore.DAL.Repositories;
using WebAppCore.Domain.Entities;

namespace WebAppCore.BLL.Managers
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private static IOptions<TokenAuthentication> _options;
        private readonly IMapper _mapper;
        private readonly IEntityRepository<Role<ObjectId>, ObjectId> _roleRepository;
        private readonly IEntityRepository<User<ObjectId>, ObjectId> _userRepository;

        public AuthenticationManager(IEntityRepository<User<ObjectId>, ObjectId> userRepository,
            IEntityRepository<Role<ObjectId>, ObjectId> roleRepository, IMapper mapper,
            IOptions<TokenAuthentication> options)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _options = options;
        }

        public async Task<UserModel> LoginUserAsync(LoginUserModel model, HttpContext httpContext)
        {
            UserModel userModel;

            var validationResult = await new LoginUserModelValidation().ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                throw new ConflictRequestException(validationResult.Errors.ToAggregateResult());
            }

            var user = await ((UserRepository) _userRepository).GetUserByEmailAsync(model.Email);
            if (user != null && !user.IsLocked && SecurityManager.ValidatePassword(model.Password, user.PassportHash, user.PassportSalt))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, model.Email)
                };

                var userRoles = await ((RoleRepository) _roleRepository).GetUserRolesAsync(model.Email);
                if (userRoles != null && userRoles.Any())
                {
                    claims.AddRange(userRoles.Select(userRole => new Claim(ClaimsIdentity.DefaultRoleClaimType, userRole.Name)));
                }

                userModel = _mapper.Map<UserModel>(user);
                userModel.Token = SecurityManager.GenerateToken(claims, _options);

                await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(new ClaimsIdentity(null, CookieAuthenticationDefaults.AuthenticationScheme)),
                    new AuthenticationProperties {IsPersistent = model.RememberMe});
            }
            else
            {
                throw new ResourceSearchException($"An user with email '{model.Email}' and password '{model.Password}' not found");
            }

            return userModel;
        }

        public async Task LogoutUserAsync(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}