using System.ComponentModel.DataAnnotations;
using static System.Collections.Specialized.BitVector32;

namespace Identity_Login.Models.dbModels
{
    public class JobProcess : BaseEntity
    {
        [Key]
        public int ProcessId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;  // e.g., "Anodizing", "Passivation"

        [MaxLength(250)]
        public string Description { get; set; } = string.Empty;

        public ICollection<ProcessStep> Steps { get; set; } = new List<ProcessStep>();
    }
}
