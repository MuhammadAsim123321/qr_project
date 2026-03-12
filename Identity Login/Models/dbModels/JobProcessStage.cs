using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Identity_Login.Enums;

namespace Identity_Login.Models.dbModels
{
    public class JobProcessStage : BaseEntity
    {
        [Key]
        public int JobProcessStageId { get; set; }

        [ForeignKey("RouterJob")]
        public int JobId { get; set; }
        public RouterJob? RouterJob { get; set; }

        [ForeignKey("ProcessStep")]
        public int ProcessStepId { get; set; }
        public ProcessStep? ProcessStep { get; set; }

        public ProcessStageStatus StageStatus { get; set; } 

        public DateTime? CompletedOn { get; set; }

    }
}
