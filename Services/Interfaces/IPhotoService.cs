namespace RanchDuBonheur.Services.Interfaces
{
    public interface IPhotoService
    {
        bool DeletePhoto(string photoUrl);
        Task<(bool IsSuccess, string PhotoPath)> UploadPhotoAsync(string name, IFormFile photo, string folder);
    }
}
