using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Remedios.Models
{
    public class Remedio
    {
        [Key]
        public long Id { get; set; }
        [Required(ErrorMessage = "É necessário informar o nome do medicamento a ser cadastrado.")]
        [MaxLength(50)]
        public string Nome { get; set; }
        [Required(ErrorMessage = "É necessário informar o preço do medicamento a ser cadastrado.")]
        public double Preco { get; set; }
        [Required(ErrorMessage = "Somente com a informação de quantidade podemos alerta-lo sobre a falta de algum medicamento")]
        public int Quantidade { get; set; }
        [MaxLength(20, ErrorMessage = "Nome muito grande para a tarja.")]
        public string Tarja { get; set; }
        [Required(ErrorMessage = "É necessário informar a validade do medicamento.")]
        [DataType(DataType.Date)]
        public DateTime Validade { get; set; }
        public ICollection<MembroRemedio> Usuarios { get; set; }
    }
}
