using System;
using UserPlatform.API.Helpers.Interfaces;
using UserPlatform.Domain.Entities;
using UserPlatform.Domain.Enums;
using UserPlatform.Domain.Models;

namespace UserPlatform.API.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public UserViewModel UserToUserViewModel(User user)
        {
            return new()
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserType = user.UserType == 0 ? "Administrador" : "Operativo"
            };
        }

        public User UserViewModelToUser(UserViewModel model, bool isNew)
        {
            return new()
            {
                Id = isNew ? Guid.NewGuid().ToString() : model.Id,
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserType = (UserType)(model.UserType == "Administrador" ? 0 : 1)
            };
        }
    }
}
