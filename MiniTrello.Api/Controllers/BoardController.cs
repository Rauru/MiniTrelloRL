using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting.Web.Http;
using AutoMapper;
using MiniTrello.Domain.Entities;
using MiniTrello.Domain.Services;
using MiniTrello.Api.Models;

namespace MiniTrello.Api.Controllers
{
    public class BoardController : ApiController
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IWriteOnlyRepository _writeOnlyRepository;
        private readonly IMappingEngine _mappingEngine;



        public BoardController(IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository,
            IMappingEngine mappingEngine)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
            _mappingEngine = mappingEngine;
        }

        [POST("CreateBoard/{idOrganization}/{accesToken}")]
        public ReturnModel CreateBoard([FromBody] AccountBoardsModel model,long idOrganization,string accesToken)
        {
            var account =_readOnlyRepository.First<Account>(account1 => account1.Token == accesToken);
            ReturnModel remodel=new ReturnModel();
            if (account != null)
            {
                if (account.VerifyToken(account))
                {
                    Organization organization = _readOnlyRepository.GetById<Organization>(idOrganization);
                    Board board = _mappingEngine.Map<AccountBoardsModel, Board>(model);
                    board.Administrator = account;
                    Board boardCreated = _writeOnlyRepository.Create(board);
                    if (boardCreated != null)
                    {
                        organization.AddBoard(boardCreated);
                        Activity activity = new Activity();
                        activity.Text = account.FirstName + " Creo un board";
                        account.AddActivities(activity);
                        var organizacionUpdate = _writeOnlyRepository.Update(organization);
                        var accountUpdate = _writeOnlyRepository.Update(account);
                        return remodel.ConfigureModel("Succesfull", "Se creo el board correctamente", remodel);
                    }
                    return remodel.ConfigureModel("Error", "No se pudo crear el board", remodel);
                }
                return remodel.ConfigureModel("Error", "Su session ya expiro", remodel);
            }
            return remodel.ConfigureModel("Error", "No se pudo acceder a su cuenta", remodel);
        }

        [AcceptVerbs("PUT")]
        [PUT("boards/addmember/{accesToken}")]
        public ReturnModel AddMember([FromBody] AddMemberBoardModel model, string accesToken)
        {
            var account = _readOnlyRepository.First<Account>(account1 => account1.Token == accesToken);
            ReturnModel remodel=new ReturnModel();
            if (account.VerifyToken(account))
            {
                var memberToAdd = _readOnlyRepository.GetById<Account>(model.MemberId);
                var board = _readOnlyRepository.GetById<Board>(model.BoardId);
                board.AddMember(memberToAdd);
                var updateBoard = _writeOnlyRepository.Update(board);
                var boardModel = _mappingEngine.Map<Board, AccountBoardsModel>(updateBoard);
                Activity activity=new Activity();
                activity.Text = account.FirstName + " Agrego a " + memberToAdd.FirstName + " al board: " + board.Title;
                account.AddActivities(activity);
                return boardModel;
            }
            return remodel.ConfigureModel("Error", "Su session ya expiro", remodel);
        }

        [AcceptVerbs("PUT")]
        [PUT("boards/changeName/{titleOrganization}/{accesToken}")]
        public ReturnModel ChangeTitleBoard([FromBody] ChangeBoardsTitleModel model,string titleOrganization ,string accesToken)
        {
            var account = _readOnlyRepository.First<Account>(account1 => account1.Token == accesToken);
            ReturnModel remodel=new ReturnModel();
            if (account != null)
            {
                if (account.VerifyToken(account))
                {
                    var board = _readOnlyRepository.GetById<Board>(model.Id);
                    board.Title = model.NewTitle;
                    if (board.Administrator.Token.Equals(account.Token))
                    {
                        var boardUpdate = _writeOnlyRepository.Update(board);
                        var boardModel = _mappingEngine.Map<Board, AccountBoardsModel>(boardUpdate);
                        Activity activity = new Activity();
                        activity.Text = account.FirstName + " renombro el board a  "+ board.Title;
                        account.AddActivities(activity);
                        var accountUpdate = _writeOnlyRepository.Update(account);
                        return remodel.ConfigureModel("Successfull", "Se cambio el nombre del board correctamente", remodel);
                    }
                    return remodel.ConfigureModel("Error", "No es el administrador no puede cambiar el nombre", remodel);
                }
                return remodel.ConfigureModel("Error", "Su session ya expiro", remodel);
            }
            return remodel.ConfigureModel("Error", "No se pudo acceder a su cuenta", remodel);
            
        }

        [DELETE("boards/deleteBoard/{accesToken}")]
        public ReturnModel DeleteBoard([FromBody] BoardArchiveModel model,string accesToken)
        {
            var account = _readOnlyRepository.First<Account>(account1 => account1.Token == accesToken);
            ReturnModel remodel=new ReturnModel();
            if (account != null)
            {
                if (account.VerifyToken(account))
                {
                    var board = _readOnlyRepository.GetById<Board>(model.Id);
                    var archivedBoard = _writeOnlyRepository.Archive(board);
                    Activity activity = new Activity();
                    activity.Text = account.FirstName + " elimino el board: "+board.Title;
                    account.AddActivities(activity);
                    var accountUpdate = _writeOnlyRepository.Update(account);
                    AccountBoardsModel boardmodel=_mappingEngine.Map<Board, AccountBoardsModel>(archivedBoard);
                    return boardmodel.ConfigureModel("Successfull", "Se Borro el board correctamente", boardmodel);
                }
            }
            return remodel.ConfigureModel("Error", "Su session ya expiro", remodel);
        }

        [AcceptVerbs("GET")]
        [GET("boards/members/{idBoard}/{accesToken}")]
        public ReturnMembersModel MembersList(long idBoard, string accesToken)
        {
            var board = _readOnlyRepository.GetById<Board>(idBoard);
            return _mappingEngine.Map<Board, ReturnMembersModel>(board);
        }

        [AcceptVerbs("GET")]
        [GET("boards/{organizationId}/{accessToken}")]
        public ReturnModel GetOrganizations(long organizationId, string accessToken)
        {
            var account = _readOnlyRepository.First<Account>(account1 => account1.Token == accessToken);
            ReturnModel remodel = new ReturnModel();
            if (account != null)
            {
                if (account.VerifyToken(account))
                {
                    var organization = _readOnlyRepository.GetById<Organization>(organizationId);
                    if (organization != null)
                    {
                        ReturnBoardsModel boardsModel = _mappingEngine.Map<Organization, ReturnBoardsModel>(organization);
                        var boards = new ReturnBoardsModel();
                        boards.Boards = new List<Board>();
                        foreach (var or in boardsModel.Boards)
                        {
                            if (!or.IsArchived)
                            {
                                var o = new Board();
                                //o.Administrator = or.Administrator;
                                o.Title = or.Title;
                                o.Id = or.Id;
                                boards.Boards.Add(o);
                            }

                        }
                        return boards.ConfigureModel("Successfull", "", boards);
                    }
                }
                return remodel.ConfigureModel("Error", "Su session ya expiro", remodel);
            }
            return remodel.ConfigureModel("Error", "No se pudo acceder a su cuenta", remodel);
        }
    }
}
