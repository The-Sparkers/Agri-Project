using Autofac;
using Autofac.Core;
using EFarmer.Connections;
using EFarmerPkModelLibrary.Repositories;
using EntityGrabber;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFarmerPkModelLibrary.Factories
{
    public class RepositoryFactory
    {
        private readonly BuildOptions options;
        public RepositoryFactory(BuildOptions options)
        {
            this.options = options;
        }
        public IContainer Build()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<SQLConnection>()
                .As<IDbConnection>()
                .WithParameter("connectionString", options.ConnectionString);
            builder.RegisterType<UserRepository>()
                .As<IUserRepository>();
            builder.RegisterType<CityRepository>()
                .As<ICityRepository>();

            return builder.Build();
        }
        public class BuildOptions
        {
            public string ConnectionString { get; set; }
        }
    }
}
