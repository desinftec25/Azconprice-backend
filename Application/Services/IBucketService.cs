﻿using Application.Models.DTOs;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public interface IBucketService
    {
        Task<string> UploadAsync(IFormFile file);
        Task<string> UploadAsync(IFormFile file,string fileName);
        Task<string> UploadTaxIdAsync(IFormFile file,string fileName);
        Task<string> UploadExcelAsync(IFormFile file, string firstName, string lastName, string email, string userId);
        Task<string> GetSignedUrlAsync(string path, int expiresInSeconds = 3600);
    }
}
