using store_management_mono_api.Modules.NoteModule._ViewModels;
using store_management_mono_api.ViewModels;

namespace store_management_mono_api.Modules.NoteModule._Repositories._Interfaces;

public interface INoteOtherRepository
{
    Task<NoteOtherResponseVm> CreateNoteOther(CredentialReq credentialReq, NoteOtherRequestVm noteOtherRequestVm);
    Task<IEnumerable<NoteOtherResponseVm>> GetNoteOthers(string storeId);
    Task<NoteOtherResponseVm> GetNoteOtherById(string noteId);

    Task<NoteOtherResponseVm> UpdateNoteOther(string noteId, CredentialReq credentialReq,
        NoteOtherRequestVm noteOtherRequestVm);

    Task DeleteNoteOther(string noteId);
}