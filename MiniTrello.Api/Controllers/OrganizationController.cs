using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting.Web.Http;
using AutoMapper;
using MiniTrello.Api.Models;
using MiniTrello.Domain.Entities;
using MiniTrello.Domain.Services;

namespace MiniTrello.Api.Controllers
{
    public class OrganizationController : ApiController
    {
        readonly IReadOnlyRepository _readOnlyRepository;
        readonly IWriteOnlyRepository _writeOnlyRepository;
        readonly IMappingEngine _mappingEngine;

        public OrganizationController(IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository,
            IMappingEngine mappingEngine)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
            _mappingEngine = mappingEngine;
        }


        [POST("createorganization/{accesToken}")]
        public ReturnModel Register([FromBody] OrganizationModel model,string accesToken)
        {
            var account =
                _readOnlyRepository.First<Account>(
                    account1 => account1.Token ==accesToken);
            ReturnModel remodel=new ReturnModel();
            if (account != null)
            {
                if (account.VerifyToken(account))
                {
                    Organization organization = _mappingEngine.Map<OrganizationModel, Organization>(model);
                    Organization organizationCreated = _writeOnlyRepository.Create(organization);
                    if (organizationCreated != null)
                    {
                        account.AddOrganization(organizationCreated);
                        Activity activity=new Activity();
                        activity.Text = account.FirstName + " Creo una organizacion";
                        account.AddActivities(activity);
                        var accountUpdate = _writeOnlyRepository.Update(account);

                        return remodel.ConfigureModel("SuccessFull", "Se Creo satisfactoriamente la organizacion "+organizationCreated.Title, remodel); ;
                    }
                }
                return remodel.ConfigureModel("Error", "Su session ya expiro", remodel); 
            }
            return remodel.ConfigureModel("Error", "No se pudo acceder a su cuenta", remodel);
        }

        [DELETE("organization/{accesToken}")]
        public ReturnModel Archive([FromBody] OrganizationArchiveModel model, string accesToken)
        {
            var account =_readOnlyRepository.First<Account>(account1 => account1.Token == accesToken);
            ReturnModel remodel=new ReturnModel();
            if (account != null)
            {
                if (account.VerifyToken(account))
                {
                    var organization = _readOnlyRepository.GetById<Organization>(model.Id);
                    var archiveOrganization = _writeOnlyRepository.Archive(organization);
                    ReturnOrganizationModel organizationmodel = new ReturnOrganizationModel();
                    organizationmodel = _mappingEngine.Map<Organization, ReturnOrganizationModel>(archiveOrganization);
                    return organizationmodel.ConfigureModel("Successfull", "Se elimino satisfactoriamente " + organization.Title, organizationmodel);
                }
                return remodel.ConfigureModel("Error", "Su session ya expiro", remodel); 
            }
            return remodel.ConfigureModel("Error", "No se pudo acceder a su cuenta", remodel);
        }

        [AcceptVerbs("GET")]
        [GET("organizations/{accessToken}")]
        public ReturnModel GetOrganizations(string accessToken)
        {
            var account = _readOnlyRepository.First<Account>(account1 => account1.Token == accessToken);
            ReturnModel remodel = new ReturnModel();
            if (account != null)
            {
                if (account.VerifyToken(account))
                {
                    ReturnOrganizationsModel organizationsmodel = _mappingEngine.Map<Account, ReturnOrganizationsModel>(account);
                    ReturnOrganizationsModel organ = new ReturnOrganizationsModel();
                    organ.Organizations = new List<Organization>();
                    foreach (var or in organizationsmodel.Organizations)
                    {
                        if (!or.IsArchived)
                        {
                            Organization o = new Organization();
                            o.Title = or.Title;
                            o.Id = or.Id;
                            organ.Organizations.Add(o);
                        }
                    }
                    return organ.ConfigureModel("Successfull", "", organ);
                }
                return remodel.ConfigureModel("Error", "Su session ya expiro", remodel);
            }
            return remodel.ConfigureModel("Error", "No se pudo acceder a su cuenta", remodel);
        }
    }
}