using System.Collections;
using System.Collections.Generic;
using MiniTrello.Domain.Entities;

namespace MiniTrello.Api.Models
{
    public class ReturnActivitiesModel:ReturnModel
    {
        public IList<Activity> Activities { get; set; }
    }
}