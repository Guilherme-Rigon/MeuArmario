using Remedios.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Remedios.ViewModel
{
    [NotMapped]
    public class CreateReceitaViewModel : Receita
    {
        public RemediosASelecionar[] Remedios { get; set; }
    }
    [NotMapped]
    public class RemediosASelecionar : Remedio
    {
        public bool Selecionado { get; set; }
    }
}
