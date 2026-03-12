using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Identity_Login.Models.dbModels
{
    public class UploadImage
    {
        [Key]
        public int UploadImageId { get; set; }

        [Required]
        public string ImagePath { get; set; } = string.Empty; // Path to image in wwwroot

        [ForeignKey("RouterJob")]
        public int RouterJobId { get; set; }
        public RouterJob RouterJob { get; set; }

    }
}
