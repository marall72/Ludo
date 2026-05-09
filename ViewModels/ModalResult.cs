namespace Ludo.ViewModels
{
    public class ModalResult
    {
        public bool IsError { get; set; }
        public string Message { get; set; }
        public string Header
        {
            get
            {
                if (IsError)
                    return "خطا";

                return "نتیجه";
            }
        }
    }
}
