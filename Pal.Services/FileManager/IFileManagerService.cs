using Microsoft.AspNetCore.Http;
using Pal.Core.Domains.Attachments;
using Pal.Core.Enums.Attachment;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pal.Services.FileManager
{
    public interface IFileManagerService 
    {
        public Task<List<Attachment>> UploadFilesAsync(List<IFormFile> files,
            ReferenceType referenceType,
            bool generateRandomFileName = false,
            MediaType mediaType = MediaType.Photos,
            string referenceNo = "0");


        public Task<string> UploadFileAsync(IFormFile file,
            ReferenceType referenceType,
            bool generateRandomFileName = false,
            MediaType mediaType = MediaType.Photos,
            string referenceNo = "0", string filename = null);


        public Task DeleteFileAsync(string url);
        string UploadLogo(IFormFile file);
    }
}