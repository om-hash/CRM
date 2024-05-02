using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Pal.Services.Languages;
using System.Threading;

namespace Pal.Web.Extensions
{
    public abstract class CustomBaseViewPage<TModel> : Microsoft.AspNetCore.Mvc.Razor.RazorPage<TModel>
    {
        [RazorInject]
        public ILanguageService LanguageService { get; set; }

        [RazorInject]
        public ILocalizationService LocalizationService { get; set; }

        public delegate HtmlString Localizer(string resourceKey, params object[] args);
        private Localizer _localizer;

        public Localizer T
        {
            get
            {
                if (_localizer == null)
                {
                    var currentCulture = Thread.CurrentThread.CurrentUICulture.Name;

                    var language = LanguageService.GetLanguageByCulture(currentCulture);
                    if (language != null)
                    {
                        _localizer = (resourceKey, args) =>
                        {
                            var ResourceValue = LocalizationService.GetStringResource(resourceKey, language.Id);

                            if (string.IsNullOrEmpty(ResourceValue))
                            {
                                return new HtmlString(resourceKey);
                            }
                            //return null;
                            return new HtmlString((args == null || args.Length == 0)
                                ? ResourceValue
                                : string.Format(ResourceValue, args));
                        };
                    }
                }
                return _localizer;
            }
        }
    }

    public abstract class CustomBaseViewPage : CustomBaseViewPage<dynamic>
    { }
}
