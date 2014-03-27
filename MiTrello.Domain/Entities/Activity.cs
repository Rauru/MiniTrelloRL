using System.Net.Mime;

namespace MiniTrello.Domain.Entities
{
    public class Activity : IEntity
    {
        public virtual long Id { get; set; }
        public virtual bool IsArchived { get; set; }
        public virtual string Text { get; set; }
    }
}