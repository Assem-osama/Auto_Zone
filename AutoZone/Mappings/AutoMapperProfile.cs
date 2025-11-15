using AutoMapper;
using AutoZone.DTOs;
using AutoZone.DTOs.Car;
using AutoZone.DTOs.Rental;
using AutoZone.Models;

namespace AutoZone.Mappings
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile() {
            CreateMap<Car,CarDTO>().ReverseMap();
            CreateMap<CreateCarDTO, Car>()
                .ForMember(dest => dest.Owner, opt => opt.Ignore());
            CreateMap<UpdateCarDTO, Car>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());



            // Map RegisterDto -> User
            CreateMap<RegisterDTO, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // عشان الـ Id يتولد تلقائي
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); // هنحط الـ Hash يدوي بعدين




            CreateMap<CreateRentalDTO, Rental>().ReverseMap();
            CreateMap<UpdateRentalDTO, Rental>().ReverseMap();

            CreateMap<Rental, RentalDTO>().ReverseMap();


        }

    }
}
