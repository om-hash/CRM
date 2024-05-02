using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pal.Core.Domains.Attachments;
using Pal.Core.Enums;
using Pal.Core.Enums.Attachment;
using Pal.Data.Contexts;
using Pal.Services.FileManager;
using Pal.Services.Languages;
using Pal.Services.Logger;
using Pal.Services.Notifications;
using Pal.Web.Controllers;
using Pal.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pal.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AttachmentsController : BaseController
    {
        private readonly IFileManagerService _fileManagerService;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILoggerService _logger;

        public AttachmentsController(ILoggerService logger,
            IFileManagerService fileManagerService,
            ApplicationDbContext context,
             INotificationService _notificationService,
            ILanguageService languageService, ILocalizationService localizationService, IConfiguration configuration) : base(languageService, localizationService)
        {
            _fileManagerService = fileManagerService;
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        //--------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="files">الملفات</param>
        /// <param name="mediaType">شو النوع</param>
        /// <param name="referenceType">لمين تابعة هي الملفات مثلا للمشاريع ...الخ</param>
        /// <param name="referenceId">رقم المعرف تبع سمير مثلا رقم المشروع التابعة لئله هي الملفات</param>
        /// <returns></returns>
        [HttpPost]
        //TODO AntiForgeryToken
        public async Task<IActionResult> UploadFiles(List<IFormFile> files, int mediaType, int referenceType, string referenceId)
        {
            try
            {
                var result = await _fileManagerService.UploadFilesAsync(files, (ReferenceType)referenceType, true, (MediaType)mediaType,  referenceId);
                if (result != null)
                {
                    result.ForEach(file =>
                    {
                        file.ReferenceId = referenceId;
                    });

                    await _context.Attachments.AddRangeAsync(result);
                    await _context.SaveChangesAsync();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("AttachmentsController" + nameof(UploadFiles), ex);
                return BadRequest();
            }
        }


        //--------------------------------------------------------
        public async Task<IActionResult> DeleteFile(Guid id)
        {
            try
            {
                var attachment = await _context.Attachments.FirstOrDefaultAsync(a => a.Id == id);
                if (attachment == null)
                    return Json(new ResponseResult(ResponseType.Error));


                _ = _fileManagerService.DeleteFileAsync(attachment.FileName);
                _context.Attachments.Remove(attachment);
                await _context.SaveChangesAsync();
                return Json(new ResponseResult(ResponseType.Success));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("AttachmentsController" + nameof(DeleteFile), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }




        //--------------------------------------------------------
        
        //[ProducesResponseType(StatusCodes.Status409Conflict)]
        //[ProducesResponseType(typeof(AzureStorageSASResult), StatusCodes.Status200OK)]
        public IActionResult SASToken()
        {
            try
            {
                var azureStorageConfig = _configuration.GetSection("AppSettings:AzureStorage").Get<AzureStorageConfig>();
                BlobContainerClient container = new(azureStorageConfig.ConnectionString, azureStorageConfig.ContainerName);

                if (!container.CanGenerateSasUri) return Conflict("The container can't generate SAS URI");

                var sasBuilder = new BlobSasBuilder
                {
                    BlobContainerName = container.Name,
                    Resource = "c",
                    ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(azureStorageConfig.TokenExpirationMinutes)
                };

                sasBuilder.SetPermissions(BlobContainerSasPermissions.Create);
                sasBuilder.SetPermissions(BlobContainerSasPermissions.Write);

                var sasUri = container.GenerateSasUri(sasBuilder);

                var result = new AzureStorageSASResult
                {
                    AccountName = container.AccountName,
                    AccountUrl = $"{container.Uri.Scheme}://{container.Uri.Host}",
                    ContainerName = container.Name,
                    ContainerUri = container.Uri,
                    SASUri = sasUri,
                    SASToken = sasUri.Query.TrimStart('?'),
                    SASPermission = sasBuilder.Permissions,
                    SASExpire = sasBuilder.ExpiresOn
                };

                return Json(result);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("AttachmentsController" + nameof(SASToken), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
           
        }


        //-----------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> LinkNewAttachment(Attachment attachment)
        {
            try
            {
                await _context.AddAsync(attachment);
                await _context.SaveChangesAsync();
                return Content("1");
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("AttachmentsController" + nameof(LinkNewAttachment), ex);
                return Content("");
            }
        }

    }

   
}
