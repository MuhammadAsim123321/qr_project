namespace Identity_Login.Models.ViewModels
{
    public class ChangeStageVm
    {
        public int JobId { get; set; }
        public int? CurrentProcessStepId { get; set; }
        public int? NewProcessStepId { get; set; }
        public string? CustomerName { get; set; } 

    }
}
