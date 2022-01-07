namespace VBMS.Application.Requests.vbms.diary
{
    public class GetAllPagedDiarysRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}
