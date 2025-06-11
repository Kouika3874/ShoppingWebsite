namespace Project14.ViewModels
{
    public class AjaxResponseViewModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int? CartCount { get; set; }
        public object Data { get; set; }
    }
}
