using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using BibliothequeCSWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
            Auteur.CreateInDB("Test2", "John", DB);
            List<Auteur> authors = DB.Auteurs.ToList();
            return Json(authors);
        }

        [HttpPost("Authors")]
        public IActionResult Add([FromBody] Auteur authorToAdd)
        {
            Auteur newAuthor = new Auteur
            {
                NomAuteur = authorToAdd.NomAuteur,
                PrenomAuteur = authorToAdd.PrenomAuteur,
                Livres = authorToAdd.Livres
            };

            foreach (Livre livre in newAuthor.Livres)
            {
                livre.ResumerLivre ??= "default";
            }

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
            return Json(authorToDelete);
        }

        [HttpPut("Authors/{id}")]
        public IActionResult UpdateAuthor(int id, [FromBody] Auteur updatedAuthor)
        {
            Auteur authorToUpdate = DB.Auteurs.Find(id);

            if (authorToUpdate == null)
            {
                Auteur newAuthor = new Auteur
                {
                    IdAuteur = id,
                    NomAuteur = updatedAuthor.NomAuteur,
                    PrenomAuteur = updatedAuthor.PrenomAuteur,
                    Livres = updatedAuthor.Livres
                };

                foreach (Livre livre in newAuthor.Livres)
                {
                    livre.ResumerLivre ??= "default";
                    livre.IdAuteur = newAuthor.IdAuteur;
                }

                DB.Auteurs.Add(newAuthor);
                DB.SaveChanges();
                return Json(newAuthor);
            }
            else
            {
                authorToUpdate.NomAuteur = updatedAuthor.NomAuteur;
                authorToUpdate.PrenomAuteur = updatedAuthor.PrenomAuteur;

                if (updatedAuthor.Livres != null)
                {
                    authorToUpdate.Livres = updatedAuthor.Livres;

                    foreach (Livre livre in authorToUpdate.Livres)
                    {
                        livre.ResumerLivre ??= "default";
                        livre.IdAuteur = authorToUpdate.IdAuteur;
                    }
                }
            }

            DB.SaveChanges();
            return Json(authorToUpdate);
        }

        [HttpPut("Authors/{id}/Books")]
        public IActionResult UpdateBooksForAuthor(int id, [FromBody] List<Livre> booksToAdd)
        {
            // Find the existing author by id
            Auteur author = DB.Auteurs.Find(id);
            if (author == null)
            {
                return NotFound(new { Message = "Author not found" });
            }

            foreach (Livre newBook in booksToAdd)
            {
                bool bookExists = author.Livres.Any(l => l.IdLivre == newBook.IdLivre);

                if (!bookExists)
                {
                    newBook.ResumerLivre ??= "default";
                    newBook.IdAuteur = author.IdAuteur;
                    author.Livres.Add(newBook);
                }
                else
                {
                    Livre existingBook = author.Livres.First(l => l.IdLivre == newBook.IdLivre);
                    existingBook.TitreLivre = newBook.TitreLivre;
                    existingBook.ResumerLivre = newBook.ResumerLivre;
                }
            }

            DB.SaveChanges();
            return Json(author);
        }
    }
}
