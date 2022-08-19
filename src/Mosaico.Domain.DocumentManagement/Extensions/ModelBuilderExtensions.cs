using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Domain.DocumentManagement.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void ApplyProjectManagementConfiguration(this ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(ModelBuilderExtensions).Assembly);
        }
    }
}
