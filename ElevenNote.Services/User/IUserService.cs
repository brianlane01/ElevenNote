using ElevenNote.Models.User;

namespace ElevenNote.Services.User
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsnc(UserRegister model);
    }
}