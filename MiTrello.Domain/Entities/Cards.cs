namespace MiniTrello.Domain.Entities
{
    public class Cards : IEntity
    {
        public virtual string Text { get; set; }
        public virtual string Description { get; set; }
        public virtual Lines InLine { get; set; }
        public virtual long Id { get; set; }
        public virtual bool IsArchived { get; set; }
    }
}