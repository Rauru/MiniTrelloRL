using MiniTrello.Api.Controllers;

namespace MiniTrello.Api.Models
{
    public class AccountBoardsModel:ReturnModel
    {
        public string Title;
        public bool IsArchived { get; set; }
    }
}