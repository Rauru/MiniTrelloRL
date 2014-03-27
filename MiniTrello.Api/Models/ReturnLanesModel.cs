using System.Collections.Generic;
using MiniTrello.Domain.Entities;

namespace MiniTrello.Api.Models
{
    public class ReturnLanesModel: ReturnModel
    {
        public List<Lanes> Lanes { set; get; }
    }
}