using Application.Common.Interfaces;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Azure
{
    public class AzureService : IAzureService
    {
        private readonly BlobServiceClient _imageBlobServiceClient;
        private readonly BlobServiceClient _modelBlobServiceClient;

        public AzureService(IAzureClientFactory<BlobServiceClient> clientFactory)
        {
            _imageBlobServiceClient = clientFactory.CreateClient("ImageStorageClient");
            _modelBlobServiceClient = clientFactory.CreateClient("ModelStorageClient");
        }
        public async Task<string> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return "";

            var containerClient = _imageBlobServiceClient.GetBlobContainerClient("images");
            await containerClient.CreateIfNotExistsAsync();
            var blobName = $"{file.FileName.Split(".")[0] + Guid.NewGuid()}.png";

            var blobClient = containerClient.GetBlobClient(blobName);

            using (var stream = file.OpenReadStream())
            {
               await blobClient.UploadAsync(stream, overwrite: true);
            }

            return blobClient.Uri.ToString();
        }

        public async Task<string> UploadModel(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return "";

            var containerClient = _modelBlobServiceClient.GetBlobContainerClient("3dmodels");
            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(file.FileName);

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            return blobClient.Uri.ToString();
        }
    }
}
