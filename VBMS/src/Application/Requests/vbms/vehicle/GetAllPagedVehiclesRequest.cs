namespace VBMS.Application.Requests.vbms.vehicle
{
    public class GetAllPagedVehiclesRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}
