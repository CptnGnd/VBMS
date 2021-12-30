using System.ComponentModel;

namespace VBMS.Application.Enums
{
    public enum UploadType : byte
    {
        [Description(@"Images\ProductTests")]
        ProductTest,

        [Description(@"Images\ProfilePictures")]
        ProfilePicture,

        [Description(@"Documents")]
        Document,

        [Description(@"Partners")]
        Partner,

        [Description(@"Vehicles")]
        Vehicle
    }
}