using System.Collections.Generic;

namespace MiniTrello.Domain.Entities
{
    public class Board : IEntity
    {
        private readonly IList<Account> _members = new List<Account>();
        private readonly IList<Lines> _lines =new List<Lines>(); 
        public virtual Account Administrator { get; set; }
        public virtual string Title { get; set; }
        public virtual long Id { get; set; }
        public virtual bool IsArchived { get; set; }
        public virtual IEnumerable<Account> Members{ get { return _members; }}
        public virtual IEnumerable<Lines> Lines { get { return _lines; }}

        public virtual void AddLine(Lines line)
        {
            if (!_lines.Contains(line))
            {
                _lines.Add(line);
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

        public virtual Lines GetLineById(long ID)
        {
            foreach (var line in _lines)
            {
                if (line.Id == ID)
                    return line;

            }
            return null;
        }

        public virtual Lines GetLineByTitle(string title)
        {
            foreach (var line in _lines)
            {
                if (line.Title.Equals(title))
                    return line;
            }
            return null;
        }
    }
}