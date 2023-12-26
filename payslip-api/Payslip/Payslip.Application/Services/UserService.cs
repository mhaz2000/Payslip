using AutoMapper;
using Payslip.Application.DTOs;
using Payslip.Core.Repositories.Base;

namespace Payslip.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public (IEnumerable<UserDTO> Users, int Total) GetUsers(int skip)
        {
            var users = _unitOfWork.UserRepository.Where(c => c.IsActive && !string.IsNullOrEmpty(c.NationalCode)).OrderBy(c => c.CardNumber);
            return (_mapper.Map<IEnumerable<UserDTO>>(users.Skip(skip).Take(10)), users.AsEnumerable().Count());
        }
    }
}
