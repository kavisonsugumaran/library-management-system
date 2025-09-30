using FluentValidation;
using Library.Application.DTOs;
using Library.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController(
        MemberService memberService,
        IValidator<MemberCreateDto> memberCreateValidator,
        IValidator<MemberUpdateDto> memberUpdateValidator) : ControllerBase
    {
        private readonly MemberService _memberService = memberService;
        private readonly IValidator<MemberCreateDto> _memberCreateValidator = memberCreateValidator;
        private readonly IValidator<MemberUpdateDto> _memberUpdateValidator = memberUpdateValidator;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _memberService.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
            => (await _memberService.GetAsync(id)) is { } memberReadDto ? Ok(memberReadDto) : NotFound();

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MemberCreateDto memberCreateDto)
        {
            var validationResult = await _memberCreateValidator.ValidateAsync(memberCreateDto);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            var createdMember = await _memberService.CreateAsync(memberCreateDto);
            return CreatedAtAction(nameof(Get), new { id = createdMember.Id }, createdMember);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] MemberUpdateDto memberUpdateDto)
        {
            var validationResult = await _memberUpdateValidator.ValidateAsync(memberUpdateDto);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            return await _memberService.UpdateAsync(id, memberUpdateDto) ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
            => await _memberService.DeleteAsync(id) ? NoContent() : NotFound();

    }
}
