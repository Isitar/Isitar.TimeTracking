namespace Isitar.TimeTracking.Application.Common.Interfaces
{
    using System.IO;
    using System.Threading.Tasks;
    using Entities;

    public interface IStorageProvider
    {
        public Task<Result<string>> SaveAsync(string localPath, string filename);
        public Task<Result<string>> SaveAsync(Stream content, string filename);
        public Task<Result<(Stream content, string filename)>> OpenAsync(string storagePath);
        public Task<Result> DeleteAsync(string storagePath);
    }
}