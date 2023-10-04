using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Layouts;
using System.Reflection;
#if IOS || ANDROID || MACCATALYST
using Microsoft.Maui.Graphics.Platform;
#elif WINDOWS
using Microsoft.Maui.Graphics.Win2D;
#endif
namespace ShopWorld.MAUI.Services
{
    public class ImageService:IImageService
    {
        public byte[] DownSizeImage(byte[] imageByte)
        {
            Microsoft.Maui.Graphics.IImage image;
            // PlatformImage isn't currently supported on Windows.
            MemoryStream memoryStream = new MemoryStream(imageByte);
#if IOS || ANDROID || MACCATALYST
            image = (Microsoft.Maui.Graphics.IImage)PlatformImage.FromStream(memoryStream);
#elif WINDOWS
            image = new W2DImageLoadingService().FromStream(memoryStream);
#endif
            if (image != null)
            {
                Microsoft.Maui.Graphics.IImage newImage = image.Resize(300,300,ResizeMode.Fit);
                byte[] bytes=newImage.AsBytes();
                return bytes;
            }
            return null;
        }
    }
}
