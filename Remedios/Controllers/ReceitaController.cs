using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Remedios.Data;
using Remedios.Models;
using Remedios.Services;
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
                        //receita.Temporario = model.Temporario;
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
        public IActionResult Edit(long? id, long? recId)
        {
            if (id != null && recId != null)
            {
                ViewData["Id"] = id;
                var remedios = CarregarRemedios(id);
                var usados = _context.Receitas.Include(x => x.UsuarioRemedio).ThenInclude(y => y.Remedio)
                    .Where(r => r.Id == recId).AsNoTracking()
                    .Select(x => new CreateReceitaViewModel
                    {
                        Id = x.Id,
                        Diagnostico = x.Diagnostico,
                        Instrucao = x.Instrucao,
                        Medico = x.Medico,
                        //Temporario = x.Temporario,
                        Remedios = x.UsuarioRemedio
                        .Select(y => new RemediosASelecionar
                        {
                            Id = y.Remedio.Id,
                            Nome = y.Remedio.Nome,
                            Selecionado = true
                        }).ToArray()
                    }).FirstOrDefault();

                usados.Remedios = usados.Remedios.Union(remedios).ToArray();
                return View(usados);
            }
            return RedirectToAction(nameof(Index), new { id = id });
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CreateReceitaViewModel model, long? UserId)
        {
            if (ModelState.IsValid && UserId != null)
            {
                //Para evitar problemas de tracking
                var receitas = await _context.Receitas.Include(x => x.UsuarioRemedio).ToListAsync();
                var vinculos = await _context.MembroRemedios.ToListAsync();

                var rec = receitas.FirstOrDefault(x => x.Id == model.Id);
                rec.Id = model.Id;
                rec.Diagnostico = model.Diagnostico;
                rec.Instrucao = model.Instrucao;
                //rec.Temporario = model.Temporario;

                foreach(var item in model.Remedios)
                {
                    if (item.Selecionado)
                    {
                        if (_context.MembroRemedios.Where(x => x.RemedioId == item.Id && x.UserId == UserId).Count() == 0)
                        {
                            rec.UsuarioRemedio.Add(new MembroRemedio { RemedioId = item.Id, UserId = (long)UserId, DataInicio = DateTime.Now });
                        }
                    }
                    else
                    {
                        if(vinculos.Where(x => x.RemedioId == item.Id && x.UserId == UserId).Count() != 0)
                        {
                            _context.MembroRemedios.Remove(_context.MembroRemedios.Where(x => x.RemedioId == item.Id && x.UserId == UserId).FirstOrDefault());
                        }
                    }
                }
                _context.Receitas.UpdateRange(rec);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = UserId });
            }
            return View(model);
        }
        public IActionResult Delete(long? id, long? recId)
        {
            if(id != null && recId != null)
            {
                ViewData["Id"] = id;
                return View(_context.Receitas.Find(recId));
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Receita rec, long? UserId)
        {
            if(UserId != null)
            {
                ViewData["Id"] = UserId;
                if (ModelState.IsValid)
                {
                    rec = await _context.Receitas.Include(x => x.UsuarioRemedio).FirstOrDefaultAsync(x => x.Id == rec.Id);
                    _context.MembroRemedios.RemoveRange(rec.UsuarioRemedio);
                    _context.Receitas.Remove(rec);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { id = UserId });
                }
                else
                {
                    return View(rec);
                }
            }
            return NotFound();
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
