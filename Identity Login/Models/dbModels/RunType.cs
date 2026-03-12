using System.ComponentModel.DataAnnotations;

namespace Identity_Login.Models.dbModels
{
    public class RunType
    {
        [Key]
        public int RunTypeId { get; set; }
        [Required]
        public string Name { get; set; }

    }
}
