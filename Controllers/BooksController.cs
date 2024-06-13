using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BibliothequeCSWebAPI.Models;
namespace BibliothequeWebAPI.Controllers
{
    [ApiController]
    public class BooksController : Controller
    {
        public static List<Book> Library = new List<Book>
        {
            new Book 
            {
                Id = 1, 
                Title = "Book One", 
                Author = "Author One", 
                Name = "First Book"
            },
            new Book 
            {
                Id = 2, 
                Title = "Book Two", 
                Author = "Author Two",
                Name = "Second Book"
            }
        };

        [HttpGet("Books")]
        public IActionResult Index()
        {
            return Json(Library);
        }

        [HttpGet("Books/{id}")]
        public IActionResult GetBookById(int id)
        {
            Book? bookToReturn = Library.Find(book => book.Id == id);

            if (bookToReturn == null)
            {
                return NotFound();
            }

            return Ok(bookToReturn);
        }

        [HttpPost("Books")]
        public IActionResult AddBook([FromBody] Book newBook)
        {
            if (newBook == null || string.IsNullOrWhiteSpace(newBook.Title) || string.IsNullOrWhiteSpace(newBook.Author) || string.IsNullOrWhiteSpace(newBook.Name))
            {
                return BadRequest("Invalid book data.");
            }

            newBook.Id = Library.Count + 1;
            Library.Add(newBook);
            return CreatedAtAction(nameof(GetBookById), new { id = newBook.Id }, newBook);
        }

        [HttpDelete("Books/{id}")]
        public IActionResult DeleteBook(int id)
        {
            Book? bookToDelete = Library.Find(book => book.Id == id);
            if (bookToDelete == null)
            {
                return NotFound();
            }

            Library.Remove(bookToDelete);
            return Ok();
        }

        [HttpPut("Books/{id}")]
        public IActionResult UpdateBook(int id, [FromBody] Book updatedBook)
        {
            if (updatedBook == null || string.IsNullOrWhiteSpace(updatedBook.Title) || string.IsNullOrWhiteSpace(updatedBook.Author) || string.IsNullOrWhiteSpace(updatedBook.Name))
            {
                return BadRequest("Invalid book data.");
            }

            Book? bookToUpdate = Library.Find(book => book.Id == id);
            if (bookToUpdate == null)
            {
                return Json(new { Error = "Book not found" });
            }

            bookToUpdate.Title = updatedBook.Title;
            bookToUpdate.Author = updatedBook.Author;
            bookToUpdate.Name = updatedBook.Name;

            return Ok(bookToUpdate);
        }

        [HttpGet("Books/Search")]
        public IActionResult Search(string name)
        {
            Book? bookToSearch = Library.Find(book => book.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase));
            if (bookToSearch == null)
            {
                return NotFound();
            }

            return Ok(bookToSearch);
        }
    }
}
