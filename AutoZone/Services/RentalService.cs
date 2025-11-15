using AutoMapper;
using AutoZone.DTOs.Car;
using AutoZone.DTOs.Rental;
using AutoZone.Models;
using AutoZone.Services.Interfaces;
using AutoZone.UnitOfWorks;

namespace AutoZone.Services
{
    public class RentalService: IRentalService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RentalService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<IEnumerable<RentalDTO>>> GetAllRentalsAsync()
        {
            var rentals = await _unitOfWork.Rentals.GetAllAsync();
            var dtoList = _mapper.Map<IEnumerable<RentalDTO>>(rentals);
            return ServiceResponse<IEnumerable<RentalDTO>>.SuccessResponse(dtoList);
        }

        public async Task<ServiceResponse<RentalDTO>> GetRentalByIdAsync(int id)
        {
            var rental=await _unitOfWork.Rentals.GetByIdAsync(id);
            if (rental == null)
            {
                return ServiceResponse<RentalDTO>.FailureResponse("rental not found");
            }

             var result= _mapper.Map<RentalDTO>(rental);

            return ServiceResponse<RentalDTO>.SuccessResponse(result);

        }

        public async Task<ServiceResponse<RentalDTO>> CreateRentalAsync(CreateRentalDTO dto, int userId)
        {
            // هات العربية اللي هيستأجرها المستخدم عشان نعرف سعر الايجار بتاعها واحنا بنحسب ال Total Price
            var car = await _unitOfWork.Cars.GetByIdAsync(dto.CarId);
            if (car == null)
            {
                return ServiceResponse<RentalDTO>.FailureResponse("Car not found");

            }

            Rental rental= _mapper.Map<Rental>(dto);
            rental.RenterId = userId;
            

            // احسب عدد الأيام
            int days = (int)Math.Ceiling((dto.EndDate.Date - dto.StartDate.Date).TotalDays);
            if (days <= 0) days = 1; // لو نفس اليوم، اعتبرها يوم واحد

            rental.TotalPrice = (car.PricePerDay ?? 0) * days;



            await _unitOfWork.Rentals.AddAsync(rental);
            await _unitOfWork.SaveAsync();

            Console.WriteLine($"✅ Rental created. Total price: {rental.TotalPrice}");
                var result = _mapper.Map<RentalDTO>(rental);

            return ServiceResponse<RentalDTO>.SuccessResponse(result, "Rental created successfully ✅"); 
        }
        public async Task<ServiceResponse<string>> UpdateRentalAsync(int id, UpdateRentalDTO dto, int userId)
        {
            var rental =await _unitOfWork.Rentals.GetByIdAsync(id);
            if (rental == null)
            {
                return ServiceResponse<string>.FailureResponse(" rental not found");
            }

            if (rental.RenterId != userId)// تأكيد إن المستخدم صاحب الـ rental هو اللي بيعدّل أو بيحذف
                return ServiceResponse<string>.FailureResponse("Unauthorized to update this rental.");

            //_mapper.Map(dto,rental);
            rental.CarId = dto.CarId;
            rental.StartDate = dto.StartDate;
            rental.EndDate = dto.EndDate;
            

            // هات العربية علشان نحسب السعر الجديد
            var car = await _unitOfWork.Cars.GetByIdAsync(rental.CarId);
            if (car == null)
                return ServiceResponse<string>.FailureResponse("Car not found.");
            
            int days = (int)Math.Ceiling((dto.EndDate.Date - dto.StartDate.Date).TotalDays);
            if (days <= 0) days = 1;
            rental.TotalPrice = (car.PricePerDay ?? 0) * days;



            _unitOfWork.Rentals.Update(rental);
            await _unitOfWork.SaveAsync();

            return ServiceResponse<string>.SuccessResponse(null, "Rental updated successfully ✅");
        }
        public async Task<ServiceResponse<string>> DeleteRentalAsync(int id, int userId)
        {
            var rental = await _unitOfWork.Rentals.GetByIdAsync(id);

            if (rental == null)
                return ServiceResponse<string>.FailureResponse("Rental not found.");

            if (rental.RenterId != userId)
                return ServiceResponse<string>.FailureResponse("Unauthorized to delete this rental.");

            _unitOfWork.Rentals.Delete(rental);
            await _unitOfWork.SaveAsync();

            return ServiceResponse<string>.SuccessResponse(null, "Rental deleted successfully ✅");
        }


    }
}
