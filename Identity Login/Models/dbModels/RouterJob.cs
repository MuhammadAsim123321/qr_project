using Identity_Login.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identity_Login.Models.dbModels
{
    public class RouterJob : BaseEntity
    {
        [Key]
        public int JobId { get; set; }

        [Required, MaxLength(50)]
        public string JobNumber { get; set; } = string.Empty;   

        [MaxLength(100)]
        public string CustomerName { get; set; } = string.Empty;


        [MaxLength(100)]
        public string PartName { get; set; } = string.Empty; 

        [MaxLength(100)]
        public string DrawingNo { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        [MaxLength(100)]
        public string VerbalNo { get; set; } = string.Empty;

        public double? SurfaceArea { get; set; } 

        public int Quantity { get; set; }

        public double? TotalIn2OfRunRight { get; set; }
        //[MaxLength(100)]
        //public string Materail { get; set; } = string.Empty;

        [MaxLength(100)]
        public string RCVDBy { get; set; } = string.Empty;

        [MaxLength(100)]
        public string ShippedBy { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? JobDetails { get; set; }
        [Required]
        public string QrCodeData { get; set; } = string.Empty;  // Data encoded in QR

        public string? ImagePath { get; set; }
        public ICollection<UploadImage> UploadImages { get; set; } = new List<UploadImage>();

        [ForeignKey("JobProcess")]
        public int? ProcessId { get; set; }
        public JobProcess? JobProcess { get; set; }

        [ForeignKey("Classification")]
        public int? ClassificationId { get; set; }
        public Classification? Classification { get; set; }

        [ForeignKey("RunType")]
        public int? RunTypeId { get; set; }
        public RunType? RunType { get; set; }

        //partial decimal TotalIn2OfRunRight { get;set; }

        [ForeignKey("ASF")]
        public int? ASFId { get; set; }
        public ASF? ASF { get; set; }

        [ForeignKey("Materail")]
        public int? MaterailId { get; set; }
        public Materail? Materail { get; set; }

        [Required]
        public string PdfFilePath { get; set; } = string.Empty; // Path to generated router PDF
        public JobStatus Status { get; set; } = JobStatus.Pending;

        public ICollection<JobProcessStage> ProcessStages { get; set; } = new List<JobProcessStage>();


        // New column for handling disappearance after shipped
        public bool DisappearAfterShipped { get; set; } = false;

    }
}
