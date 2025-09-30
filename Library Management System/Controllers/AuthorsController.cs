using FluentValidation;
using Library.Application.DTOs;
using Library.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController(
        AuthorService authorService,
        IValidator<AuthorCreateDto> authorCreateValidator,
        IValidator<AuthorUpdateDto> authorUpdateValidator) : ControllerBase
    {
        private readonly AuthorService _authorService = authorService;
        private readonly IValidator<AuthorCreateDto> _authorCreateValidator = authorCreateValidator;
        private readonly IValidator<AuthorUpdateDto> _authorUpdateValidator = authorUpdateValidator;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _authorService.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
            => (await _authorService.GetAsync(id)) is { } authorReadDto ? Ok(authorReadDto) : NotFound();

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AuthorCreateDto authorCreateDto)
        {
            var validationResult = await _authorCreateValidator.ValidateAsync(authorCreateDto);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            var createdAuthor = await _authorService.CreateAsync(authorCreateDto);
            return CreatedAtAction(nameof(Get), new { id = createdAuthor.Id }, createdAuthor);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] AuthorUpdateDto authorUpdateDto)
        {
            var validationResult = await _authorUpdateValidator.ValidateAsync(authorUpdateDto);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            return await _authorService.UpdateAsync(id, authorUpdateDto) ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
            => await _authorService.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
