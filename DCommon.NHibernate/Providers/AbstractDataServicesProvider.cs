using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Common.Logging;
using NHibernate.Cfg;
using NHibernate.Cfg.Loquacious;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using DCommon.Environment.Configuration;
using Cfg = NHibernate.Cfg.Configuration;

namespace DCommon.NHibernate.Providers
{
    public abstract class AbstractDataServicesProvider : IDataServicesProvider
    {
        public ILog Log { get; set; }

        public AbstractDataServicesProvider()
        {
            this.Log = LogManager.GetLogger<AbstractDataServicesProvider>();
        }

        public virtual Cfg BuildConfiguration(ShellSetting setting)
        {
            var nhibernateConfig = new Cfg();
            nhibernateConfig.DataBaseIntegration(ConfigureProperties);
            AddMappings(nhibernateConfig);
            bool exists = DatabaseExists(setting);
            if (!exists)
            {
                this.CreateDatabase(nhibernateConfig);
            }

            return nhibernateConfig;
        }

        protected abstract bool DatabaseExists(ShellSetting setting);

        protected virtual void ConfigureProperties(IDbIntegrationConfigurationProperties dbIntegration)
        {
        }

        protected virtual void CreateDatabase(Cfg configuration)
        {
            var schema = new SchemaExport(configuration);
            schema.Create(false, true);
        }

        protected virtual void DropDatabase(Cfg configuration)
        {
            new SchemaExport(configuration).Drop(false, true);
        }

        protected virtual void AddMappings(Cfg configuration)
        {
            var modelMapper = new ModelMapper();
            modelMapper.AddMappings(MappingTable.Mappings);
            var hbmMapping = modelMapper.CompileMappingForAllExplicitlyAddedEntities();
            configuration.AddMapping(hbmMapping);
            this.LogMapping(hbmMapping);
        }

        private void LogMapping(HbmMapping hbmMapping)
        {
            if (Log.IsDebugEnabled)
            {
                var log = string.Format("{0}{1}{2}{1}{0}",
                                    "~~~~~~~~~~~~~~~hbm.xml~~~~~~~~~~~~~~~",
                                    System.Environment.NewLine,
                                    hbmMapping.AsString());
                Log.Debug(log);
            }
        }
    }
}