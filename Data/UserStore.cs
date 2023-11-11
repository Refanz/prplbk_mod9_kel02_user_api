using UserAPI.Models.DTO;

namespace UserAPI.Data;

public class UserStore
{
    public static List<UserDto> UsersList = new List<UserDto>
    {
        new UserDto{Id=1, Email="refan@gmail.com", Password="123"},
        new UserDto{Id=2, Email="didan@gmail.com", Password="123"},
        new UserDto{Id=3, Email="baihaqi@gmail.com", Password="123"},
        new UserDto{Id=4, Email="khasandra@gmail.com", Password="123"},
    };
}