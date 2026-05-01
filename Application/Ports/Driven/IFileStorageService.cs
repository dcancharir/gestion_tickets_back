using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Ports.Driven;

public interface IFileStorageService {
    Task<string> SaveAsync(IFormFile file, string folder);
}
