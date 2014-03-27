using AcklenAvenue.Data.NHibernate;
using Autofac;
using FluentNHibernate.Cfg.Db;
using MiniTrello.Data;
using NHibernate;

namespace MiniTrello.Infrastructure
{
    public class ConfigureDatabase : IBootstrapperTask
    {
        readonly ContainerBuilder _container;

        public ConfigureDatabase(ContainerBuilder containerBuilder)
        {
            _container = containerBuilder;
        }

        #region IBootstrapperTask Members

        public void Run()
        {
            MsSqlConfiguration databaseConfiguration = MsSqlConfiguration.MsSql2008.ShowSql().
                ConnectionString(x => x.FromConnectionStringWithKey("MiniTrello.Remote"));

            _container.Register(c => c.Resolve<ISessionFactory>().OpenSession()).As
                <ISession>()
                .InstancePerLifetimeScope()
                .OnActivating(c =>
                {
                    if (!c.Instance.Transaction.IsActive)
                        c.Instance.BeginTransaction();
                }
                )
                .OnRelease(c =>
                {
                    if (c.Transaction.IsActive)
                    {
                        c.Transaction.Commit();
                    }
                    c.Dispose();
                });

            _container.Register(c =>
                new SessionFactoryBuilder(new MappingScheme(), databaseConfiguration).Build())
                .SingleInstance()
                .As<ISessionFactory>();
        }

        #endregion
    }
}