using System.ComponentModel.DataAnnotations;

namespace Identity_Login.Models.dbModels
{
    public class Materail
    {
        [Key]
        public int MaterailId { get; set; }
        [Required]
        public string Name { get; set; }

    }
}
