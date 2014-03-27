using System.Collections.Generic;
using System.Linq;
using DomainDrivenDatabaseDeployer;
using FizzWare.NBuilder;
using MiniTrello.Domain.Entities;
using NHibernate;

namespace MiniTrello.DatabaseDeployer
{
    public class AccountSeeder : IDataSeeder
    {
        readonly ISession _session;

        public AccountSeeder(ISession session)
        {
            _session = session;
        }

        public void Seed()
        {
            IList<Account> accountList = Builder<Account>.CreateListOfSize(10).Build();
            accountList.ElementAt(0).Email ="siwady0908@gmail.com";
            accountList.ElementAt(0).Password = "tL7nS3PhT2UABFJxFZhHBw==";

            foreach (Account account in accountList)
            {
                var organizations = Builder<Organization>.CreateListOfSize(2).Build();
                foreach (var organization in organizations)
                {
                    _session.Save(organization);
                }
                account.AddOrganization(organizations[0]);
                account.AddOrganization(organizations[1]);
                _session.Save(account);
            }
        }
    }
}