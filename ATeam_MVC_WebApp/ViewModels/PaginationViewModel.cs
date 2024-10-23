namespace ATeam_MVC_WebApp.ViewModels;

public class PaginationViewModel
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }

    // Calculated property for total pages
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}