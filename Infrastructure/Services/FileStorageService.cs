using Application.Ports.Driven;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Services;

public class FileStorageService : IFileStorageService {
    private string _basePath = string.Empty;
    private readonly IConfiguration _configuration;
    public FileStorageService(IConfiguration configuration) {
        _configuration = configuration;
    }
    public async Task<string> SaveAsync(IFormFile file, string folder) {
        _basePath = _configuration["FileStorage:BasePath"]??string.Empty;
        var directory = Path.Combine(_basePath, folder);

        if(!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
        var fullPath = Path.Combine(directory, fileName);

        using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);

        return Path.Combine("uploads", folder, fileName).Replace("\\", "/");
    }
}
