using System.Collections.Generic;
using MiniTrello.Domain.Entities;

namespace MiniTrello.Api.Models
{
    public class ReturnCardsModel:ReturnModel
    {
        public List<Cards> Cards { set; get; }
    }
}