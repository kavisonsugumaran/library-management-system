using FluentValidation;
using Library.Application.DTOs;
using Library.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private readonly LoanService _service;
        private readonly IValidator<LoanCreateDto> _createValidator;
        private readonly IValidator<LoanReturnDto> _returnValidator;

        public LoansController(LoanService service, IValidator<LoanCreateDto> createValidator, IValidator<LoanReturnDto> returnValidator)
        {
            _service = service;
            _createValidator = createValidator;
            _returnValidator = returnValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
            => (await _service.GetAsync(id)) is { } dto ? Ok(dto) : NotFound();

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LoanCreateDto dto)
        {
            var v = await _createValidator.ValidateAsync(dto);
            if (!v.IsValid) return BadRequest(v.Errors);
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPost("{id:int}/return")]
        public async Task<IActionResult> Return(int id, [FromBody] LoanReturnDto dto)
        {
            var v = await _returnValidator.ValidateAsync(dto);
            if (!v.IsValid) return BadRequest(v.Errors);
            return await _service.ReturnAsync(id, dto) ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
            => await _service.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
