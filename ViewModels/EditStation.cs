using Ludo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Ludo.ViewModels
{
    public class EditStation
    {
        public EditStation()
        {
            
        }

        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "IsRequired", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name = "عنوان")]
        [MaxLength(255, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        [MaxLength(1000)]
        public string? Description { get; set; }

        [Display(Name = "تعداد کاربر")]
        public int PlayerCount { get; set; }

        [Display(Name = "فعال")]
        public bool IsActive { get; set; }

        [Display(Name = "نوع")]
        public StationType SelectedStationType { get; set; }

        public List<SelectListItem> StationTypes
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem { Text = StationType.PC.ToString(), Value = ((int)StationType.PC).ToString() },
                    new SelectListItem { Text = StationType.Xbox.ToString(), Value = ((int)StationType.Xbox).ToString() },
                    new SelectListItem { Text = StationType.PlayStation.ToString(), Value = ((int)StationType.PlayStation).ToString() },
                };
            }
        }

        [Display(Name = "سطح")]
        public StationLevel? SelectedStationLevel { get; set; }


        public List<SelectListItem> StationLevels
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem { Text = StationLevel.Prime.ToString(), Value = ((int)StationLevel.Prime).ToString() },
                    new SelectListItem { Text = StationLevel.Arc.ToString(), Value = ((int)StationLevel.Arc).ToString() },
                    new SelectListItem { Text = StationLevel.Regular.ToString(), Value = ((int)StationLevel.Regular).ToString() },
                    new SelectListItem { Text = StationLevel.Ultimate.ToString(), Value = ((int)StationLevel.Ultimate).ToString() },
                    
                };
            }
        }
    }
}
