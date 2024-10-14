using System.ComponentModel.DataAnnotations;

namespace BusQuei.Models
{
    public class Bus
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "A placa é obrigatória.")]
        [StringLength(7, ErrorMessage = "A placa deve ter 7 caracteres.")]
        public string LicensePlate { get; set; }

        [Required(ErrorMessage = "O modelo é obrigatório.")]
        [StringLength(50)]
        public string Model { get; set; }

        [Required(ErrorMessage = "A capacidade é obrigatória.")]
        [Range(1, 100, ErrorMessage = "A capacidade deve ser entre 1 e 100.")]
        public int Capacity { get; set; }

        [Required]
        [RegularExpression("EmOperação|EmManutenção|Inativo", ErrorMessage = "Status inválido. Valores permitidos: EmOperação, EmManutenção, Inativo.")]
        public string Status { get; set; }

        public ICollection<Maintenance> Maintenances { get; set; }

        public int? RouteId { get; set; }
        public Route Route { get; set; }
    }
}
