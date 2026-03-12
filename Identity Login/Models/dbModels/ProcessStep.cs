using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Identity_Login.Models.dbModels
{
    public class ProcessStep : BaseEntity
    {
        [Key]
        public int ProcessStepId { get; set; }

        [ForeignKey("JobProcess")]
        [Display(Name = "Process")]
        public int ProcessId { get; set; }
        public JobProcess? JobProcess { get; set; }

        [Required, MaxLength(100)]
        public string StepName { get; set; } = string.Empty;   // e.g., "Ready for racking"


        [Display(Name ="Process Order")]
        public int StepOrder { get; set; }   // sequence number
        public bool IsOptional { get; set; } = false;

        public ICollection<Station> Stations { get; set; } = new List<Station>();
    }
}
