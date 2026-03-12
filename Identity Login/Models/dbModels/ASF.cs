using System.ComponentModel.DataAnnotations;

namespace Identity_Login.Models.dbModels
{
    public class ASF
    {
        [Key]
        public int ASFId { get; set; }
        [Required]
        public string Name { get; set; }

    }
}
