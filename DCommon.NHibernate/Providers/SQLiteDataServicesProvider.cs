using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using NHibernate.Cfg;
using NHibernate.Cfg.Loquacious;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.SqlTypes;
using DCommon.Environment.Configuration;
using Cfg = NHibernate.Cfg.Configuration;
using System.Data.Common;

namespace DCommon.NHibernate.Providers
{
    public class SQLiteDataServicesProvider : AbstractDataServicesProvider
    {
        private const string DatabaseFilename = "content.db";
        protected string DatabaseFullname { get; private set; }

        public static string ProviderName
        {
            get { return "SQLite"; }
        }

        protected override bool DatabaseExists(ShellSetting setting)
        {
            return File.Exists(DatabaseFullname);
        }

        public override Cfg BuildConfiguration(ShellSetting setting)
        {
            DatabaseFullname = Path.Combine(setting.ShellPath, DatabaseFilename);
            var folder = Path.GetDirectoryName(DatabaseFullname);
            if(!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            return base.BuildConfiguration(setting);
        }

        protected override void ConfigureProperties(IDbIntegrationConfigurationProperties dbIntegration)
        {
            base.ConfigureProperties(dbIntegration);

            string localConnectionString = string.Format("Data Source={0}", DatabaseFullname);
            dbIntegration.ConnectionString = localConnectionString;
            dbIntegration.Driver<SQLite20Driver>();
            dbIntegration.Dialect<SQLiteDialect>();
        }

        public class SQLiteDriver : SQLite20Driver
        {
            private PropertyInfo _dbParamSqlDbTypeProperty;

            public override void Configure(IDictionary<string, string> settings)
            {
                base.Configure(settings);
                using (var cmd = CreateCommand())
                {
                    var dbParam = cmd.CreateParameter();
                    _dbParamSqlDbTypeProperty = dbParam.GetType().GetProperty("SqlDbType");
                }
            }

            protected override void InitializeParameter(DbParameter dbParam, string name, SqlType sqlType)
            {
                base.InitializeParameter(dbParam, name, sqlType);
                if (sqlType.Length <= 4000)
                {
                    switch (sqlType.DbType)
                    {
                        case DbType.String:
                            _dbParamSqlDbTypeProperty.SetValue(dbParam, SqlDbType.NVarChar, null);
                            break;
                        case DbType.AnsiString:
                            _dbParamSqlDbTypeProperty.SetValue(dbParam, SqlDbType.NVarChar, null);
                            break;
                    }
                }
                else
                {
                    switch (sqlType.DbType)
                    {
                        case DbType.String:
                            _dbParamSqlDbTypeProperty.SetValue(dbParam, SqlDbType.NText, null);
                            break;
                        case DbType.AnsiString:
                            _dbParamSqlDbTypeProperty.SetValue(dbParam, SqlDbType.Text, null);
                            break;
                        case DbType.Byte:
                            _dbParamSqlDbTypeProperty.SetValue(dbParam, SqlDbType.Image, null);
                            break;
                    }
                }
            }
        }
    }
}
