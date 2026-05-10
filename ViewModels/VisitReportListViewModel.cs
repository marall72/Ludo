using Ludo.Business;
using Ludo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Ludo.ViewModels
{
    public class VisitReportListViewModel
    {
        public VisitReportListViewModel()
        {
            
        }

        public VisitReportListViewModel(DateTime from, DateTime to)
        {
            FromDate = HelperMethods.ConvertMiladiToShamsi(from, false);
            ToDate = HelperMethods.ConvertMiladiToShamsi(to, false);

            From = from;
            To = to;
        }

        public double TotalDays { get { return (To - From).TotalDays; } }

        public DateTime From { get; set; }
        public DateTime To { get; set; }

        [Display(Name = "از")]
        public string FromDate { get; set; }
        [Display(Name = "تا")]
        public string ToDate { get; set; }

        public List<VisitReportItem> Items { get; set; }

        [Display(Name = "مشتری")]
        public int ClientId { get; set; }

        public List<SelectListItem> Clients { get; set; }
    }

    public class VisitReportItem
    {
        public int ClientId { get; set; }
        public double TotalHours { get { return Reservations.Select(x => x.To - x.From).Sum(x => x.TotalHours); } }
        public string ClientName { get; set; }
        public string ResponsibleUser { get; set; }
        public List<Reservation> Reservations { get; set; }
        public int VisitCount { get; set; }
    }
}
