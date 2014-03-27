using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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

    public class LaneController : ApiController
    {
        readonly IReadOnlyRepository _readOnlyRepository;
        readonly IWriteOnlyRepository _writeOnlyRepository;
        readonly IMappingEngine _mappingEngine;

        

        public LaneController(IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository,
            IMappingEngine mappingEngine)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
            _mappingEngine = mappingEngine;
        }

        [POST("CreateLane/{idBoard}/{accesToken}")]
        public ReturnModel CreateLane([FromBody] LaneModel model,long idBoard, string accesToken)
        {
            var account = _readOnlyRepository.First<Account>(account1 => account1.Token ==accesToken);

            ReturnModel remodel=new ReturnModel();

            if (account != null)
            {
                if (account.VerifyToken(account))
                {
                    Board board = _readOnlyRepository.GetById<Board>(idBoard);
                    Lanes lane=_mappingEngine.Map<LaneModel, Lanes>(model);
                    Lanes laneCreated = _writeOnlyRepository.Create(lane);
                    if (laneCreated != null)
                    {
                        board.AddLane(laneCreated);
                        var boardUpdate = _writeOnlyRepository.Update(board);
                        Activity activity = new Activity();
                        activity.Text = account.FirstName + "Lane creada en "+board.Title;
                        account.AddActivities(activity);
                        var accountUpdate = _writeOnlyRepository.Update(account);
                        return remodel.ConfigureModel("Successfull", "Lane agregada "+laneCreated.Title, remodel);
                    }
                    return remodel.ConfigureModel("Error", "No se pudo crear la lane", remodel);
                }
                return remodel.ConfigureModel("Error", "Su session ya expiro", remodel);
            }
            return remodel.ConfigureModel("Error", "No se pudo acceder a su cuenta", remodel);
        }

        [AcceptVerbs("GET")]
        [GET("lanes/{boardId}/{accessToken}")]
        public ReturnModel GetOrganizations(long boardId, string accessToken)
        {
            ReturnModel remodel = new ReturnModel();
            try
            {
                var account = _readOnlyRepository.First<Account>(account1 => account1.Token == accessToken);
                
                if (account != null)
                {
                    if (account.VerifyToken(account))
                    {
                        var board = _readOnlyRepository.GetById<Board>(boardId);
                        if (board != null)
                        {
                            ReturnLanesModel boardsModel = _mappingEngine.Map<Board, ReturnLanesModel>(board);
                            var lanes = new ReturnLanesModel();
                            lanes.Lanes = new List<Lanes>();
                            foreach (var or in boardsModel.Lanes)
                            {
                                if (!or.IsArchived)
                                {
                                    var o = new Lanes();
                                    o.Title = or.Title;
                                    o.Id = or.Id;
                                    lanes.Lanes.Add(o);
                                }
                            }
                            return lanes.ConfigureModel("Successfull", "", lanes);
                        }
                    }
                    return remodel.ConfigureModel("Error", "Su session ya expiro", remodel);
                }
                return remodel.ConfigureModel("Error", "No se pudo acceder a su cuenta", remodel);
            }
            catch (Exception e)
            {
                return remodel.ConfigureModel("Error", e.Message, remodel);
            }
            

            
        }
    }
}