using System;
using System.Linq;
using System.Threading.Tasks;
using UserPlatform.API.Helpers.Interfaces;
using UserPlatform.Common.Enums;
using UserPlatform.Domain;
using UserPlatform.Domain.Entities;

namespace UserPlatform.API.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckDocumentTypesAsync();
            await CheckRolesAsycn();
            await CheckUserAsync("1010", "Luis", "Pérez", "luis@yopmail.com", "311 321 3624", "Calle 5ta Av 6ta", UserType.Administrador);
            await CheckUserAsync("2020", "Juan", "López", "juan@yopmail.com", "311 322 3625", "Calle 5ta Av 7ta", UserType.Operativo);
            await CheckUserAsync("3030", "Ana", "Correa", "ana@yopmail.com", "311 323 3626", "Calle 5ta Av 8ta", UserType.Operativo);
            await CheckUserAsync("4040", "Maria", "Caro", "maria@yopmail.com", "311 324 3627", "Calle 5ta Av 9ta", UserType.Administrador);
        }

        private async Task CheckUserAsync(
            string document, 
            string firstName, 
            string lastName, 
            string email, 
            string phoneNumber, 
            string address, 
            UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    Address = address,
                    Document = document,
                    DocumentType = _context.DocumentTypes.FirstOrDefault(x => x.Description == "Cédula"),
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    PhoneNumber = phoneNumber,
                    UserName = email,
                    UserType = userType
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }
        }

        private async Task CheckRolesAsycn()
        {
            await _userHelper.CheckRoleAsync(UserType.Administrador.ToString());
            await _userHelper.CheckRoleAsync(UserType.Operativo.ToString());
        }

        private async Task CheckDocumentTypesAsync()
        {
            if (!_context.DocumentTypes.Any())
            {
                _context.DocumentTypes.Add(new DocumentType { Description = "Tarjeta de Identidad" });
                _context.DocumentTypes.Add(new DocumentType { Description = "Cédula de Ciudadanía" });
                _context.DocumentTypes.Add(new DocumentType { Description = "Cédula de Extranjería" });
                _context.DocumentTypes.Add(new DocumentType { Description = "Pasaporte" });
                _context.DocumentTypes.Add(new DocumentType { Description = "NIT" });
                await _context.SaveChangesAsync();
            }
        }
    }
}
