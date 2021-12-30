namespace VBMS.Application.Requests.vbms.partner
{
    public class GetAllPagedPartnersRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}
