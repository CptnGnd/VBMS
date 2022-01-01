namespace VBMS.Application.Requests.vbms.booking
{
    public class GetAllPagedBookingsRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}
