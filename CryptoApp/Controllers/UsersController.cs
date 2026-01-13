using AutoMapper;
using CryptoApp.DTOs;
using CryptoApp.Entities;
using CryptoApp.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CryptoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public UsersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Registers a new user account with the provided registration details.
        /// </summary>
        /// <param name="userDto">An object containing the user's registration information. Must include a valid and unique email address.</param>
        /// <returns>A status code</returns>
        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync(UserRegisterDto userDto)
        {
            if (await _unitOfWork.UserRepository.EmailExistsAsync(userDto.Email))
                return Conflict("Email already exists.");

            await _unitOfWork.UserRepository.CreateAsync(_mapper.Map<User>(userDto));
            await _unitOfWork.SaveAsync();
            return Created();
        }

        /// <summary>
        /// Retrieves the user with the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to retrieve.</param>
        /// <returns>A userDto containing the user data if found; otherwise, a 404 Not Found response.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserByIdAsync(int id)
        {
            User result = await _unitOfWork.UserRepository.GetUserAsync(id);

            if (result == null)
                return NotFound();

            return Ok(_mapper.Map<UserDto>(result));
        }

        /// <summary>
        /// Updates the details of an existing user with the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to update.</param>
        /// <param name="userDto">An object containing the updated user information.</param>
        /// <returns>A status code</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUserAsync(int id, UserUpdateDto userDto)
        {
            if (!await _unitOfWork.UserRepository.UserExistsAsync(id))
                return NotFound();

            await _unitOfWork.UserRepository.UpdateUserAsync(id, _mapper.Map<User>(userDto));
            await _unitOfWork.SaveAsync();
            return Ok();
        }

        /// <summary>
        /// Deletes the user with the specified identifier, along with the user's associated wallet.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>A status code</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUserAsync(int id)
        {
            if (!await _unitOfWork.UserRepository.UserExistsAsync(id))
                return NotFound();

            await _unitOfWork.UserRepository.DeleteUserAsync(id);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}
