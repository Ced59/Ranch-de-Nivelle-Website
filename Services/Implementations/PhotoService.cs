using RanchDuBonheur.Services.Interfaces;

namespace RanchDuBonheur.Services.Implementations
{
    public class PhotoService(ImageProcessingService imageProcessingService) : IPhotoService
    {
        public async Task<(bool IsSuccess, string PhotoPath)> UploadPhotoAsync(string name, IFormFile photo, string folder)
        {
            var safeName = name.Replace(" ", "");
            var randomPart = Path.GetRandomFileName();
            var fileName = $"{safeName}-{randomPart}{Path.GetExtension(photo.FileName)}";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", folder);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var filePath = Path.Combine(path, fileName);

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }

            var resizedPath = await imageProcessingService.ResizeAndCompressImageAsync(filePath, 100);
            if (filePath != resizedPath)
            {
                imageProcessingService.DeleteFile(filePath);
            }

            return (true, $"/images/{folder}/{fileName}");
        }

        public bool DeletePhoto(string photoUrl)
        {
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", photoUrl);
            try
            {
                imageProcessingService.DeletePhoto(fullPath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}