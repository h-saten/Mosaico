using System.Collections.Generic;
using System.Linq;

namespace Mosaico.Domain.Base.Extensions
{
    public static class TranslatableEntityExtensions
    {
        public static Dictionary<string, string> ToDictionary<T>(this TranslatableEntityBase<T> entity, string language = null) where T : TranslationBase, new()
        {
            var dic = new Dictionary<string, string>();
            if (entity != null && entity.Translations.Any())
            {
                foreach (var entityTranslation in entity.Translations)
                {
                    if (!dic.ContainsKey(entityTranslation.Language) && (string.IsNullOrWhiteSpace(language) || entityTranslation.Language == language))
                    {
                        dic.Add(entityTranslation.Language, entityTranslation.Value);
                    }
                }
            }

            return dic;
        }
    }
}