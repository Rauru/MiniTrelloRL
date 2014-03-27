using System.Collections.Generic;
using MiniTrello.Domain.Entities;

namespace MiniTrello.Api.Models
{
    public class ReturnMembersModel
    {
        public IList<Account> Members { get; set; }
    }
}