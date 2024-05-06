using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OnlineBookstore.Application.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstore.Application.Utilies
{
    public class AzureBlogService
    {
        private readonly AppSettings _appSettings;
        private const string ContainerName = "myfiles";
        //public const string SuccessMessageKey = "SuccessMessage";
        //public const string ErrorMessageKey = "ErrorMessage";
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobContainerClient _containerClient;

        private readonly ILogger<AzureBlogService> _logger;

        public AzureBlogService(BlobServiceClient blobServiceClient, IOptions<AppSettings> appSettings, ILogger<AzureBlogService> logger)
        {
            _appSettings = appSettings.Value;
            _logger = logger;
            _blobServiceClient = blobServiceClient;
            _containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
            _containerClient.CreateIfNotExists();
        }
        public async Task<string> UploadBlob(string blobName, string base64String)
        {
            // Convert Base64 to byte array
            byte[] data = Convert.FromBase64String(base64String);
            MemoryStream stream = new MemoryStream(data);
            var blobClient = _containerClient.GetBlobClient(blobName);
            //var blockBlob = _containerClient.CanGenerateSasUri;
            await blobClient.UploadAsync(stream, true);
            _logger.LogInformation("Upload image was successful");
            var sharedAccessBlobPolicy = new BlobSasBuilder(BlobSasPermissions.Read, DateTime.Now.AddYears(50));
            

            var sasToken = blobClient.GenerateSasUri(sharedAccessBlobPolicy);

            //blobClient = _containerClient.GetBlobClient(blobName);
            //var memoryStream = new MemoryStream();
            //await blobClient.DownloadToAsync(memoryStream);
            //memoryStream.Position = 0;
            //var contentType = blobClient.GetProperties().Value.ContentType;

              //var blobUrlWithSas = $"{blobClient.Uri}{sasToken.Split("&")[0]}";

            return sasToken.ToString();
        }
      
    }
}
