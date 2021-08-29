using System;
using UserPlatform.API.Helpers.Interfaces;
using UserPlatform.Domain.Entities;
using UserPlatform.Domain.Enums;
using UserPlatform.Domain.Models;

namespace UserPlatform.API.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public UserViewModel ToUserViewModel(User user)
        {
            return new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserType = user.UserType == 0 ? "Administrador" : "Operativo"
            };
        }

        public User ToUser(UserViewModel model, bool isNew)
        {
            User user = new();
            user.Id = model.Id;
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.UserType = (UserType)(model.UserType == "Administrador" ? 0 : 1);
            if (isNew)
            {
                user.Id = Guid.NewGuid().ToString();
            }

            return user;
        }
    }
}
