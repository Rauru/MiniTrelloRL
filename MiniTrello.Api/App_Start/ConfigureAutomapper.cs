using System;
using AutoMapper;
using MiniTrello.Api.Controllers;
using MiniTrello.Api.Models;
using MiniTrello.Domain.Entities;
using MiniTrello.Infrastructure;

namespace MiniTrello.Api
{
    public class ConfigureAutomapper : IBootstrapperTask
    {
        public void Run()
        {
            Mapper.CreateMap<Account, AccountLoginModel>().ReverseMap();
            Mapper.CreateMap<Account, AccountRegisterModel>().ReverseMap();
            Mapper.CreateMap<OrganizationModel, Organization>().ReverseMap();
            Mapper.CreateMap<AccountBoardsModel, Board>().ReverseMap();
            Mapper.CreateMap<LineModel, Lines>().ReverseMap();
            Mapper.CreateMap<CardModel, Cards>().ReverseMap();
            Mapper.CreateMap<ReturnMembersModel, Board>().ReverseMap();
            Mapper.CreateMap<ReturnBoardsModel,Organization>().ReverseMap();
            Mapper.CreateMap<Account, ReturnActivitiesModel>().ReverseMap();
            Mapper.CreateMap<ReturnOrganizationModel, Organization>().ReverseMap();
            Mapper.CreateMap<ReturnUpdateDataModel, Account>().ReverseMap();
            Mapper.CreateMap<ReturnRegisterModel, Account>().ReverseMap();
            Mapper.CreateMap<ReturnOrganizationsModel, Account>().ReverseMap();
            Mapper.CreateMap<ReturnLinesModel, Board>().ReverseMap();
            Mapper.CreateMap<ReturnCardsModel, Lines>().ReverseMap();
            //Mapper.CreateMap<DemographicsEntity, DemographicsModel>().ReverseMap();
            //Mapper.CreateMap<IReportEntity, IReportModel>()
            //    .Include<DemographicsEntity, DemographicsModel>();
        }
    }
}