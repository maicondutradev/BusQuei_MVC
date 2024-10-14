using System.ComponentModel.DataAnnotations;

namespace BusQuei.Models
{
    public class Route
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da rota é obrigatório.")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "O ponto de partida é obrigatório.")]
        [StringLength(200)]
        public string Departure { get; set; }

        [Required(ErrorMessage = "O ponto de chegada é obrigatório.")]
        [StringLength(200)]
        public string Arrival { get; set; }

        [Required(ErrorMessage = "O horário de partida é obrigatório.")]
        public TimeSpan DepartureTime { get; set; }

        [Required(ErrorMessage = "O horário de chegada é obrigatório.")]
        public TimeSpan ArrivalTime { get; set; }

        public ICollection<Bus> Buses { get; set; }
    }
}
