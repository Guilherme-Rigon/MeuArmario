using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Remedios.Data;
using Remedios.Models;
using Remedios.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Remedios.Controllers
{
    public class ReceitaController : Controller
    {
        private readonly RemediosDbContext _context;
        public ReceitaController(RemediosDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(long? id)
        {
            if(id != null)
            {
                ViewData["Id"] = id;
                return View(_context.Receitas.Include(x => x.UsuarioRemedio).Where(x => x.UsuarioRemedio.Where(y => y.UserId == id).Count() > 0).ToList());
            }
            return NotFound();
        }
        public IActionResult Create(long? id)
        {
            ViewData["Id"] = id;
            return View(new CreateReceitaViewModel { Remedios = CarregarRemedios(id) });
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateReceitaViewModel model, long? id)
        {
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    if (model.Remedios.Where(x => x.Selecionado).Count() > 0)
                    {
                        Receita receita = new Receita();
                        receita.Instrucao = model.Instrucao;
                        receita.Medico = model.Medico;
                        receita.Temporario = model.Temporario;
                        receita.Diagnostico = model.Diagnostico;
                        receita.UsuarioRemedio = new List<MembroRemedio>();
                        foreach (var item in model.Remedios.Where(x => x.Selecionado))
                        {
                            receita.UsuarioRemedio.Add(new MembroRemedio { RemedioId = item.Id, UserId = (long)id, DataInicio = DateTime.Now });
                        }
                        _context.Receitas.Add(receita);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index), new { id = id });
                    }
                    else
                    {
                        ViewData["Error"] = "Você precisa vincular ao menos um remédio a essa receita";
                    }
                }
                else
                {
                    return RedirectToAction(nameof(Index), new { id = id });
                }
            }
            return View(model);
        }
        private RemediosASelecionar[] CarregarRemedios(long? id)
        {
            if (id != null)
            {
                return _context.Remedios.Include(x => x.Usuarios)
                    .Where(x => x.Usuarios.Where(y => y.UserId == id).LongCount() == 0).Select(x => new RemediosASelecionar()
                    {
                        Id = x.Id,
                        Nome = x.Nome,
                        Selecionado = false
                    }).AsNoTracking().ToArray();
            }
            else
            {
                return _context.Remedios.Select(x => new RemediosASelecionar()
                    {
                        Id = x.Id,
                        Nome = x.Nome,
                        Selecionado = false
                    }).AsNoTracking().ToArray();
            }
        }
        public IActionResult Detail(long? id, long? recId)
        {
            if (id != null && recId != null)
            {
                ViewData["Id"] = id;
                return View(_context.Receitas.Include(x => x.UsuarioRemedio).ThenInclude(y => y.Remedio).FirstOrDefault(r => r.Id == recId));
            }
            return RedirectToAction(nameof(Index), new { id = id });
        }
    }
}
