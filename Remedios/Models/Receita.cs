using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Remedios.Models
{
    public class Receita
    {
        [Key]
        public long Id { get; set; }
        [Required(ErrorMessage = "O dignóstico é referente ao motivo de estar usando o medicamento.")]
        [MaxLength(100, ErrorMessage = "Descrição muito grande para um diagnóstico.")]
        public string Diagnostico { get; set; }
        [Required(ErrorMessage = "É importante ter registrado o nome do médico que receitou o medicamento.")]
        [MaxLength(60, ErrorMessage = "O nome do médico deve conter até 60 caracteres.")]
        public string Medico { get; set; }
        [Required(ErrorMessage = "A instrução é referente ao procedimento de uso do medicamento.")]
        [MaxLength(100, ErrorMessage = "A descrição do procedimento deve conter até 100 caracteres.")]
        public string Instrucao { get; set; }
        [Required]
        public bool Temporario { get; set; }
        public virtual ICollection<MembroRemedio> UsuarioRemedio { get; set; }
    }
}
