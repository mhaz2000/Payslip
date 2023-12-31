using AutoMapper;
using Payslip.Application.Commands;
using Payslip.Application.DTOs;
using Payslip.Core.Entities;
using Payslip.Core.Repositories.Base;

namespace Payslip.Application.Base
{
    public class ModelProfile : Profile
    {
        public ModelProfile() 
        {
            CreateMap<PayslipCommand, UserPayslip>()
                .ForMember(c=> c.User, opt => opt.MapFrom<UserResolver>());

            CreateMap<User, UserDTO>();
            CreateMap<UserPayslip, UserPayslipDTO>();
        }
    }


    public class UserResolver : IValueResolver<PayslipCommand, UserPayslip, User?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserResolver(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public User? Resolve(PayslipCommand source, UserPayslip destination, User destMember, ResolutionContext context)
        {
            return _unitOfWork.UserRepository.GetUserByCardNumber(source.CardNumber);
        }
    }
}
