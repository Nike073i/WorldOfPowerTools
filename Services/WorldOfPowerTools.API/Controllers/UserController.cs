using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WorldOfPowerTools.API.Extensions;
using WorldOfPowerTools.API.RequestModels.User;
using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Repositories;
using WorldOfPowerTools.Domain.Services;

namespace WorldOfPowerTools.API.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        public static readonly Actions GetAllAccess = Actions.Users;
        public static readonly Actions GetByIdAccess = Actions.Users;
        public static readonly Actions RemoveAccess = Actions.Users;
        public static readonly Actions AddUserRightsAccess = Actions.Users;
        public static readonly Actions RemoveUserRightsAccess = Actions.Users;

        private readonly SecurityService _securityService;
        private readonly IUserRepository _userRepository;

        public UserController(SecurityService securityService, IUserRepository userRepository)
        {
            _securityService = securityService;
            _userRepository = userRepository;
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public async Task<IActionResult> GetAll(int skip = 0, int? count = null)
        {
            if (!_securityService.UserOperationAvailability(User.GetUserRights(), GetAllAccess))
                return StatusCode(StatusCodes.Status405MethodNotAllowed, "У вас нет доступа к этой операции");
            return Ok(await _userRepository.GetAllAsync(skip, count));
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public async Task<IActionResult> GetById([Required] Guid id)
        {
            if (!_securityService.UserOperationAvailability(User.GetUserRights(), GetByIdAccess))
                return StatusCode(StatusCodes.Status405MethodNotAllowed, "У вас нет доступа к этой операции");
            return await _userRepository.GetByIdAsync(id) is { } item ? Ok(item) : NotFound("Пользователь по указанному Id не найден");
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public async Task<IActionResult> RemoveUser([Required] Guid id)
        {
            if (!_securityService.UserOperationAvailability(User.GetUserRights(), RemoveAccess))
                return StatusCode(StatusCodes.Status405MethodNotAllowed, "У вас нет доступа к этой операции");
            try
            {
                var userId = await _userRepository.RemoveByIdAsync(id);
                return Ok(userId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("add_rights")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddUserRights([FromBody] ChangeUserRightModel model)
        {
            if (!_securityService.UserOperationAvailability(User.GetUserRights(), AddUserRightsAccess))
                return StatusCode(StatusCodes.Status405MethodNotAllowed, "У вас нет доступа к этой операции");
            var userId = model.UserId;
            var action = model.Action;
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null) return NotFound("Пользователь по указанному Id не найден");
                user.AllowAction(action);
                var changedUser = await _userRepository.SaveAsync(user);
                return Ok(changedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("remove_rights")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public async Task<IActionResult> RemoveUserRights([FromBody] ChangeUserRightModel model)
        {
            if (!_securityService.UserOperationAvailability(User.GetUserRights(), RemoveUserRightsAccess))
                return StatusCode(StatusCodes.Status405MethodNotAllowed, "У вас нет доступа к этой операции");
            var userId = model.UserId;
            var action = model.Action;
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null) return NotFound("Пользователь по указанному Id не найден");
                user.ProhibitAction(action);
                var changedUser = await _userRepository.SaveAsync(user);
                return Ok(changedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
