﻿using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using RunGroups.Helpers;
using RunGroups.Interfaces;

namespace RunGroups.Service
{
    public class PhotoService: IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        public PhotoService(IOptions<CloudinarySettings> config) 
        {
            Account acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
                );
            _cloudinary = new Cloudinary( acc );
        }

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            ImageUploadResult uploadResult = new ImageUploadResult();
            if(file.Length > 0)
            {
                using Stream stream = file.OpenReadStream();
                ImageUploadParams uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(360).Width(640).Crop("fill").Gravity("face")
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);    
            }
            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            DeletionParams deleteParams = new DeletionParams(publicId);
            DeletionResult result = await _cloudinary.DestroyAsync(deleteParams);
            return result;
        }

    }
}
