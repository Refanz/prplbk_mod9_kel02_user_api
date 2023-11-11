using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using UserAPI.Data;
using UserAPI.Models.DTO;

namespace UserAPI.Controllers;

[Route("api/users")]
[ApiController]
public class UserApiController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetUsers()
    {
        return Ok(UserStore.UsersList);
    }

    [HttpGet("{id:int}", Name = "GetUser")]
    [ProducesResponseType(200, Type = typeof(UserDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(404)]
    public ActionResult<UserDto> GetUser(int id)
    {
        if (id == 0) return BadRequest();
        var user = UserStore.UsersList.FirstOrDefault(u => u.Id == id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpPost]
    public ActionResult<UserDto> CreateUser([FromBody] UserDto userDto)
    {
        if (UserStore.UsersList.FirstOrDefault(u => u.Email.ToLower() == userDto.Email.ToLower()) != null)
        {
            ModelState.AddModelError("CustomError", "Email already exists");
            return BadRequest(ModelState);
        }

        if (userDto == null)
        {
            return BadRequest(userDto);
        }

        if (userDto.Id > 0)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        userDto.Id = UserStore.UsersList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
        UserStore.UsersList.Add(userDto);
        string response = "Sukses menambahkan data User" + "\nId : " + userDto.Id.ToString() + "\nEmail : " +
                          userDto.Email;
        return CreatedAtRoute("GetUser", new { id = userDto.Id }, response);
    }


    [HttpDelete("{id:int}", Name = "DeleteUser")]
    public IActionResult DeleteUser(int id)
    {
        if (id == 0)
        {
            return BadRequest();
        }

        var user = UserStore.UsersList.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        UserStore.UsersList.Remove(user);
        return NoContent();
    }

    [HttpPut("{id:int}", Name = "UpdateUser")]
    public IActionResult UpdateUser(int id, [FromBody] UserDto userDto)
    {
        if (userDto == null || id != userDto.Id)
        {
            return BadRequest();
        }

        var user = UserStore.UsersList.FirstOrDefault(u => u.Id == id);
        user.Email = userDto.Email;
        user.Password = user.Password;
        return NoContent();
    }

    [HttpPatch("{id:int}", Name = "UpdatePartialUser")]
    public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<UserDto> patchDTO)
    {
        if (patchDTO == null || id == 0)
        {
            return BadRequest();
        }

        var user = UserStore.UsersList.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return BadRequest();
        }

        patchDTO.ApplyTo(user, ModelState);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return NoContent();
    }
}