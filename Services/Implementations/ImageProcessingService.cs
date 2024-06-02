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
            bool needSave = false;

            // Redimensionnement initial si nécessaire
            if (image.Width > maxWidth || image.Height > maxHeight)
            {
                image.Mutate(x => x.Resize(maxWidth, maxHeight));
                needSave = true;
            }

            // Compression par paliers de qualité
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

                // Réduction progressive de la qualité
                quality -= 10;

                // Réduction progressive des dimensions si qualité très faible
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

        public void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
