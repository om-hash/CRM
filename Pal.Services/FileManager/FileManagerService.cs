using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Pal.Core.Domains.Attachments;
using Pal.Core.Enums.Attachment;
using Pal.Services.Logger;
using Syncfusion.EJ2.Inputs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Pal.Services.FileManager
{

    public class FileManagerService : IFileManagerService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly ILoggerService _loggerService;
        private readonly AzureStorageConfig _azureStorageConfig;

        //--------------------------------------------------------------------------------------------
        public FileManagerService(IWebHostEnvironment environment, IConfiguration configuration, ILoggerService loggerService)
        {
            _environment = environment;
            _configuration = configuration;
            _azureStorageConfig = _configuration.GetSection("AppSettings:AzureStorage").Get<AzureStorageConfig>();
            _loggerService = loggerService;
        }

        //----------------------------------------------------------------------------------------------
        private async Task<string> SaveBlobAsync(IFormFile file, string fullPath, string storageContainer)
        {
            try
            {
                var container = new BlobContainerClient(_azureStorageConfig.ConnectionString, storageContainer);
                var createResponse = await container.CreateIfNotExistsAsync();
                if (createResponse != null && createResponse.GetRawResponse().Status == 201)
                    await container.SetAccessPolicyAsync(PublicAccessType.BlobContainer);
                var blob = container.GetBlobClient(fullPath);
                await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);

                var stream = new MemoryStream();
                //var stream = new FileStream(blob.Uri.OriginalString, FileMode.Create);


                await file.CopyToAsync(stream);
                stream.Position = 0;
                await blob.UploadAsync(stream, new BlobHttpHeaders
                {
                    //ContentType = contentType,
                    CacheControl = "max-age=31536000",
                });


                return blob.Uri.ToString();
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync(nameof(SaveBlobAsync), ex);
                throw;
            }

        }

        //----------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="files">List Of the Files You want yo save</param>
        /// <param name="generateRandomFileName">to generate random file name</param>
        /// <param name="mediaType">what ever</param>
        /// <returns></returns>
        public async Task<List<Attachment>> UploadFilesAsync(List<IFormFile> files,
            ReferenceType referenceType,
            bool generateRandomFileName,
            MediaType mediaType,
            string referenceNo)
        {
            var result = new List<Attachment>();
            try
            {
                foreach (var file in files)
                {
                    try
                    {
                        // file name

                        string fileName = file.FileName;

                        if (generateRandomFileName)
                            fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                        var pathToSave = Path.Combine(referenceType.ToString(), referenceNo, mediaType.ToString(), fileName);

                        var url = await SaveBlobAsync(file, pathToSave, _azureStorageConfig.ContainerName);

                        result.Add(new()
                        {
                            ReferenceType = referenceType,
                            FileName = url,
                            MediaType = mediaType,
                            ReferenceId = referenceNo,

                        });


                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync(nameof(UploadFilesAsync), ex);
                return null;
            }
        }

        //---------------------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file">File You want yo save</param>
        /// <param name="folder">sub folder if any</param>
        /// <param name="generateRandomFileName">to generate random file name</param>
        /// <param name="mediaType">what ever</param>
        /// <returns></returns>
        public async Task<string> UploadFileAsync(IFormFile file,
             ReferenceType referenceType,
            bool generateRandomFileName = false,
            MediaType mediaType = MediaType.Photos,
            string referenceNo = "0", string filename = null)
        {

            try
            {
                if (file == null)
                {
                    return null;
                }
                string mainFolder = mediaType.ToString();

                // file name
                string fileName = file.FileName;
                if (filename != null)
                    fileName = filename;
                else if (generateRandomFileName)
                    fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                var pathToSave = Path.Combine(referenceType.ToString(), referenceNo, mediaType.ToString(), fileName);


                string wwwPath = _environment.WebRootPath;
                string contentPath = _environment.ContentRootPath;

                string path = Path.Combine(_environment.WebRootPath, referenceType.ToString());
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                List<string> uploadedFiles = new List<string>();
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    file.CopyTo(stream);
                }
               
                //var url = await SaveBlobAsync(file, pathToSave, _azureStorageConfig.ContainerName);

                return Path.Combine(referenceType.ToString(), fileName); ;
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync(nameof(UploadFileAsync), ex);
                return null;
            }
        }

        public string UploadLogo(IFormFile file)
        {
            try
            {

                if (file == null) return null;
                string wwwPath = Path.Combine(_environment.WebRootPath, "logo");
                string fileName = "myLogo.png";
                string path = Path.Combine(wwwPath, fileName);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
              
                return path;
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync(nameof(UploadFileAsync), ex);
                return null;
            }
        }


        //---------------------------------------------------------------------------------------------------
        public async Task DeleteFileAsync(string url)
        {
            try
            {

                if (url != null)
                {
                    url = url.Trim().Replace("/", "\\");
                    var filepath = _environment.WebRootPath + url; //Path.Combine(_environment.WebRootPath,"s\\ssss");
                    await Task.Run(() =>
                    {
                        if (File.Exists(filepath))
                            File.Delete(filepath);

                    });
                }

            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync(nameof(DeleteFileAsync), ex);
            }
        }
        //---------------------------------------------------------------------------------------------------


    }
}
