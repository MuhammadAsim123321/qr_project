namespace Identity_Login.Models.ViewModels
{
    // ViewModels/RouterJobEditVm.cs
    public class RouterJobEditVm
    {
        public int JobId { get; set; }
        public string CustomerName { get; set; }
        public string JobDetails { get; set; }
        public string PartName { get; set; }
        public string DrawingNo { get; set; }
        public DateTime? Date { get; set; }
        public string VerbalNo { get; set; }
        public double? SurfaceArea { get; set; }
        public int Quantity { get; set; }
        public string RCVDBy { get; set; }
        public string ShippedBy { get; set; }
        public int? ClassificationId { get; set; }
        public int? RunTypeId { get; set; }
        public int? ASFId { get; set; }
        public int? MaterailId { get; set; }
        public int? ProcessId { get; set; }
        public string? ImagePath { get; set; }
        public IFormFile? ImageFile { get; set; }
        public List<IFormFile>? ImageFiles { get; set; } // For multiple images

        public double? TotalIn2OfRunRight { get; set; }

        public string? ExistingImages { get; set; }

        public double? AMPS { get; set; }
        public double? AMPSPerParts { get; set; }
        public int? TimeMinutes { get; set; }
        public int? TimeSeconds { get; set; }

        public bool DisappearAfterShipped { get; set; }

    }
}
