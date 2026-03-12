using System.ComponentModel.DataAnnotations.Schema;

namespace Identity_Login.Models.dbModels
{
    public abstract class BaseEntity
    {
        public DateTime? CreatedOn { get; set; }

        [ForeignKey("CreatedByUser")]
        public string? CreatedBy { get; set; }
        public ApplicationUser? CreatedByUser { get; set; }

        public DateTime? UpdatedOn { get; set; }

        [ForeignKey("UpdatedByUser")]
        public string? UpdatedBy { get; set; }
        public ApplicationUser? UpdatedByUser { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
