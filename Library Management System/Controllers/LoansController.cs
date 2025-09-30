using FluentValidation;
using Library.Application.DTOs;
using Library.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController(
        LoanService loanService,
        IValidator<LoanCreateDto> loanCreateValidator,
        IValidator<LoanReturnDto> loanReturnValidator) : ControllerBase
    {
        private readonly LoanService _loanService = loanService;
        private readonly IValidator<LoanCreateDto> _loanCreateValidator = loanCreateValidator;
        private readonly IValidator<LoanReturnDto> _loanReturnValidator = loanReturnValidator;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _loanService.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
            => (await _loanService.GetAsync(id)) is { } loanReadDto ? Ok(loanReadDto) : NotFound();

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LoanCreateDto loanCreateDto)
        {
            var validationResult = await _loanCreateValidator.ValidateAsync(loanCreateDto);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            var createdLoan = await _loanService.CreateAsync(loanCreateDto);
            return CreatedAtAction(nameof(Get), new { id = createdLoan.Id }, createdLoan);
        }

        [HttpPut("{id:int}/return")]
        public async Task<IActionResult> Return(int id, [FromBody] LoanReturnDto loanReturnDto)
        {
            var validationResult = await _loanReturnValidator.ValidateAsync(loanReturnDto);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            return await _loanService.ReturnAsync(id, loanReturnDto) ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
            => await _loanService.DeleteAsync(id) ? NoContent() : NotFound();

    }
}
