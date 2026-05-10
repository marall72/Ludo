namespace Ludo.ViewModels
{
    public class PagingViewModel
    {
        public static int PageSize{ get { return 10; } }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int TotalCount { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
    }
}
