using System;

namespace Mosaico.Domain.Base
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}