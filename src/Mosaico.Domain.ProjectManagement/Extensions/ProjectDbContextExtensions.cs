using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Exceptions;

namespace Mosaico.Domain.ProjectManagement.Extensions
{
    public static class ProjectDbContextExtensions
    {
        public static async Task<Project> GetProjectOrThrowAsync(this IProjectDbContext context, Guid identifier,
            CancellationToken tok = new CancellationToken())
        {
            var project = await context.Projects.FirstOrDefaultAsync(p => p.Id == identifier, tok);
            if (project == null)
            {
                throw new ProjectNotFoundException(identifier);
            }

            return project;
        }
        
        public static async Task<Project> GetProjectOrThrowAsync(this IProjectDbContext context, string identifier, CancellationToken tok = new CancellationToken())
        {
            Project project;
            
            if (string.IsNullOrWhiteSpace(identifier))
            {
                throw new ProjectNotFoundException(identifier);
            }
            
            if (Guid.TryParse(identifier, out var projectId))
            {
                project = await context.Projects.FirstOrDefaultAsync(p => p.Id == projectId, tok);
            }
            else
            {
                var comparableId = identifier.Trim().ToUpperInvariant();
                project = await context.Projects.FirstOrDefaultAsync(p => p.SlugInvariant == comparableId, tok);
            }

            if (project == null)
            {
                throw new ProjectNotFoundException(identifier);
            }

            return project;
        }
        
        public static async Task<Project> GetProjectWithStagesOrThrowAsync(this IProjectDbContext context, string identifier, CancellationToken tok = new CancellationToken())
        {
            Project project;
            if (Guid.TryParse(identifier, out var projectId))
            {
                project = await context.Projects
                    .Include(p => p.Stages)
                    .FirstOrDefaultAsync(p => p.Id == projectId, tok);
            }
            else
            {
                project = await context.Projects
                    .Include(p => p.Stages)
                    .FirstOrDefaultAsync(p => p.Title == identifier, tok);
            }

            if (project == null)
            {
                throw new ProjectNotFoundException(identifier);
            }

            return project;
        }
    }
}