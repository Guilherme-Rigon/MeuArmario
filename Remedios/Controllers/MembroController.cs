using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Remedios.Data;
using Remedios.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Remedios.Controllers
{
    public class MembroController : Controller
    {
        private readonly RemediosDbContext _context;
        public MembroController(RemediosDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View(_context.MembrosFamilia.Include(x => x.Fotos).Include(r => r.Remedios).ToList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(IFormCollection form)
        {
            return View(_context.MembrosFamilia.Include(x => x.Fotos).Include(r => r.Remedios).Where(x => x.Nome.Contains(form["nome"]) || x.Sobrenome.Contains(form["nome"])).ToList());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(MembroFamilia user, IList<IFormFile> file)
        {
            IFormFile img = file.FirstOrDefault();
            MemoryStream ms = new MemoryStream();
            if (ModelState.IsValid)
            {
                if (img != null && img.ContentType.StartsWith("image/"))
                {
                    img.OpenReadStream().CopyTo(ms);
                }
                user.Fotos = new Foto[] { new Foto { foto = ms.ToArray(), ContentType = img.ContentType } };
                _context.MembrosFamilia.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
        public IActionResult Edit(long? id)
        {
            return View(_context.MembrosFamilia.Find(id));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(MembroFamilia user, IList<IFormFile> file)
        {
            if (ModelState.IsValid)
            {
                _context.MembrosFamilia.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
        public IActionResult Delete(long? id)
        {
            return View(_context.MembrosFamilia.Find(id));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(MembroFamilia user)
        {
            try
            {
                _context.Fotos.RemoveRange(_context.Fotos.Where(x => x.Membro.Id == user.Id));
                _context.MembrosFamilia.Remove(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Delete), user.Id);
            }
        }

        [HttpGet]
        public FileStreamResult Img(long id)
        {
            Foto imagem = _context.Fotos.FirstOrDefault(m => m.Id == id);
            MemoryStream ms = new MemoryStream(imagem.foto);
            return new FileStreamResult(ms, imagem.ContentType);
        }
    }
}
