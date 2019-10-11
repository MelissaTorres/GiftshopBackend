using System.IO;

namespace SS.GiftShop.Core
{
    public interface IImageResizer
    {
        void Resize(Stream input, Stream output, int maxWidth, int maxHeight);
    }
}
