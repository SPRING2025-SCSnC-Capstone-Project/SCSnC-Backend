using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IAzureService
    {
        public Task<String> UploadFile(IFormFile file, string name);
        public Task<String> UploadModel(IFormFile file);
        public Task<List<string>> UploadMultipleImage(IFormFile[] files, string name);
    }
}
