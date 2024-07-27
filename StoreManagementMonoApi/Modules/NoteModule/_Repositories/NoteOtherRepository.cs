using Microsoft.EntityFrameworkCore;
using store_management_mono_api.Context;
using store_management_mono_api.Entities;
using store_management_mono_api.Exceptions;
using store_management_mono_api.Modules.NoteModule._Repositories._Interfaces;
using store_management_mono_api.Modules.NoteModule._ViewModels;
using store_management_mono_api.ViewModels;

namespace store_management_mono_api.Modules.NoteModule._Repositories;

public class NoteOtherRepository : INoteOtherRepository
{
    private AppDbContext _appDbContext;

    public NoteOtherRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<NoteOtherResponseVm> CreateNoteOther(CredentialReq credentialReq, NoteOtherRequestVm noteOtherRequestVm)
    {
        try
        {
            Note saveNote = new Note
            {
                Id = Guid.NewGuid().ToString(),
                Title = noteOtherRequestVm.Title,
                Content = noteOtherRequestVm.Content,
                StoreId = credentialReq.StoreId,
                CreatedAt = DateTime.Now,
                CreatedBy = credentialReq.AccountId,
                EditedAt = DateTime.Now,
                EditedBy = credentialReq.AccountId,
            };

            var entityEntry = await _appDbContext.Notes.AddAsync(saveNote);
            await _appDbContext.SaveChangesAsync();
            var response = entityEntry.Entity;
            return new NoteOtherResponseVm
            {
                Id = response.Id,
                Title = response.Title,
                Content = response.Content,
                CreatedBy = response.CreatedBy,
                CreatedAt = response.CreatedAt,
                EditedBy = response.EditedBy,
                EditedAt = response.EditedAt
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<NoteOtherResponseVm>> GetNoteOthers(string storeId)
    {
        var notes = await _appDbContext.Notes.Where(n => n.StoreId.Equals(storeId))
            .Include(note => note.CreatedByNavigation)
            .Include(note => note.EditedByNavigation)
            .OrderByDescending(n => n.EditedAt)
            .ToListAsync();
        return notes.Select(n => new NoteOtherResponseVm
        {
            Id = n.Id,
            Title = n.Title,
            Content = n.Content,
            CreatedBy = n.CreatedByNavigation.FullName,
            CreatedAt = n.CreatedAt,
            EditedBy = n.EditedByNavigation.FullName,
            EditedAt = n.EditedAt
        });
    }
    public async Task<NoteOtherResponseVm> GetNoteOtherById(string noteId)
    {
        var n = await _appDbContext.Notes.Where(n => n.Id.Equals(noteId))
            .Include(note => note.CreatedByNavigation).Include(note => note.EditedByNavigation)
            .FirstOrDefaultAsync();
        if (n == null) throw new NotFoundException("Catatan tidak ditemukan");
        return new NoteOtherResponseVm
        {
            Id = n.Id,
            Title = n.Title,
            Content = n.Content,
            CreatedBy = n.CreatedByNavigation.FullName,
            CreatedAt = n.CreatedAt,
            EditedBy = n.EditedByNavigation.FullName,
            EditedAt = n.EditedAt
        };
    }

    public async Task<NoteOtherResponseVm> UpdateNoteOther(string noteId, CredentialReq credentialReq, NoteOtherRequestVm noteOtherRequestVm)
    {
        var note = await _appDbContext.Notes.Where(n => n.Id.Equals(noteId)).Include(n => n.EditedByNavigation)
            .Include(n => n.CreatedByNavigation).FirstOrDefaultAsync();
        if (note == null) throw new NotFoundException("Catatan tidak ditemukan");

        try
        {
            note.Content = noteOtherRequestVm.Content;
            note.Title = noteOtherRequestVm.Title;
            note.EditedAt = DateTime.Now;
            note.EditedBy = credentialReq.AccountId;

            _appDbContext.Update(note);
            await _appDbContext.SaveChangesAsync();
            return new NoteOtherResponseVm
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content,
                CreatedBy = note.CreatedByNavigation.FullName,
                CreatedAt = note.CreatedAt,
                EditedBy = note.EditedByNavigation.FullName,
                EditedAt = note.EditedAt
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task DeleteNoteOther(string noteId)
    {
        var note = await _appDbContext.Notes.FindAsync(noteId);
        if (note == null) throw new NotFoundException("Catatan tidak ditemukan");
        try
        {
            _appDbContext.Notes.Remove(note);
            await _appDbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}