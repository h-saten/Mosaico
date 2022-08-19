using Mosaico.Domain.Base;

namespace Mosaico.Domain.Identity.Entities
{
    public class UserEvaluationQuestion : EntityBase
    {
        public string Key { get; set; }
        public string UserId { get; set; }
        public string Response { get; set; }
    }
}