namespace store_management_mono_api.Modules.NoteModule._ViewModels;

public class NoteOtherRequestVm
{
    public string Title { get; set; }
    public string Content { get; set; }
}

public class NoteOtherResponseVm
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string EditedBy { get; set; }
    public DateTime EditedAt { get; set; }
}