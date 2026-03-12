using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identity_Login.Models.dbModels
{
    public class Station : BaseEntity
    {
        [Key]
        public int StationId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;  // e.g., "Anodizing Line 1"

        [ForeignKey("ProcessStep")]
        public int ProcessStepId { get; set; }
        public ProcessStep? ProcessStep { get; set; }

        public ICollection<StaffStationMapping> StaffMembers { get; set; } = new List<StaffStationMapping>();
    }
}
