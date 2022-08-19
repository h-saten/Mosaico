using Microsoft.EntityFrameworkCore;

namespace Mosaico.Domain.Base.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void AddTranslatableEntity<TE, T>(this ModelBuilder builder) where TE : TranslatableEntityBase<T> where T : TranslationBase, new()
        {
            builder.Entity<TE>().HasMany(c => c.Translations).WithOne().HasForeignKey(t => t.EntityId);
        }
    }
}