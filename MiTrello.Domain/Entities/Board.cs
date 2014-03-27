using System.Collections.Generic;

namespace MiniTrello.Domain.Entities
{
    public class Board : IEntity
    {
        private readonly IList<Account> _members = new List<Account>();
        private readonly IList<Lanes> _lanes =new List<Lanes>(); 
        public virtual Account Administrator { get; set; }
        public virtual string Title { get; set; }
        public virtual long Id { get; set; }
        public virtual bool IsArchived { get; set; }
        public virtual IEnumerable<Account> Members{ get { return _members; }}
        public virtual IEnumerable<Lanes> Lanes { get { return _lanes; }}

        public virtual void AddLane(Lanes lane)
        {
            if (!_lanes.Contains(lane))
            {
                _lanes.Add(lane);
            }
        }

        public virtual void AddMember(Account member)
        {
            if (!_members.Contains(member))
            {
                _members.Add(member);
            }
        }

        public virtual void ChangeNameBoard(string title)
        {
            Title = title;
        }

        public virtual Lanes GetLaneById(long ID)
        {
            foreach (var lane in _lanes)
            {
                if (lane.Id == ID)
                    return lane;

            }
            return null;
        }

        public virtual Lanes GetLaneByTitle(string title)
        {
            foreach (var lane in _lanes)
            {
                if (lane.Title.Equals(title))
                    return lane;
            }
            return null;
        }
    }
}