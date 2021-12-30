using VBMS.Application.Requests;

namespace VBMS.Application.Interfaces.Services
{
    public interface IUploadService
    {
        string UploadAsync(UploadRequest request);
    }
}