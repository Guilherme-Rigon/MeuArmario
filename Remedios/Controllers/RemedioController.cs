using Microsoft.AspNetCore.Http;
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
    public class RemedioController : Controller
    {
        private readonly RemediosDbContext _context;
        public RemedioController(RemediosDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View(_context.Remedios.ToList());
        }
        public IActionResult DosesPDia(long? id)
        {
            ViewData["Id"] = id;
            return View();
        }
        [HttpPost]
        public IActionResult DosesPDia(long? id, DateTime? dateTime)
        {
            ViewData["Id"] = id;
            if (id != null && dateTime != null)
            {
                var rem = _context.Remedios.Where(x => x.Usuarios.Where(y => y.UserId == id).Count() > 0).ToList();
                var usados = _context.Remedios
                        .Where(x => x.Usuarios
                        .Where(y => y.UserId == id && y.Doses
                        .Where(z => z.DataUso.Date == ((DateTime)dateTime).Date).Count() > 0).Count() > 0)
                        .Select(x => new RemediosASelecionar
                        {
                            Id = x.Id,
                            Nome = x.Nome,
                            Preco = x.Preco,
                            Quantidade = x.Quantidade,
                            Tarja = x.Tarja,
                            Validade = x.Validade,
                            Selecionado = true
                        }).AsEnumerable();
                var remedios = rem.Except(usados, new RemedioComparer()).Select(x => new RemediosASelecionar
                {
                    Id = x.Id,
                    Nome = x.Nome,
                    Preco = x.Preco,
                    Quantidade = x.Quantidade,
                    Tarja = x.Tarja,
                    Validade = x.Validade,
                    Selecionado = false
                }).AsEnumerable().Concat(usados);
                return View(remedios.OrderBy(x => x.Nome));
            }
            return View(null);
        }
        [HttpGet]
        public IActionResult MinhasDoses(long? id)
        {
            if(id != null)
            {
                ViewData["id"] = id;
                return View(_context.Remedios
                    .Where(x => x.Usuarios.Where(y => y.UserId == id && y.Doses.Where(z => z.DataUso.Date == DateTime.Now.Date).Count() == 0).Count() > 0)
                    .Select(x => new RemediosASelecionar
                    {
                        Id = x.Id,
                        Nome = x.Nome,
                        Preco = x.Preco,
                        Quantidade = x.Quantidade,
                        Tarja = x.Tarja,
                        Validade = x.Validade,
                        Selecionado = false
                    }).ToArray());
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> MarcarDose(List<RemediosASelecionar> model, long? id)
        {
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    foreach (var item in model)
                    {
                        if (item.Selecionado)
                        {
                            var med = _context.Remedios.Find(item.Id);
                            med.Quantidade -= 1;
                            _context.Remedios.Update(med);
                            _context.Doses.Add(
                                new Dose
                                {
                                    DataUso = DateTime.Now.Date,
                                    MembroRemedio = _context.MembroRemedios.FirstOrDefault(x => x.RemedioId == item.Id && x.UserId == id)
                                }
                                );
                        }
                    }
                    await _context.SaveChangesAsync();
                }
            }
            ViewData["id"] = id;
            return RedirectToAction(nameof(MinhasDoses), new { id = id });
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Remedio model)
        {
            if (ModelState.IsValid)
            {
                _context.Remedios.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Edit(long? id)
        {
            return View(_context.Remedios.FirstOrDefault(x => x.Id == id));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Remedio model)
        {
            if (ModelState.IsValid)
            {
                _context.Remedios.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Delete(long? id)
        {
            return View(_context.Remedios.FirstOrDefault(x => x.Id == id));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Remedio model)
        {
            try
            {
                _context.Remedios.Remove(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Delete), new { id = model.Id });
            }
        }
    }
}
