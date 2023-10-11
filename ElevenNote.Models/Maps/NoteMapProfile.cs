using AutoMapper;
using ElevenNote.Data.Entities;
using ElevenNote.Models.Note;

namespace ElevenNote.Models.Maps
{
    public class NoteMapProfile : Profile
    {
        public NoteMapProfile()
        {
            CreateMap<NoteEntity, NoteDetail>();
            CreateMap<NoteEntity, NoteListItem>();

            CreateMap<NoteCreate, NoteEntity>()
                .ForMember(note => note.CreatedUtc, opt => opt
                .MapFrom(src => DateTimeOffset.Now));

            CreateMap<NoteUpdate, NoteEntity>()
                .ForMember(note => note.ModifiedUtc, opt => opt
                .MapFrom(src => DateTimeOffset.Now));
        }

    }
}