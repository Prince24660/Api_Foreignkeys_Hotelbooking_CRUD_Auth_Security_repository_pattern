using API_Foreignkey.Models;
using Test2.Models;

namespace API_Foreignkey.IReposiory
{
    public interface IAuthRepository
    {
        User AddUser(User user);
        ServiceResponse Login(LoginRequest loginRequest);
        Role AddRole(Role role);
        bool AssignRoleToUser(AddUserAssignRole obj);
    }
}
