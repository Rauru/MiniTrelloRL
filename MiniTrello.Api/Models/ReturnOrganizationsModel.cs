using System.Collections.Generic;
using MiniTrello.Domain.Entities;

namespace MiniTrello.Api.Models
{
    public class ReturnOrganizationsModel:ReturnModel
    {
        public List<Organization> Organizations { set; get; }
    }
}