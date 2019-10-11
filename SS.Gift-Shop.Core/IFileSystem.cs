using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SS.GiftShop.Core
{
    public interface IFileSystem
    {
        Task<bool> Delete(string path);

        Task<bool> Exists(string path);

        Task<bool> Read(Stream outputStream, string path);

        //Task<string> Save(Stream contentStream, string container, string extension, string fileName = null, IDictionary<string, string> metadata = null);

        Task Save(Stream contentStream, string path, IDictionary<string, string> metadata = null);

        Task Copy(string source, string destination, IDictionary<string, string> metadata = null);
    }
}
