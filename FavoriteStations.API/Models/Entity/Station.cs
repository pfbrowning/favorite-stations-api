using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FavoriteStations.Models.Entity {
    public class Station {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  
        public int? StationId { get; set; }
        [Required]
        [MaxLength(255)]
        public string UserId { get; set; }
        [MaxLength(255)]
        public string Title { get; set; }
        [Required]
        public string Url { get; set; }
        public string IconUrl { get; set; }
        public string Notes { get; set; }
    }
}