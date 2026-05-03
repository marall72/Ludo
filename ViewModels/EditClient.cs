using Ludo.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ludo.ViewModels
{
    public enum Gender
    {
        Male, Female
    }

    public class EditClient
    {
        public int Id { get; set; }

        [MaxLength(255, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Required(ErrorMessageResourceName = "IsRequired", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name = "نام")]
        public string Firstname { get; set; }

        [MaxLength(255, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessageResourceName = "IsRequired", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string Lastname { get; set; }

        [MaxLength(11, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name = "موبایل")]
        [RegularExpression(@"^09\d{9}$", ErrorMessage = "فرمت شماره موبایل صحیح نیست. (مثال: 09123456789)")]
        [Required(ErrorMessageResourceName = "IsRequired", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string Mobile { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        [MaxLength(255, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name = "ایمیل")]
        [EmailAddress(ErrorMessage = "فرمت ایمیل صحیح نیست.")]
        public string? Email { get; set; }

        [Display(Name = "جنسیت")]
        public Gender Gender { get; set; }
    }
}
