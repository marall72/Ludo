using Ludo.Business;
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
            ToDate = HelperMethods.ConvertMiladiToShamsi(from, false);
            FromDate = HelperMethods.ConvertMiladiToShamsi(to, false);
        }
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
        public string ClientName { get; set; }
        public int VisitCount { get; set; }
    }
}
