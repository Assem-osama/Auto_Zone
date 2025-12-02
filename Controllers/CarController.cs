using System.Security.Claims;
using AutoZone.DTOs.Car;
using AutoZone.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AutoZone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        /// <summary>
        /// Get all cars
        /// </summary>
        /// <param name="parameters">Filtering and sorting parameters</param>
        /// <returns>A paged list of cars</returns>
        /// <response code="200">List of cars retrieved successfully</response>
        [SwaggerOperation(Summary = "Get all cars", Description = "Returns a paged list of cars with optional filtering and sorting.")]
        [SwaggerResponse(200, "List of cars retrieved successfully")]
        [HttpGet]
        public async Task<IActionResult> GetAllCars([FromQuery] CarQueryParameters parameters)
        {
            var response = await _carService.GetAllCarsAsync(parameters);
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        /// <summary>
        /// Get car by ID
        /// </summary>
        /// <param name="id">The car ID</param>
        /// <returns>The car details</returns>
        /// <response code="200">Car retrieved successfully</response>
        /// <response code="404">Car not found</response>
        [SwaggerOperation(Summary = "Get car by ID", Description = "Returns car details by ID.")]
        [SwaggerResponse(200, "Car retrieved successfully")]
        [SwaggerResponse(404, "Car not found")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarById(int id)
        {
            var response = await _carService.GetCarByIdAsync(id);
            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        /// <summary>
        /// Create a new car
        /// </summary>
        /// <param name="dto">The car creation details</param>
        /// <returns>The created car</returns>
        /// <response code="201">Car created successfully</response>
        /// <response code="400">Invalid input data</response>
        [SwaggerOperation(Summary = "Create a new car", Description = "Creates a new car record. Requires authentication.")]
        [SwaggerResponse(201, "Car created successfully")]
        [SwaggerResponse(400, "Invalid input data")]
        [Authorize] // المستخدم لازم يكون مسجل
        [HttpPost]
        public async Task<IActionResult> CreateCar([FromBody] CreateCarDTO dto)
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var response = await _carService.CreateCarAsync(dto, userId);
            if (!response.Success)
                return BadRequest(response);

            return CreatedAtAction(nameof(GetCarById), new { id = response.Data.Id }, response);
        }

        /// <summary>
        /// Update a car
        /// </summary>
        /// <param name="id">The car ID</param>
        /// <param name="dto">The car update details</param>
        /// <returns>No content</returns>
        /// <response code="204">Car updated successfully</response>
        /// <response code="404">Car not found</response>
        [SwaggerOperation(Summary = "Update a car", Description = "Updates car details. Requires authentication.")]
        [SwaggerResponse(204, "Car updated successfully")]
        [SwaggerResponse(404, "Car not found")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCar(int id, [FromBody] UpdateCarDTO dto)
        {
            var response = await _carService.UpdateCarAsync(id, dto);
            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }


        /// <summary>
        /// Delete a car
        /// </summary>
        /// <param name="id">The car ID</param>
        /// <returns>No content</returns>
        /// <response code="204">Car deleted successfully</response>
        /// <response code="404">Car not found</response>
        [SwaggerOperation(Summary = "Delete a car", Description = "Deletes a car. Requires authentication.")]
        [SwaggerResponse(204, "Car deleted successfully")]
        [SwaggerResponse(404, "Car not found")]
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var response = await _carService.DeleteCarAsync(id);
            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }
    }
}
