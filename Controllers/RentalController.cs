using System.Security.Claims;
using AutoZone.DTOs.Rental;
using AutoZone.Services;
using AutoZone.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoZone.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RentalController : ControllerBase
    {
        private readonly IRentalService _rentalService;

        public RentalController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllRentals()
        {
            var response = await _rentalService.GetAllRentalsAsync();
            return Ok(response);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult>GetRentalById(int id)
        {
            var response = await _rentalService.GetRentalByIdAsync(id);
            return response.Success ? Ok(response) : NotFound(response);

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateRental([FromBody] CreateRentalDTO rentalDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var response=await _rentalService.CreateRentalAsync(rentalDTO,userId);
            return CreatedAtAction(nameof(GetRentalById), new { id = response.Data.Id }, response);

        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult>UpdateRental(int id,[FromBody]UpdateRentalDTO rentalDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var response = await _rentalService.UpdateRentalAsync(id, rentalDTO, userId);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRental(int id)
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var response = await _rentalService.DeleteRentalAsync(id,userId);

            return response.Success ? Ok(response) : BadRequest(response);
        }

    }
}
