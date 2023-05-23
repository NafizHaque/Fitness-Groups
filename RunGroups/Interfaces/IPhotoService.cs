using CloudinaryDotNet.Actions;

namespace RunGroups.Interfaces
{
    public interface IPhotoService
    {

        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
