using System.Collections.Generic;
using System.Linq;

namespace Mosaico.Domain.Base
{
    public abstract class TranslatableEntityBase<T> : EntityBase where T : TranslationBase, new()
    {
        public string Title { get; set; }
        public string Key { get; set; }
        public virtual List<T> Translations { get; set; } = new List<T>();

        public virtual List<T> GetTranslationsInLanguage(string lang = Mosaico.Base.Constants.Languages.English)
        {
            return Translations?.Where(t => t.Language == lang).ToList();
        }
        
        public virtual T GetTranslationInLanguage(string lang = Mosaico.Base.Constants.Languages.English, bool strict = false)
        {
            var translation = Translations?.FirstOrDefault(t => t.Language == lang);
            if (strict)
                return translation;
            
            if (translation == null)
            {
                translation = Translations?.FirstOrDefault(t => t.Language == Mosaico.Base.Constants.Languages.English);
            }

            if (translation == null)
            {
                translation = Translations?.FirstOrDefault();
            }

            return translation;
        }

        public virtual void UpdateTranslation(string value, string lang = Mosaico.Base.Constants.Languages.English)
        {
            var translation = Translations.FirstOrDefault(t => t.Language == lang);
            if (translation == null)
            {
                translation = new T
                {
                    Language = lang,
                    Value = value,
                    EntityId = Id
                };
                Translations.Add(translation);
            }
            else
            {
                translation.Value = value;
            }
        }
    }
}