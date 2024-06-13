using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BibliothequeCSWebAPI.Models;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Data.Common;

namespace BibliothequeWebAPI.Controllers
{
    public class AuthorController : Controller
    {
        private readonly ILogger<AuthorController> _logger;
        private readonly BiblioContext DB;

        public AuthorController(ILogger<AuthorController> logger, BiblioContext context)
        {
            _logger = logger;
            DB = context;
        }

        [HttpGet("Authors")]
        public IActionResult Index()
        {
            List<Auteur> authors = DB.Auteurs.ToList();
            return Json(authors);
        }

        [HttpPost("Authors")]
       
        public IActionResult Add([FromBody] Auteur authorToAdd)
        {
            Auteur newAuthor = new Auteur
            {
                //IdAuteur = _context.Auteurs.Count() + 1,
                NomAuteur = authorToAdd.NomAuteur,
                PrenomAuteur = authorToAdd.PrenomAuteur
            };
            DB.Auteurs.Add(newAuthor);
            DB.SaveChanges();
            return Json(newAuthor);
        }
        [HttpGet("Authors/{id}")] 
        public IActionResult Index(int id)
        {
            Auteur author = DB.Auteurs.Find(id);
            return Json(author);
        }
        
        [HttpDelete("Authors/{id}")]
        public IActionResult DeleteAuthor(int id)
        {
            Auteur authorToDelete = DB.Auteurs.Find(id);
            DB.Auteurs.Remove(authorToDelete);
            DB.SaveChanges();
            return Json (authorToDelete);
        }

        [HttpPut("Authors/{id}")]
        public IActionResult UpdateAuthor(int id, [FromBody] Auteur updatedAuthor)
        {
            Auteur authorToUpdate = DB.Auteurs.Find(id);
            authorToUpdate.NomAuteur = updatedAuthor.NomAuteur;
            authorToUpdate.PrenomAuteur = updatedAuthor.PrenomAuteur;
            DB.SaveChanges();
            return Json(authorToUpdate);
        }
    }
}

