using System.Security.Claims;
using AutoZone.DTOs.Rental;
using AutoZone.Services;
using AutoZone.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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

        [SwaggerOperation(Summary = "Get all rentals", Description = "Returns a list of all rentals.")]
        [SwaggerResponse(200, "Rentals retrieved successfully")]
        [HttpGet]
        public async Task<IActionResult> GetAllRentals()
        {
            var response = await _rentalService.GetAllRentalsAsync();
            return Ok(response);
        }

        [SwaggerOperation(Summary = "Get rental by ID", Description = "Returns rental details by ID.")]
        [SwaggerResponse(200, "Rental retrieved successfully")]
        [SwaggerResponse(404, "Rental not found")]
        [HttpGet("{id}")]
        public async Task<IActionResult>GetRentalById(int id)
        {
            var response = await _rentalService.GetRentalByIdAsync(id);
            return response.Success ? Ok(response) : NotFound(response);

        }

        [SwaggerOperation(Summary = "Create a rental", Description = "Creates a new rental. Requires authentication.")]
        [SwaggerResponse(201, "Rental created successfully")]
        [SwaggerResponse(400, "Invalid input data")]
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

        [SwaggerOperation(Summary = "Update a rental", Description = "Updates rental details. Requires authentication.")]
        [SwaggerResponse(200, "Rental updated successfully")]
        [SwaggerResponse(400, "Invalid input data")]
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

        [SwaggerOperation(Summary = "Delete a rental", Description = "Deletes a rental. Requires authentication.")]
        [SwaggerResponse(200, "Rental deleted successfully")]
        [SwaggerResponse(400, "Could not delete rental")]
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
