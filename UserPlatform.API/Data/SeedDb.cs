using System.Threading.Tasks;
using UserPlatform.API.Helpers.Interfaces;
using UserPlatform.Domain;
using UserPlatform.Domain.Entities;
using UserPlatform.Domain.Enums;

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
            await CheckRolesAsycn();
            await CheckUserAsync("jarevalo", "John", "Arévalo", "jarevalo@yopmail.com", UserType.Administrador);
            await CheckUserAsync("lperez", "Luis", "Pérez", "lperez@yopmail.com", UserType.Operativo);
            await CheckUserAsync("jlopez", "Juan", "López", "jlopez@yopmail.com", UserType.Operativo);
            await CheckUserAsync("acorrea", "Ana", "Correa", "acorrea@yopmail.com", UserType.Operativo);
            await CheckUserAsync("mcaro", "Maria", "Caro", "mcaro@yopmail.com", UserType.Administrador);
        }

        private async Task CheckUserAsync(
            string userName,
            string firstName,
            string lastName,
            string email,
            UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    UserName = userName,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
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
    }
}
