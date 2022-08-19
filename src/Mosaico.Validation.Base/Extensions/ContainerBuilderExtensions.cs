using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;
using FluentValidation;
using System.Linq;
using System.Reflection;

namespace Mosaico.Validation.Base.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterAssemblyValidators(this ContainerBuilder builder, Assembly assembly)
        {
            return builder.RegisterAssemblyTypes(assembly)
            .Where(t => t.IsAssignableTo(typeof(IValidator)) || t.IsClosedTypeOf(typeof(IValidator<>)))
            .AsImplementedInterfaces();
        }
    }
}
