using FluentValidation;
using Library.Application.DTOs;
using Library.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly AuthorService _service;
        private readonly IValidator<AuthorCreateDto> _createValidator;
        private readonly IValidator<AuthorUpdateDto> _updateValidator;

        public AuthorsController(AuthorService service, IValidator<AuthorCreateDto> createValidator, IValidator<AuthorUpdateDto> updateValidator)
        {
            _service = service;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
            => (await _service.GetAsync(id)) is { } dto ? Ok(dto) : NotFound();

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AuthorCreateDto dto)
        {
            var v = await _createValidator.ValidateAsync(dto);
            if (!v.IsValid) return BadRequest(v.Errors);
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] AuthorUpdateDto dto)
        {
            var v = await _updateValidator.ValidateAsync(dto);
            if (!v.IsValid) return BadRequest(v.Errors);
            return await _service.UpdateAsync(id, dto) ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
            => await _service.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
