using Pal.Core.Domains.Languages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pal.Services.Languages
{
    public interface ILanguageService
    {
        IQueryable<Language> GetLanguages();
        Language GetLanguageByCulture(string culture);
        public Task<Language> GetLanguageFromRequestAsync();
        public Task<int> GetLanguageIdFromRequestAsync();

        public List<Language> GetAllLanguages();
        bool IsLayoutRtl();
        string GetCurrentLayoutDirection();
    }
}
