using System.Collections.Generic;

namespace MiniTrello.Domain.Entities
{
    public class Organization: IEntity
    {
        public virtual long Id { get; set; }
        public virtual bool IsArchived { get; set; }
        public virtual string Title { get; set; }
        private readonly IList<Board> _boards = new List<Board>();
        public virtual IEnumerable<Board> Boards{get { return _boards; }}
        public virtual void AddBoard(Board board)
        {
            if (!_boards.Contains(board))
            {
                _boards.Add(board);
            }
        }

        public virtual Board GetBoardById(long ID)
        {
            foreach (var board in _boards)
            {
                if (board.Id == ID)
                    return board;

            }
            return null;
        }

        public virtual Board GetBoardByTitle(string title)
        {
            foreach (var board in _boards)
            {
                if (board.Title.Equals(title))
                    return board;
            }
            return null;
        }
    }
}