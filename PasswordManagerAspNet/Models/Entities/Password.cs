using System.ComponentModel.DataAnnotations;

namespace PasswordManagerAspNet.Models.Entities
{
    public class Password
    {
        public string? Id { get; set; }
        public string? UserMail { get; set; }

        [Required(ErrorMessage = "Please enter a name")]
        [StringLength(150, ErrorMessage = "The account name cannot be longer than 150 characters.")]
        [Display(Name = "Account Name*")]
        public string AccountName { get; set; }

        [Required(ErrorMessage = "Please enter an e-mail")]
        [StringLength(150, ErrorMessage = "The account e-mail cannot be longer than 150 characters.")]
        [Display(Name = "Account Email*")]
        public string AccountEmail { get; set; }

        [Required(ErrorMessage = "Please enter a password")]
        [StringLength(300, ErrorMessage = "The password cannot be longer than 300 characters.")]
        [Display(Name = "Password*")]
        public string PasswordValue { get; set; }
        public string? Notes { get; set; }
    }
}
