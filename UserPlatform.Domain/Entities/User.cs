using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using UserPlatform.Domain.Enums;

namespace UserPlatform.Domain.Entities
{
    public class User : IdentityUser
    {
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string FirstName { get; set; }

        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string LastName { get; set; }

        public UserType UserType { get; set; }
    }
}
