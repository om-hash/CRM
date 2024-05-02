using Pal.Core.Domains.Languages;
using System;

namespace Pal.Services.Languages
{
    public interface ILocalizationService : IDisposable
    {
        string GetStringResource(string resourceKey, int languageId);
    }
}
