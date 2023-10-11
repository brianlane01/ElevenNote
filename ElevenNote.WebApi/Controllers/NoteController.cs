using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElevenNote.Services.Note;
using ElevenNote.Models.Note;
using ElevenNote.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElevenNote.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;
        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] NoteCreate request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _noteService.CreateNoteAsync(request);
            if (response is not null)
            {
                return Ok(response);
            }

            return BadRequest(new TextResponse("Could not create note."));
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<NoteListItem>), 200)]
        public async Task<IActionResult> GetAllNotes()
        {
            var notes = await _noteService.GetAllNotesAsync();
            return Ok(notes);
        }

        [HttpGet("{noteId:int}")]
        public async Task<IActionResult> GetNoteById([FromRoute] int noteId)
        {
            NoteDetail? detail = await _noteService.GetNoteByIdAsync(noteId);
            return detail is not null 
            ? Ok(detail)
            : NotFound();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateNoteById([FromBody] NoteUpdate request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await _noteService.UpdateNoteAsync(request)
                ? Ok("Note updated successfully.")
                : BadRequest("Note could not be found.");
        }

        //!Delete api/Note/5
        [HttpDelete("{noteId:int}")]
        public async Task<IActionResult> DeleteNote([FromRoute] int noteId)
        {
            return await _noteService.DeleteNoteAsync(noteId)
                ? Ok($"Note {noteId} was deleted successfully.")
                : BadRequest($"Note {noteId} could not be deleted.");
        }

    }
}
