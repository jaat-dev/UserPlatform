using System.Threading.Tasks;
using UserPlatform.Domain.Entities;
using UserPlatform.Domain.Models;

namespace UserPlatform.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<ResponseViewModel> GetAll();
        Task<ResponseViewModel> GetById(string id);
        Task<ResponseViewModel> Create(UserViewModelCreate model);
        Task<ResponseViewModel> Update(string id, UserViewModel model);
        Task<ResponseViewModel> Delete(int id);
        Task<ResponseViewModel> LogIn(LoginViewModel model);
        Task<ResponseViewModel> LogOut();
    }
}
