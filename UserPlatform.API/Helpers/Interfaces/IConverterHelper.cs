using UserPlatform.Domain.Entities;
using UserPlatform.Domain.Models;

namespace UserPlatform.API.Helpers.Interfaces
{
    public interface IConverterHelper
    {
        UserViewModel ToUserViewModel(User user);

        User ToUser(UserViewModel model, bool isNew);
    }
}
