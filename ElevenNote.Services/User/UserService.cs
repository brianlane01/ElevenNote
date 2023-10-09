using ElevenNote.Models.User;
using ElevenNote.Data.Entities;
using ElevenNote.Data;
using Microsoft.AspNetCore.Identity;

namespace ElevenNote.Services.User
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;

        public UserService(ApplicationDbContext context,
                            UserManager<UserEntity> userManager,
                            SignInManager<UserEntity> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<UserDetail?> GetUserByIdAsync(int userId)
        {
            UserEntity? entity = await _context.User.FindAsync(userId);
            if(entity is null)
                return null;
            
            UserDetail detail = new UserDetail()
            {
                Id = entity.Id,
                Email = entity.Email,
                UserName = entity.UserName!,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                DateCreated = entity.DateCreated
            };

            return detail;
        }

        public async Task<bool> RegisterUserAsync(UserRegister model)
        {   
            if(await CheckEmailAvailability(model.Email) == false)
            {
                System.Console.WriteLine("Please Try a Different Email, Email Entered Linked To Existing Account.");
                return false;
            }

            if(await CheckUserNameAvailability(model.UserName) == false)
            {
                System.Console.WriteLine("UserName already taken please try again");
                return false; 
            }

            UserEntity entity = new UserEntity()
            {
                Email = model.Email,
                UserName = model.UserName,
                DateCreated = DateTime.Now
            };

            IdentityResult registerResult = await _userManager.CreateAsync(entity, model.Password);

            return registerResult.Succeeded;
        }

        private async Task<bool> CheckUserNameAvailability(string userName)
        {
            UserEntity? existingUser = await _userManager.FindByNameAsync(userName);
            return existingUser is null;
        }

        private async Task<bool> CheckEmailAvailability(string email)
        {
            UserEntity? existingUser = await _userManager.FindByEmailAsync(email);
            return existingUser is null; 
        }

        // private async Task<bool> IsEmailUniqueAsync(string email)
        // {
        //     // Check if the provided email is unique in the database.
        //     var existingUser = await _userManager.FindByEmailAsync(email);
        //     return existingUser == null;
        // }

        // private async Task<bool> IsUsernameUniqueAsync(string username)
        // {
        //     // Check if the provided username is unique in the database.
        //     var existingUser = await _userManager.FindByNameAsync(username);
        //     return existingUser == null;
        // }
    }
}