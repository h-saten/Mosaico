using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Events.DocumentManagement
{
    public record DocumentCreatedEvent(Guid Id, string Title);
    public record DocumentUpdatedEvent(Guid Id, string Title);
    public record DocumentDeletedEvent(Guid Id);
    public record DocumentContentRemovedEvent(Guid Id, Guid DocumentId);
    public record FileStoredEvent(string FileId);

    public record UserPhotoDeletedEvent(Guid UserId);
}
