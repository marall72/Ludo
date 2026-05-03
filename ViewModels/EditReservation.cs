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

            var nowPlus = DateTime.Now.AddHours(1);
            FromTime = now.ToString("HH:mm");
            ToTime = now.AddHours(1).ToString("HH:mm");
            //todo: if to time is passed AM then date should be added 1 days
        }

        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "IsRequired", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name = "سیستم")]
        public int StationId { get; set; }

        [Required(ErrorMessageResourceName = "IsRequired", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name = "مشتری")]
        public int ClientId { get; set; }

        public List<SelectListItem> Clients { get; set; }
        public List<SelectListItem> Stations { get; set; }


        [Required(ErrorMessageResourceName = "IsRequired", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name = "از ساعت")]
        public string FromDate { get; set; }
        [Required(ErrorMessageResourceName = "IsRequired", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string FromTime { get; set; }
        [Required(ErrorMessageResourceName = "IsRequired", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name = "تا ساعت")]
        public string ToDate { get; set; }
        [Required(ErrorMessageResourceName = "IsRequired", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string ToTime { get; set; }

        [MaxLength(1000, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name = "توضیحات")]
        public string? Description { get; set; }

        public GameSelection Games { get; set; }
    }
}
