using System;
using ElevenNote.Data;
using ElevenNote.Data.Entities;
using ElevenNote.Models.Note;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

        public async Task<NoteListItem?> CreateNoteAsync(NoteCreate request)
        {
            NoteEntity entity = new()
            {
                Title = request.Title,
                Content = request.Content,
                OwnerId = _userId,
                CreatedUtc = DateTimeOffset.Now
            };

            _dbContext.Notes.Add(entity);
            var numberOfChanges = await _dbContext.SaveChangesAsync();

            if(numberOfChanges != 1)
            {
                return null;
            }

            NoteListItem response = new()
            {
                Id = entity.Id,
                Title = entity.Title,
                CreatedUtc = entity.CreatedUtc
            };

            return response;
        }

        public async Task<IEnumerable<NoteListItem>> GetAllNotesAsync()
        {
            List<NoteListItem> notes = await _dbContext.Notes 
                .Where(entity => entity.OwnerId == _userId)
                .Select(entity => new NoteListItem
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    Content = entity.Content,
                    CreatedUtc = entity.CreatedUtc
                })
                .ToListAsync();

            return notes;
        }

        public async Task<NoteDetail?> GetNoteByIdAsync(int noteId)
        {
            NoteEntity? entity = await _dbContext.Notes
                .FirstOrDefaultAsync(e => e.Id == noteId && e.OwnerId == _userId);

            return entity is null ? null : new NoteDetail
            {
                Id = entity.Id,
                Title = entity.Title,
                Content = entity.Content,
                CreatedUtc = entity.CreatedUtc,
                ModifiedUtc = entity.ModifiedUtc
            };
        }

        public async Task<bool> UpdateNoteAsync(NoteUpdate request)
        {
            //* Find the note and validate it's owned by the user
            NoteEntity? entity = await _dbContext.Notes.FindAsync(request.Id);

            //*By using the null operator we can check if it's null 
            //? And at the ame time we chack the OwnerId vs the _userId

            if(entity?.OwnerId != _userId)
            {
                return false;
            }

            //* Now we update the entity's properties
            entity.Title = request.Title;
            entity.Content = request.Content;
            entity.ModifiedUtc = DateTimeOffset.Now;

            //* Save the changes to the database and capture how many rows were updated
            int numberOfChanges = await _dbContext.SaveChangesAsync();
            
            //* numberOfChanges is stated to be equal to 1 because only one row is updated
            return numberOfChanges == 1;
        }

    }
}