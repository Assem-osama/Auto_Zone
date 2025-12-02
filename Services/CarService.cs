using AutoMapper;
using AutoZone.DTOs;
using AutoZone.DTOs.Car;
using AutoZone.Models;
using AutoZone.Services.Interfaces;
using AutoZone.UnitOfWorks;

namespace AutoZone.Services
{
    public class CarService : ICarService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CarService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<PagedResult<CarDTO>>> GetAllCarsAsync(CarQueryParameters parameters)
        {
            var pagedCars = await _unitOfWork.Cars.GetCarsAsync(parameters);
            var carDtos = _mapper.Map<IEnumerable<CarDTO>>(pagedCars.Items);
            
            var result = new PagedResult<CarDTO>(
                carDtos, 
                pagedCars.TotalCount, 
                pagedCars.PageNumber, 
                pagedCars.PageSize
            );

            return ServiceResponse<PagedResult<CarDTO>>.SuccessResponse(result);
        }

        public async Task<ServiceResponse<CarDTO>> GetCarByIdAsync(int id)
        {
            var car = await _unitOfWork.Cars.GetByIdAsync(id);
            if (car == null)
                return ServiceResponse<CarDTO>.FailureResponse("Car not found");

            var result = _mapper.Map<CarDTO>(car);
            return ServiceResponse<CarDTO>.SuccessResponse(result);
        }

        public async Task<ServiceResponse<CarDTO>> CreateCarAsync(CreateCarDTO dto, int userId)
        {
            Car car = _mapper.Map<Car>(dto);
            car.UserId = userId;

            await _unitOfWork.Cars.AddAsync(car);
            await _unitOfWork.SaveAsync();

            var result = _mapper.Map<CarDTO>(car);
            return ServiceResponse<CarDTO>.SuccessResponse(result, "Car created successfully ✅");
        }

        public async Task<ServiceResponse<string>> UpdateCarAsync(int id, UpdateCarDTO dto)
        {
            var car = await _unitOfWork.Cars.GetByIdAsync(id);
            if (car == null)
                return ServiceResponse<string>.FailureResponse("Car not found");

            _mapper.Map(dto, car);
            _unitOfWork.Cars.Update(car);
            await _unitOfWork.SaveAsync();

            return ServiceResponse<string>.SuccessResponse(null, "Car updated successfully ✅");
        }

        public async Task<ServiceResponse<string>> DeleteCarAsync(int id)
        {
            var car = await _unitOfWork.Cars.GetByIdAsync(id);
            if (car == null)
                return ServiceResponse<string>.FailureResponse("Car not found");

            _unitOfWork.Cars.Delete(car);
            await _unitOfWork.SaveAsync();

            return ServiceResponse<string>.SuccessResponse(null, "Car deleted successfully ✅");
        }
    }
}
