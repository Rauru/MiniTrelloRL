using System.Collections.Generic;
using MiniTrello.Domain.Entities;

namespace MiniTrello.Api.Models
{
    public class ReturnLinesModel: ReturnModel
    {
        public List<Lines> Lines { set; get; }
    }
}