using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserPlatform.API.Helpers.Interfaces;
using UserPlatform.API.Services.Interfaces;
using UserPlatform.Domain;
using UserPlatform.Domain.Entities;
using UserPlatform.Domain.Models;

namespace UserPlatform.API.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _db;
        private readonly IConverterHelper _converter;
        private readonly IUserHelper _userHelper;
        private readonly IConfiguration _configuration;

        public UserService(
            DataContext db,
            IConverterHelper converter,
            IUserHelper userHelper,
            IConfiguration configuration)
        {
            _db = db;
            _converter = converter;
            _userHelper = userHelper;
            _configuration = configuration;
        }

        public async Task<ResponseViewModel> GetAll()
        {
            List<UserViewModel> listUserModel = new();
            try
            {
                List<User> users = await _db.Users.ToListAsync();
                if (users == null)
                {
                    return new ResponseViewModel { IsSuccess = false, Message = "Registros no encontrados." };
                }

                foreach (User user in users)
                {
                    listUserModel.Add(_converter.UserToUserViewModel(user));
                }

                return new ResponseViewModel { IsSuccess = true, Message = "Consulta Exitosa", Data = listUserModel };
            }
            catch (Exception ex)
            {
                return new ResponseViewModel { IsSuccess = false, Message = ex.Message, IsException = true };
            }
        }

        public async Task<ResponseViewModel> GetById(string id)
        {
            User user = await _userHelper.GetUserAsync(Guid.Parse(id));
            if (user == null)
            {
                return new ResponseViewModel { IsSuccess = false, Message = "Usuarios no existe." };
            }

            return new ResponseViewModel { IsSuccess = true, Message = "Consulta Exitosa", Data = _converter.UserToUserViewModel(user) };
        }

        public async Task<ResponseViewModel> Create(UserViewModelCreate model)
        {
            try
            {
                if (model.UserType != "Administrador" && model.UserType != "Operativo")
                {
                    return new ResponseViewModel { IsSuccess = false, Message = "El tipo de usuario no existe." };
                }

                User user = _converter.UserViewModelToUser(model, true);
                await _userHelper.AddUserAsync(user, model.Password);
                await _userHelper.AddUserToRoleAsync(user, user.UserType.ToString());
                return new ResponseViewModel { IsSuccess = true, Message = "Usuario creado exitosamante.", Data = _converter.UserToUserViewModel(user) };
            }
            catch (DbUpdateException updateEx)
            {
                if (updateEx.InnerException.Message.Contains("duplicate"))
                {
                    return new ResponseViewModel { IsSuccess = false, Message = "Ya existe Usuario.", IsException = true };
                }
                else
                {
                    return new ResponseViewModel { IsSuccess = false, Message = updateEx.InnerException.Message, IsException = true };
                }
            }
            catch (Exception ex)
            {
                return new ResponseViewModel { IsSuccess = false, Message = ex.Message, IsException = true };
            }
        }

        public async Task<ResponseViewModel> Update(string id, UserViewModel model)
        {
            if (id != model.Id)
            {
                return new ResponseViewModel { IsSuccess = false, Message = "Error en la solicitud." };
            }

            try
            {
                User user = _converter.UserViewModelToUser(model, false);
                await _userHelper.UpdateUserAsync(user);
                return new ResponseViewModel { IsSuccess = true, Message = "Registro modificado exitosamente.", Data = _converter.UserToUserViewModel(user) };
            }
            catch (DbUpdateException updateEx)
            {
                if (updateEx.InnerException.Message.Contains("duplicate"))
                {
                    return new ResponseViewModel { IsSuccess = false, Message = "Ya existe Usuario.", IsException = true };
                }
                else
                {
                    return new ResponseViewModel { IsSuccess = false, Message = updateEx.InnerException.Message, IsException = true };
                }
            }
            catch (Exception ex)
            {
                return new ResponseViewModel { IsSuccess = false, Message = ex.Message, IsException = true };
            }
        }

        public async Task<ResponseViewModel> Delete(int id)
        {
            try
            {
                User user = await _db.Users.FindAsync(id);
                if (user == null)
                {
                    return new ResponseViewModel { IsSuccess = false, Message = "Registro no encontrado." };
                }

                _db.Remove(user);
                await _db.SaveChangesAsync();
                return new ResponseViewModel { IsSuccess = true, Message = "Registro eliminado exitosamente." };
            }
            catch (Exception ex)
            {
                return new ResponseViewModel { IsSuccess = false, Message = ex.Message, IsException = true };
            }
        }

        public async Task<ResponseViewModel> LogIn(LoginViewModel model)
        {
            try
            {
                User user = await _userHelper.GetUserAsync(model.Email);
                if (user == null)
                {
                    return new ResponseViewModel { IsSuccess = false, Message = "Usuarios no existe." };
                }

                model.Email = user.UserName;
                Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.LoginAsync(model);
                if (!result.Succeeded)
                {
                    return new ResponseViewModel { IsSuccess = false, Message = "Email o contraseña incorrectos." };
                }

                return new ResponseViewModel { IsSuccess = true, Message = "Successful log-in", Data = GetToken(user) };
            }
            catch (Exception ex)
            {
                return new ResponseViewModel { IsSuccess = false, Message = ex.Message, IsException = true };
            }
        }

        private object GetToken(User user)
        {
            Claim[] claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new(
                _configuration["Tokens:Issuer"],
                _configuration["Tokens:Audience"],
                claims,
                expires: DateTime.UtcNow.AddDays(99),
                signingCredentials: credentials);

            return new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                user
            };
        }

        public async Task<ResponseViewModel> LogOut()
        {
            try
            {
                await _userHelper.LogoutAsync();
                return new ResponseViewModel { IsSuccess = true, Message = "Sesión cerrada exitosamente!" };
            }
            catch (Exception ex)
            {
                return new ResponseViewModel { IsSuccess = false, Message = ex.Message, IsException = true };
            }
        }
    }
}
