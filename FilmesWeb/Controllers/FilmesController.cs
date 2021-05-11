using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entidades.Model;
using Entidades.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Negocio;
using Persistencia.Repositorio;

namespace FilmesWeb.Controllers
{

    [Authorize]
    public class FilmesController : Controller
    {
        public readonly AdmFacade _negocio;

        public readonly UserManager<ApplicationUser> _userManager;

        private readonly MovieContext _context;


        private IWebHostEnvironment _environment;

        public FilmesController(AdmFacade negocio,
                                UserManager<ApplicationUser> userManager,
                                IWebHostEnvironment environment)
        {
            _context = new MovieContext();
            _negocio = negocio;
            _userManager = userManager;
            _environment = environment;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            List<Movie> filmes = _negocio.TodosFilmes();
            return View(filmes);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,ReleaseDate,Genre,Price")] Movie movie)
        {
            if (id != movie.MovieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.MovieId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        [AllowAnonymous]
        public IActionResult roteiroAutenticacao()
        {
            return View();
        }


        public IActionResult relFilmes()
        {
            List<RelFilmes> consolidado = _negocio.relatorioFilmes();

            return View(consolidado);

        }

        public async Task<IActionResult> dadosUsuario()
        {
            var usuario = await _userManager.GetUserAsync(User);

            ViewBag.Id = usuario.Id;
            ViewBag.UserName = usuario.UserName;

            return View();

        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.MovieId == id);
        }
    }
}