using Identity_Login.Models.dbModels;

namespace Identity_Login.Models.ViewModels
{
    public class RouterjobPdfVM
    {
        public int JobId { get; set; }

        public string JobNumber { get; set; }
        public string CustomerName { get; set; }
        public string PartName { get; set; }
        public string DrawingNo { get; set; }
        public DateTime? Date { get; set; }
        public string VerbalNo { get; set; }
        public int Quantity { get; set; }

        public string RCVDBy { get; set; }
        public string ShippedBy { get; set; }
        public string JobDetails { get; set; }
        public string Status { get; set; }
        public string QrCodeBase64 { get; set; }
        public string? ImageBase64 { get; set; }

        public string? ClassificationName { get; set; }
        public string? RunTypeName { get; set; }
        public string? ASFName { get; set; }
        public string? MaterailName { get; set; }

        public string? JobProcessName { get; set; }

        public double? SurfaceArea { get; set; }
        public double? TotalIn2OfRun { get; set; }
        public int? TimeMinutes { get; set; }
        public int? TimeSeconds { get; set; }
        public double? AMPS { get; set; }
        public double? AMPSPerParts { get; set; }

        public List<UploadImage>? UploadImages { get; set; }
        public List<string>? UploadImageBase64List { get; set; }
        public double? TotalIn2OfRunRight { get; set; }
        // 🔹 Add these flags
        public bool HideRunTypeAndSurface { get; set; }
        public bool HideAmps { get; set; }




    }
}
