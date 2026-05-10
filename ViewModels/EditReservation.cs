using Ludo.Business;
using Ludo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Ludo.ViewModels
{
    public class EditReservation
    {
        public EditReservation()
        {
            var now = DateTime.Now;
            ToDate = FromDate = HelperMethods.ConvertMiladiToShamsi(now, false);

            var nowPlus = now.AddHours(1);
            
            if(nowPlus.Date > now.Date)
            {
                ToDate = HelperMethods.ConvertMiladiToShamsi(nowPlus, false);
            }

            FromTime = now.ToString("HH:mm");
            ToTime = nowPlus.ToString("HH:mm");

            Games = new GameSelection();
        }

        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "IsRequired", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name = "سیستم")]
        public List<int> StationIds { get; set; }

        [Required(ErrorMessageResourceName = "IsRequired", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name = "مشتری")]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "IsRequired", ErrorMessageResourceType = typeof(Resources.Resource))]
        public int ClientId { get; set; }

        public List<SelectListItem> Clients { get; set; }
        
        public List<SelectListItem> Stations { get; set; }


        [Required(ErrorMessageResourceName = "IsRequired", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name = "از")]
        public string FromDate { get; set; }
        [Required(ErrorMessageResourceName = "IsRequired", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string FromTime { get; set; }
        [Required(ErrorMessageResourceName = "IsRequired", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name = "تا")]
        public string ToDate { get; set; }
        [Required(ErrorMessageResourceName = "IsRequired", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string ToTime { get; set; }

        [MaxLength(1000, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name = "توضیحات")]
        public string? Description { get; set; }

        public GameSelection Games { get; set; }
    }
}
