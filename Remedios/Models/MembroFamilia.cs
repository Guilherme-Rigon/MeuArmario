using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Remedios.Models
{
    public class MembroFamilia
    {
        [Key]
        public long Id { get; set; }
        [Required(ErrorMessage = "Você deve informar um nome.")]
        [MaxLength(50, ErrorMessage = "O nome deve ter menos de 50 caracteres.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Você deve informar o sobrenome.")]
        [MaxLength(80, ErrorMessage = "Seu sobrenome deve ter menos de 80 caracteres.")]
        public string Sobrenome { get; set; }
        [MaxLength(11, ErrorMessage = "Seu CPF deve ter 11 caracteres sem o uso de '.' (ponto)")]
        [MinLength(11, ErrorMessage = "Seu CPF deve ter 11 caracteres sem o uso de '.' (ponto)")]
        public string CPF { get; set; }
        [Required(ErrorMessage = "Você deve informar a data de nascimento.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Nascimento { get; set; }
        public virtual ICollection<MembroRemedio> Remedios { get; set; }
        public virtual ICollection<Foto> Fotos { get; set; }
    }
}
