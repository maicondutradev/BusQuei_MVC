using System.ComponentModel.DataAnnotations;

namespace BusQuei.Models
{
    public class Maintenance
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BusId { get; set; }
        public Bus Bus { get; set; }

        [Required(ErrorMessage = "A data é obrigatória.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "O tipo de manutenção é obrigatório.")]
        [StringLength(100)]
        public string Type { get; set; }

        [Required]
        [RegularExpression("Agendada|EmAndamento|Concluída", ErrorMessage = "Status inválido. Valores permitidos: Agendada, EmAndamento, Concluída.")]
        public string Status { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }
    }
}
