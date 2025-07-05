using Application.Common.Interfaces;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Azure
{
    public class AzureService : IAzureService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public AzureService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }
        public async Task<string> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return "";

            var containerClient = _blobServiceClient.GetBlobContainerClient("images");
            await containerClient.CreateIfNotExistsAsync();
            var blobName = $"{file.FileName.Split(".")[0] + Guid.NewGuid()}.png";

            var blobClient = containerClient.GetBlobClient(blobName);

            using (var stream = file.OpenReadStream())
            {
               await blobClient.UploadAsync(stream, overwrite: true);
            }

            return blobClient.Uri.ToString();
        }
    }
}
