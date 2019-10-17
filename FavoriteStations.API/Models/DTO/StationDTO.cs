using System.ComponentModel.DataAnnotations;

namespace FavoriteStations.Models {
    public class StationDTO {
        public int? StationId { get; set; }
        [MaxLength(255)]
        public string Title { get; set; }
        [Required]
        public string Url { get; set; }
        public string IconUrl { get; set; }
        public string Notes { get; set; }
    }
}