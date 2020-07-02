namespace Isitar.TimeTracking.Infrastructure.StorageProvider
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Common.Entities;
    using Application.Common.Interfaces;

    public class FileStorageProvider : IStorageProvider
    {
        public string BasePath { get; }

        public FileStorageProvider(FileStorageConfig fileStorageConfig)
        {
            BasePath = fileStorageConfig.BasePath;
        }

        public Task<Result<string>> SaveAsync(string localPath, string filename)
        {
            var content = File.OpenRead(localPath);
            return SaveAsync(content, filename);
        }

        public async Task<Result<string>> SaveAsync(Stream content, string filename)
        {
            try
            {
                var storageFilename = CreateFilename(filename);
                var fs = File.Create(CreatePath(storageFilename));
                content.Seek(0, SeekOrigin.Begin);
                await content.CopyToAsync(fs);
                fs.Close();
                return Result<string>.Success(storageFilename);
            }
            catch (Exception e)
            {
                return Result<string>.Failure(new[] {e.Message});
            }
        }

        public async Task<Result<(Stream content, string filename)>> OpenAsync(string storagePath)
        {
            try
            {
                var origFilename = GetOriginalFilename(storagePath);
                var fs = new FileStream(CreatePath(storagePath), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var ms = new MemoryStream();
                fs.Seek(0, SeekOrigin.Begin);
                await fs.CopyToAsync(ms);
                return Result<(Stream content, string filename)>.Success((ms, origFilename));
            }
            catch (Exception e)
            {
                return Result<(Stream content, string filename)>.Failure(new[] {e.Message});
            }
        }

#pragma warning disable 1998
        public async Task<Result> DeleteAsync(string storagePath)
#pragma warning restore 1998
        {
            try
            {
                var path = CreatePath(storagePath);
                File.Delete(path);
                return Result.Success();
            }
            catch (Exception e)
            {
                return Result.Failure(new[] {e.Message});
            }
        }

        protected virtual string CreatePath(string storageFilename)
        {
            return Path.Combine(BasePath, storageFilename);
        }

        protected virtual string CreateFilename(string filename)
        {
            return $"{Guid.NewGuid():N}.{filename}";
        }

        protected virtual string GetOriginalFilename(string filename)
        {
            return string.Join(".", filename.Split(".").Skip(1));
        }
    }
}