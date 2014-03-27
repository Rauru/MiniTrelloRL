using System.Collections.Generic;
using MiniTrello.Domain.Entities;

namespace MiniTrello.Api.Models
{
    public class ReturnBoardsModel: ReturnModel
    {
        public List<Board> Boards { get; set; }
    }
}