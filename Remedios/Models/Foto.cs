using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Remedios.Models
{
    public class Foto
    {
        [Key]
        public long Id { get; set; }
        public byte[] foto { get; set; }
        public string ContentType { get; set; }
        public virtual MembroFamilia Membro { get; set; }
    }
}
