using Pal.Core.Domains;
using Pal.Core.Domains.Languages;
using Pal.Data.DTOs;
using Pal.Data.DTOs.Languages;
using Syncfusion.EJ2.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Services.DataServices.LanguegService
{
    public interface ILanguagesService
    {
    
        public Task<bool> AddLanguage(LanguageCreateDTO newLanguageCreateDTO);
        public Task<LanguageCreateDTO> GetLanguageById(int id);
        public Task<bool> LanguageUpdate(LanguageCreateDTO newLanguageCreateDTO);
        Task<SyncPaginatedListModel<LanguageCreateDTO>> GetAllLanguages(DataManagerRequest dm);
        public Task<bool> DelelteLanguage(int id);


        public Task<bool> StringResourceUpdate(int id, string value);
        public  Task<LanguageStringResourceDTO> GetStringResourcesById(int id);
        Task<bool> AddNewResource(string key, string value, int langId);
    }
}
