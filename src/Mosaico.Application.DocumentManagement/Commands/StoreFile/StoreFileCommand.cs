using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Application.DocumentManagement.Commands.StoreFile
{
    public class StoreFileCommand : IRequest<string>
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
    }
}
