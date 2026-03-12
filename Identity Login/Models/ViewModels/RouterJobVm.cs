using Identity_Login.Enums;
using Identity_Login.Models.dbModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identity_Login.Models.ViewModels
{
    public class RouterJobVm
    {
        public int JobId { get; set; }
        public int Quantity { get; set; }
        public string ProcessStep { get; set; } = string.Empty;
        public string UpdatedOnDisplay { get; set; } = string.Empty;

        public string JobNumber { get; set; } = string.Empty;   // External ID for customers

        public string CustomerName { get; set; } = string.Empty;
        public string JobDetails { get; set; } = string.Empty;

        public string QrCodeData { get; set; } = string.Empty;  // Data encoded in QR

        public string PdfFilePath { get; set; } = string.Empty; // Path to generated router PDF

        public string PartName { get; set; } = string.Empty;
        public string DrawingNo { get; set; } = string.Empty;

        public string VerbalNo { get; set; } = string.Empty;


        public string ClassificationName { get; set; } = string.Empty;
        public string ProcessName { get; set; } = string.Empty;
        public string RunTypeName { get; set; } = string.Empty;
        public string AsfName { get; set; } = string.Empty;
        public string MaterialName { get; set; } = string.Empty;

        public bool DisappearAfterShipped { get; set; }


    }
}
