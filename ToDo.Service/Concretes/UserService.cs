using Microsoft.AspNetCore.Identity;
using ToDo.Models.Dtos.User.Request;
using ToDo.Models.Entities;
using ToDo.Service.Abstracts;

namespace ToDo.Service.Concretes;

public sealed class UserService : IUserService
{
    private readonly UserManager<User> _userManager;

    public UserService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<User> ChangePasswordAsync(string id, UserChangePasswordRequest changePasswordRequestDto)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) throw new Exception("User not found");

            var result = await _userManager.ChangePasswordAsync(user, changePasswordRequestDto.CurrentPassword, changePasswordRequestDto.NewPassword);
            CheckForIdentityResult(result);

            return user;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error changing password: {ex.Message}");
        }
    }

    public async Task<string> DeleteAsync(string id)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) throw new Exception("User not found");

            var result = await _userManager.DeleteAsync(user);
            CheckForIdentityResult(result);

            return "User deleted successfully";
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting user: {ex.Message}");
        }
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new Exception("User not found");

            return user;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error fetching user by email: {ex.Message}");
        }
    }

    public async Task<User> LoginAsync(UserLoginRequestDto loginRequestDto)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDto.Email);
            if (user == null) throw new Exception("Invalid email or password");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            if (!isPasswordValid) throw new Exception("Invalid email or password");

            return user;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error during login: {ex.Message}");
        }
    }

    public async Task<User> RegisterAsync(UserRegisterRequestDto registerDto)
    {
        try
        {
            var user = new User
            {
                UserName = registerDto.Email,
                Email = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            CheckForIdentityResult(result);

            return user;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error during registration: {ex.Message}");
        }
    }

    public async Task<User> UpdateAsync(string id, UserUpdateRequest updateDto)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) throw new Exception("User not found");

            user.UserName = updateDto.UserName ?? user.UserName;
            //user.Email = updateDto.Email ?? user.Email;

            var result = await _userManager.UpdateAsync(user);
            CheckForIdentityResult(result);

            return user;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error updating user: {ex.Message}");
        }
    }

    private void CheckForIdentityResult(IdentityResult result)
    {
        if (!result.Succeeded)
        {
            throw new Exception(result.Errors.First().Description);
        }
    }
}
