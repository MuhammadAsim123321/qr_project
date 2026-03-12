using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Collections.Specialized.BitVector32;

namespace Identity_Login.Models.dbModels
{
    public class StaffStationMapping:BaseEntity
    {
        [Key]
        public int MappingId { get; set; }
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }

        [ForeignKey("Station")]
        public int StationId { get; set; }
        public Station? Station { get; set; }

    }
}
