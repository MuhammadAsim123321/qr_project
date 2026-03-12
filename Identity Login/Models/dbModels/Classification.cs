using System.ComponentModel.DataAnnotations;

namespace Identity_Login.Models.dbModels
{
    public class Classification
    {
        [Key]
        public int ClassificationId { get; set; }
        [Required]
        public string Name { get; set; }

        public int Minutes { get; set; }
    }
}
