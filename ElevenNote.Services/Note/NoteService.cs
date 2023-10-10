using ElevenNote.Data;
using ElevenNote.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace ElevenNote.Services.Note
{
    public class NoteService : INoteService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly int _userId;

        public NoteService(UserManager<UserEntity> userManager,
                            SignInManager<UserEntity> signInManager,
                            ApplicationDbContext dbContext)
        {
            var currentUser = signInManager.Context.User;
            var userIdClaim = userManager.GetUserId(currentUser);
            var hasValidId = int.TryParse(userIdClaim, out _userId);

            if(hasValidId == false)
            {
                throw new Exception("Attempted to build NoteService without Id Claim.");
            }

            _dbContext = dbContext;
        }
    }
}