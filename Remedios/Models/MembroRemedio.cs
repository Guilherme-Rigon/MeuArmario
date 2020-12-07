using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Remedios.Models
{
    public class MembroRemedio
    {
        [ForeignKey("UserId")]
        public MembroFamilia User { get; set; }
        public virtual long UserId { get; set; }
        [ForeignKey("RemedioId")]
        public Remedio Remedio { get; set; }
        public virtual long RemedioId { get; set; }
        public virtual Receita Receita { get; set; }
        public virtual ICollection<Dose> Doses { get; set; }
        public DateTime DataInicio { get; set; }
    }
}
