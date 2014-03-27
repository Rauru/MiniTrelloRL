namespace MiniTrello.Api.Models
{
    public class AddMemberBoardModel
    {
        public long MemberId { get; set; }
        public long BoardId { get; set; }
    }
}