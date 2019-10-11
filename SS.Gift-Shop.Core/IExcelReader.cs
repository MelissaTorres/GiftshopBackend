using System.IO;

namespace SS.GiftShop.Core
{
    public interface IExcelReader
    {
        Table Read(Stream stream);
    }
}
