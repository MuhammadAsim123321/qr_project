namespace Identity_Login.Models.ViewModels
{
    public class CustomerPoMatchVm
    {
        public int JobId { get; set; }
        public string JobNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string PartName { get; set; } = string.Empty;
    }
}