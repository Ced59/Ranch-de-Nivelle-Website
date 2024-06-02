using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RanchDuBonheur.Services.Implementations
{
    public class ImageProcessingService
    {
        public async Task<string> ResizeAndCompressImageAsync(string filePath, int maxFileSizeInKb, int maxWidth = 1920, int maxHeight = 1080, int minDimension = 600)
        {
            using var image = await Image.LoadAsync(filePath);

            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Length < maxFileSizeInKb * 1024)
            {
                return filePath;
            }

            bool needSave = false;

            if (image.Width > maxWidth || image.Height > maxHeight)
            {
                image.Mutate(x => x.Resize(maxWidth, maxHeight));
                needSave = true;
            }

            int quality = 90;
            while (true)
            {
                if (needSave)
                {
                    await using var ms = new MemoryStream();
                    await image.SaveAsJpegAsync(ms, new JpegEncoder { Quality = quality });

                    if (ms.Length < maxFileSizeInKb * 1024)
                    {
                        await File.WriteAllBytesAsync(filePath, ms.ToArray());
                        return filePath;
                    }
                }

                if (quality <= 30 || (image.Width <= minDimension && image.Height <= minDimension))
                {
                    throw new Exception("Cannot compress image to the desired size.");
                }

                quality -= 10;

                if (quality <= 50)
                {
                    int newWidth = (int)(image.Width * 0.75);
                    int newHeight = (int)(image.Height * 0.75);
                    newWidth = Math.Max(newWidth, minDimension);
                    newHeight = Math.Max(newHeight, minDimension);

                    image.Mutate(x => x.Resize(newWidth, newHeight));
                    needSave = true;
                }
            }
        }

        public bool DeletePhoto(string photoUrl)
        {
            // Suppression du préfixe `/images/` pour obtenir un chemin correct dans `wwwroot`
            var localPath = photoUrl.StartsWith('/') ? photoUrl.Substring(1) : photoUrl;
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", localPath);

            return DeleteFile(fullPath);
        }

        public bool DeleteFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
