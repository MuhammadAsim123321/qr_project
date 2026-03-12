namespace Identity_Login.Models.ViewModels
{
    public class CustomerJobProcessStepVm
    {
        public int StepOrder { get; set; }
        public string StepName { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";

    }
}
