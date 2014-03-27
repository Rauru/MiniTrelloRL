using System.Collections.Generic;

namespace MiniTrello.Domain.Entities
{
    public class Lines: IEntity
    {
        private readonly IList<Cards> _cards = new List<Cards>();
        public virtual Board Inboard { get; set; }
        public virtual string Title { get; set; }
        public virtual long Id { get; set;}
        public virtual bool IsArchived { get;set; }

        public virtual IEnumerable<Cards> Cards { get { return _cards; } }

        public virtual void AddCard(Cards cards)
        {
            if (!_cards.Contains(cards))
            {
                _cards.Add(cards);
            }
        }

        public virtual Cards GetCardById(long ID)
        {
            foreach (var card in _cards)
            {
                if (card.Id == ID)
                    return card;

            }
            return null;
        }
    }
}
