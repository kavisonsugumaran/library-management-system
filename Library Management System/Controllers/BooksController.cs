using FluentValidation;
using Library.Application.DTOs;
using Library.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController(
        BookService bookService,
        IValidator<BookCreateDto> bookCreateValidator,
        IValidator<BookUpdateDto> bookUpdateValidator) : ControllerBase
    {
        private readonly BookService _bookService = bookService;
        private readonly IValidator<BookCreateDto> _bookCreateValidator = bookCreateValidator;
        private readonly IValidator<BookUpdateDto> _bookUpdateValidator = bookUpdateValidator;

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search = null)
            => Ok(await _bookService.GetAllAsync(search));

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
            => (await _bookService.GetAsync(id)) is { } bookReadDto ? Ok(bookReadDto) : NotFound();

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookCreateDto bookCreateDto)
        {
            var validationResult = await _bookCreateValidator.ValidateAsync(bookCreateDto);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            var createdBook = await _bookService.CreateAsync(bookCreateDto);
            return CreatedAtAction(nameof(Get), new { id = createdBook.Id }, createdBook);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] BookUpdateDto bookUpdateDto)
        {
            var validationResult = await _bookUpdateValidator.ValidateAsync(bookUpdateDto);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            return await _bookService.UpdateAsync(id, bookUpdateDto) ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
            => await _bookService.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
