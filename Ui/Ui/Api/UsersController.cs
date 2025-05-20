using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Ui.Api;

[ApiController]
[Route("api/[controller]")]
public sealed class UsersController(IUserService userService, IUserCsvService userCsvService) : ControllerBase
{
    [HttpPost("parse")]
    public IAsyncEnumerable<UserValidationResult> ParseAndValidate([FromBody] string csv) => 
        userCsvService.ParseAndValidateAsync(csv);

    [HttpPost]
    public async Task<IActionResult> SaveUsers([FromBody] List<User> users)
    {
        await userService.SaveUsersAsync(users);
        return Ok();
    }
}
