using System.ComponentModel.DataAnnotations;

namespace NEWLOOK.Models.NewLook
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Pswd { get; set; } = null!;
    }
}