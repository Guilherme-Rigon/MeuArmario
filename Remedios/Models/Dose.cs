using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Remedios.Models
{
    public class Dose
    {
        [Key]
        public long Id { get; set; }
        public DateTime DataUso { get; set; }
        public virtual MembroRemedio MembroRemedio { get; set; }
    }
}
