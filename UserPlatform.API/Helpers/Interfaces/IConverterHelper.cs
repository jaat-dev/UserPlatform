using UserPlatform.Domain.Entities;
using UserPlatform.Domain.Models;

namespace UserPlatform.API.Helpers.Interfaces
{
    public interface IConverterHelper
    {
        UserViewModel UserToUserViewModel(User user);

        User UserViewModelToUser(UserViewModel model, bool isNew);
        //UserViewModel UserViewModelCreateToUserViewModel(UserViewModelCreate model);
    }
}
