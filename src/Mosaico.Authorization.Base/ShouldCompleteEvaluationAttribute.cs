using System;

namespace Mosaico.Authorization.Base
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class ShouldCompleteEvaluationAttribute: Attribute
    {
        
    }
}