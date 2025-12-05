using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OpenCRM.Core.Extensions;
using OpenCRM.Core.Models;
using OpenCRM.Core.Web.Extensions;
using OpenCRM.Core.Web.Models;

namespace OpenCRM.Core.Web.Services
{
    public class MediaService<TDBContext> : IMediaService where TDBContext : DataContext
    {
        private readonly TDBContext dbContextClass;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MediaService(TDBContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            dbContextClass = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<MediaEntity> PostFileAsync(IFormFile fileData, bool isPublic = false)
        {
            try
            {
                var filename = fileData.FileName ?? "UnknowFileName.generic";
                var extension = Path.GetExtension(filename);

                var fileDetails = new MediaEntity()
                {
                    ID = Guid.NewGuid(),
                    FileName = filename,
                    Extension = extension,
                    FileType = GetMediaType(extension),
                    IsPublic = isPublic
                };

                using (var stream = new MemoryStream())
                {
                    fileData.CopyTo(stream);
                    fileDetails.FileData = stream.ToArray();
                }

                StoreMediaToPublicFile(fileDetails);

                var result = dbContextClass.Medias.Add(fileDetails);
                await dbContextClass.SaveChangesAsync();

                return fileDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task PostFileAsync(MediaModel model)
        {
            try
            {
                var filename = model.FileName ?? "UnknowFileName.generic";
                var extension = Path.GetExtension(filename);

                var fileDetails = new MediaEntity()
                {
                    ID = Guid.NewGuid(),
                    FileName = filename,
                    Extension = extension,
                    FileType = GetMediaType(extension),
                    IsPublic = model.IsPublic
                };

                using (var stream = new MemoryStream())
                {
                    model.FileData.CopyTo(stream);
                    fileDetails.FileData = stream.ToArray();
                }
                if (model.IsPublic)
                {
                    StoreMediaToPublicFile(fileDetails);
                }

                var result = dbContextClass.Medias.Add(fileDetails);
                await dbContextClass.SaveChangesAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task PostMultiFileAsync(List<MediaUploadModel> fileData)
        {

            try
            {
                foreach (MediaUploadModel file in fileData)
                {
                    var filename = file.FileDetails?.FileName ?? "UnknowFileName.generic";
                    var extension = Path.GetExtension(filename);

                    var fileDetails = new MediaEntity()
                    {
                        ID = Guid.NewGuid(),
                        FileName = filename,
                        Extension = extension,
                        FileType = GetMediaType(extension),
                        IsPublic = file.IsPublic
                    };

                    using (var stream = new MemoryStream())
                    {
                        file.FileDetails?.CopyTo(stream);
                        fileDetails.FileData = stream.ToArray();
                    }

                    if (file.IsPublic)
                    {
                        StoreMediaToPublicFile(fileDetails);
                    }

                    var result = dbContextClass.Medias.Add(fileDetails);
                }
                await dbContextClass.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<MediaEntity?> DownloadFileById(Guid uuid)
        {
            try
            {
                var file = await dbContextClass.Medias.Where(x => x.ID == uuid).FirstOrDefaultAsync();
                if (file == null) return null;
                if (file.FileName == null) return null;
                if (file.FileData == null) return null;

                if (file.IsPublic)
                {
                    StoreMediaToPublicFile(file);
                }

                var content = new System.IO.MemoryStream(file.FileData);

                var webRootPath = EnvironmentExtensions.GetWebRoot();
                if (!string.IsNullOrEmpty(webRootPath))
                {
                    var path = Path.Combine(webRootPath, "media", file.FileName);
                    await CopyStream(content, path);
                }

                return file;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task CopyStream(Stream stream, string downloadPath)
        {
            using (var fileStream = new FileStream(downloadPath, FileMode.Create, FileAccess.Write))
            {
                await stream.CopyToAsync(fileStream);
            }
        }

        private static void StoreMediaToPublicFile(MediaEntity fileDetails)
        {
            var webRootPath = EnvironmentExtensions.GetWebRoot();

            if (!string.IsNullOrEmpty(webRootPath))
            {
                var mediaPublicDir = "media";

                string mediaPublicDirPath = Path.Combine(webRootPath, mediaPublicDir);

                if (!Directory.Exists(mediaPublicDirPath))
                    Directory.CreateDirectory(mediaPublicDirPath);

                var extension = Path.GetExtension(fileDetails.FileName);
                var filePath = Path.Combine(mediaPublicDirPath, fileDetails.ID.ToString() + extension);
                if (fileDetails == null || fileDetails.FileData == null) return;
                File.WriteAllBytes(filePath, fileDetails.FileData);
            }
        }
        public List<MediaEntity> GetMedias()
        {
            var result = dbContextClass.Medias.ToList() ?? new List<MediaEntity>();
            return result;
        }
        public MediaEntity GetMedia(Guid Id)
        {
            return dbContextClass.Medias.FirstOrDefault(s => s.ID == Id);
        }
        public async Task RemoveMedia(Guid Id)
        {
            var media = await dbContextClass.Medias.FindAsync(Id);
            if (media == null) return;
            dbContextClass.Medias.Remove(media);
            dbContextClass.SaveChanges();
        }
        public async Task<MediaEntity> EditFileAsync(Guid Id, MediaEntity media)
        {
            try
            {
                var entity = await dbContextClass.Medias.FindAsync(Id);

                if (entity == null) return null;

                entity.FileName = media.FileName;
                entity.FileType = media.FileData != null ? media.FileType : entity.FileType;
                entity.IsPublic = media.IsPublic;
                entity.FileData = media.FileData != null ? media.FileData : entity.FileData;
                entity.UpdatedAt = DateTime.UtcNow;

                if (media.IsPublic)
                {
                    StoreMediaToPublicFile(entity);
                }
                await dbContextClass.SaveChangesAsync();
                return entity;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public MediaType GetMediaType(string extension)
        {

            switch (extension)
            {
                case ".doc":
                    {
                        return MediaType.DOCX;
                    }
                case ".docx":
                    {
                        return MediaType.DOCX;
                    }
                case ".pdf":
                    {
                        return MediaType.PDF;
                    }
                case ".png":
                    {
                        return MediaType.IMAGE;
                    }
                case ".jpg":
                    {
                        return MediaType.IMAGE;
                    }
                case ".svg":
                    {
                        return MediaType.IMAGE;
                    }
                default:
                    {
                        return MediaType.GENERIC;
                    }
            }
        }

        public string GetMediaUrl(string mediaId)
        {
            var baseUrl = _httpContextAccessor.GetBaseUrl();
            if (string.IsNullOrEmpty(baseUrl)) return string.Empty;

            var mediaGuid = Guid.Parse(mediaId);
            var media = GetMedia(mediaGuid);
            if (media == null) return string.Empty;

            var extension = Path.GetExtension(media.FileName);

            return $"{baseUrl}/media/{media.ID.ToString() + extension}";
        }

        public bool IsImage(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            return (extension == ".png" || extension == ".jpg" || extension == ".jpeg" ||
                    extension == ".gif" || extension == ".svg" || extension == ".webp");
        }

        public List<MediaBlockModel> GetImageMedias()
        {
            var medias = GetMedias();
            var mediasUrl = new List<MediaBlockModel>();

            foreach (var media in medias)
            {
                if (IsImage(media.FileName))
                {
                    mediasUrl.Add(new MediaBlockModel()
                    {
                        Id = media.ID,
                        ImageName = media.FileName,
                        ImageUrl = GetMediaUrl(media.ID.ToString())
                    });
                }
            }
            return mediasUrl;
        }
    }

}
