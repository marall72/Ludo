using System.ComponentModel.DataAnnotations;

namespace Ludo.ViewModels
{
    public class EditUser
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "IsRequired", ErrorMessageResourceType = typeof(Resources.Resource))]
        [MaxLength(255, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name = "نام")]
        public string Firstname { get; set; } = string.Empty;

        [Required(ErrorMessageResourceName = "IsRequired", ErrorMessageResourceType = typeof(Resources.Resource))]
        [MaxLength(255, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name = " نام خانوادگی")]
        public string Lastname { get; set; } = string.Empty;

        [Required(ErrorMessageResourceName = "IsRequired", ErrorMessageResourceType = typeof(Resources.Resource))]
        [MaxLength(11, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name = "موبایل")]
        [RegularExpression(@"^09\d{9}$", ErrorMessage = "فرمت شماره موبایل صحیح نیست. (مثال: 09123456789)")]
        public string Mobile { get; set; } = string.Empty;

        [Required(ErrorMessageResourceName = "IsRequired", ErrorMessageResourceType = typeof(Resources.Resource))]
        [MaxLength(20, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessageResourceName = "IsRequired", ErrorMessageResourceType = typeof(Resources.Resource))]
        [MaxLength(20, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name = "نام کاربری")]
        public string Username { get; set; } = string.Empty;

        [Display(Name = "ایمیل")]
        [MaxLength(255, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resources.Resource))]
        [EmailAddress(ErrorMessage = "فرمت ایمیل صحیح نیست.")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "مدیر")]
        public bool IsAdmin { get; set; }
        [Display(Name = "فعال")]
        public bool IsActive { get; set; }
    }
}
