using FluentValidation;
using LibraryInventoryAPI.Data;
using LibraryInventoryAPI.Models;
using LibraryInventoryAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryInventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext _context;
        private readonly IValidator<BookCreateDto> _createValidator;
        private readonly IValidator<BookUpdateDto> _updateValidator;

        public BooksController(LibraryContext context, IValidator<BookCreateDto> createValidator, IValidator<BookUpdateDto> updateValidator)
        {
            _context = context;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return await _context.Books.ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

    
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(BookCreateDto bookDto)
        {
     
            var validationResult = await _createValidator.ValidateAsync(bookDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var book = new Book
            {
                Title = bookDto.Title,
                Author = bookDto.Author,
                PublicationYear = bookDto.PublicationYear
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

      
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, BookUpdateDto bookDto)
        {
            
            var validationResult = await _updateValidator.ValidateAsync(bookDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            book.Title = bookDto.Title;
            book.Author = bookDto.Author;
            book.PublicationYear = bookDto.PublicationYear;

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!_context.Books.Any(e => e.Id == id))
            {
                return NotFound();
            }

            return NoContent();
        }

   
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
