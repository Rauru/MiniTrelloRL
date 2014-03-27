using System.Collections.Generic;
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
    public class CardController : ApiController
    {
        readonly IReadOnlyRepository _readOnlyRepository;
        readonly IWriteOnlyRepository _writeOnlyRepository;
        readonly IMappingEngine _mappingEngine;

        public CardController(IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository,
            IMappingEngine mappingEngine)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
            _mappingEngine = mappingEngine;
        }

        [POST("CreateCard/{idLine}/{accesToken}")]
        public ReturnModel CreateCard([FromBody] CardModel model,long idLine, string accesToken)
        {
            var account =
                _readOnlyRepository.First<Account>(
                    account1 => account1.Token == accesToken);

            ReturnModel remodel=new ReturnModel();
            if (account != null)
            {
                if (account.VerifyToken(account))
                {
                    Lines line = _readOnlyRepository.GetById<Lines>(idLine);
                    Cards card= _mappingEngine.Map<CardModel, Cards>(model);
                    Cards cardCreated = _writeOnlyRepository.Create(card);
                    if (cardCreated != null)
                    {
                        line.AddCard(cardCreated);
                        var lineUpdate = _writeOnlyRepository.Update(line);
                        Activity activity = new Activity();
                        activity.Text = account.FirstName + " Creo una card en " + line.Title;
                        account.AddActivities(activity);
                        var accountUpdate = _writeOnlyRepository.Update(account);
                        return remodel.ConfigureModel("Successfull", "Se creo correctamente la card "+cardCreated.Text, remodel);
                    }
                    return remodel.ConfigureModel("Error", "No se pudo crear la card", remodel);
                }
                return remodel.ConfigureModel("Error", "Su session ya expiro", remodel);
            }
            return remodel.ConfigureModel("Error", "No se pudo acceder a la cuenta", remodel);
        }

        [DELETE("deleteCard/{accesToken}")]
        public ReturnModel DeleteBoard([FromBody] CardArchiveModel model, string accesToken)
        {
            var account =_readOnlyRepository.First<Account>(account1 => account1.Token == accesToken);
            ReturnModel remodel=new ReturnModel();
            if (account != null)
            {
                if (account.VerifyToken(account))
                {
                    var card = _readOnlyRepository.GetById<Cards>(model.Id);
                    var archivedCard = _writeOnlyRepository.Archive(card);
                    Activity activity = new Activity();
                    activity.Text = account.FirstName + " elimino la card "+card.Text;
                    account.AddActivities(activity);
                    var accountUpdate = _writeOnlyRepository.Update(account);
                    CardModel cardmodel= _mappingEngine.Map<Cards, CardModel>(archivedCard);
                    return cardmodel.ConfigureModel("Successfull", "Se borro exitosamente la card "+card.Text, cardmodel);
                }
            }
            return remodel.ConfigureModel("Error", "Su session ya expiro", remodel);
            
        }

        [AcceptVerbs("PUT")]
        [PUT("card/moveCard/{accessToken}")]
        public ReturnModel MoveCardToLane([FromBody] MoveCardModel model, string accessToken)
        {
            var account = _readOnlyRepository.First<Account>(account1 => account1.Token == accessToken);
            ReturnModel remodel=new ReturnModel();
            if (account != null)
            {
                if (account.VerifyToken(account))
                {
                    var card = _readOnlyRepository.GetById<Cards>(model.Id);
                    Cards card1 =new Cards();
                    card1.Text=card.Text;
                    card1.Description = card.Description;
                    var line = _readOnlyRepository.GetById<Lines>(model.IdLineTo);

                    line.AddCard(card1);
                    var archivedCard = _writeOnlyRepository.Archive(card);
                    var updateLine = _writeOnlyRepository.Update(line);
                    Activity activity = new Activity();
                    activity.Text = account.FirstName + " Movio la card "+card.Text+" a la line: "+line.Title;
                    account.AddActivities(activity);
                    var accountUpdate = _writeOnlyRepository.Update(account);
                    return remodel.ConfigureModel("Successfull", "Se movio correctamente la card", remodel);
                }
                return remodel.ConfigureModel("Error", "Su session ya expiro", remodel);
            }
            return remodel.ConfigureModel("Error", "No se pudo acceder a su cuenta", remodel);
        }

        [AcceptVerbs("GET")]
        [GET("cards/{lineId}/{accessToken}")]
        public ReturnModel GetOrganizations(long lineId, string accessToken)
        {
            var account = _readOnlyRepository.First<Account>(account1 => account1.Token == accessToken);
            ReturnModel remodel = new ReturnModel();
            if (account != null)
            {
                if (account.VerifyToken(account))
                {
                    var line = _readOnlyRepository.GetById<Lines>(lineId);
                    if (line != null)
                    {
                        ReturnCardsModel cardsModel = _mappingEngine.Map<Lines, ReturnCardsModel>(line);
                        var cards = new ReturnCardsModel();
                        cards.Cards = new List<Cards>();
                        foreach (var or in cardsModel.Cards)
                        {
                            if (!or.IsArchived)
                            {
                                var o = new Cards();
                                o.Text = or.Text;
                                o.Id = or.Id;
                                cards.Cards.Add(o);
                            }

                        }
                        return cards.ConfigureModel("Successfull", "", cards);
                    }
                }
                return remodel.ConfigureModel("Error", "Su session ya expiro", remodel);
            }
            return remodel.ConfigureModel("Error", "No se pudo acceder a su cuenta", remodel);
        }
    }
}