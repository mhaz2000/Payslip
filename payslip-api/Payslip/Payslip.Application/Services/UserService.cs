using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Payslip.Application.Base;
using Payslip.Application.Commands;
using Payslip.Application.DTOs;
using Payslip.Core.Entities;
using Payslip.Core.Repositories.Base;

namespace Payslip.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task CreateUser(UserCreateCommand command)
        {
            var checkNattionalCodeDuplication = await _unitOfWork.UserRepository.AnyAsync(c => c.IsActive && c.NationalCode == command.NationalCode);
            if (checkNattionalCodeDuplication)
                throw new ManagedException("کد ملی تکراری است.");

            var checkCardNumberDuplication = await _unitOfWork.UserRepository.AnyAsync(c => c.IsActive && c.CardNumber == command.CardNumber);
            if (checkCardNumberDuplication)
                throw new ManagedException("شماره کارت تکراری است.");

            var newUser = new User(command.NationalCode, command.LastName, command.FirstName, command.NationalCode, command.CardNumber);
            await _userManager.CreateAsync(newUser, command.NationalCode);
            await _userManager.AddToRoleAsync(newUser, "User");

            await _unitOfWork.CommitAsync();
        }

        public (IEnumerable<UserDTO> Users, int Total) GetUsers(int skip, string search)
        {
            var users = _unitOfWork.UserRepository.Where(c => !string.IsNullOrEmpty(c.NationalCode))
                .Where(c => (c.FirstName + c.LastName + c.NationalCode + c.CardNumber).Contains(search) || (c.FirstName + " " + c.LastName).Contains(search))
                .OrderBy(c => c.CardNumber).ToList();

            return (_mapper.Map<IEnumerable<UserDTO>>(users.Skip(skip).Take(10)), users.AsEnumerable().Count());
        }

        public async Task RemoveUser(Guid userId)
        {
            var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(c => c.Id == userId);
            if (user is null)
                throw new ManagedException("کاربر مورد نظر یافت نشد.");

            await _userManager.DeleteAsync(user);

            await _unitOfWork.CommitAsync();
        }

        public async Task ToggleActivation(Guid userId)
        {
            var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(c => c.Id == userId);
            if (user is null)
                throw new ManagedException("کاربر مورد نظر یافت نشد.");

            user.IsActive = !user.IsActive;

            await _unitOfWork.CommitAsync();
        }
    }
}
