namespace Identity_Login.Models.ViewModels
{
    public class CustomerJobSearchViewModel
    {
        public string JobNumber { get; set; } = string.Empty;
        public string Process { get; set; } = string.Empty;
        public List<CustomerJobProcessStepVm> Steps { get; set; } = new();

    }
}
