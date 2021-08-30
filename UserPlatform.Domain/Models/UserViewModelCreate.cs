using System.ComponentModel.DataAnnotations;

namespace UserPlatform.Domain.Models
{
    public class UserViewModelCreate : UserViewModel
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(20, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }
    }
}
