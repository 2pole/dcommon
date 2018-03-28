using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Common.Logging;
using DCommon.Data;
using DCommon.Utility;
using NHibernate;
using NHibernate.Linq;

namespace DCommon.NHibernate
{
    public class NHRepository<T> : RepositoryBase<T>
        where T : class
    {
        public ILog Logger { get; set; }

        public ISession Session { get; private set; }

        public NHRepository(IUnitOfWorkManager uowManager)
        {
            Guard.Against<ArgumentNullException>(uowManager == null, "The unit of work manager cann't null.");
            var uow = uowManager.CurrentUnitOfWork as INHUnitOfWork;
            Guard.Against<ArgumentNullException>(uow == null, "The unit of work cann't null.");
           
            Session = uow.GetSession<T>();
            Logger = LogManager.GetLogger(typeof(NHRepository<T>));
        }

        public override IQueryable<T> Table
        {
            get { return Session.Query<T>(); }
        }

        public override T Get(object key)
        {
            return Session.Get<T>(key);
        }

        public override void Create(T entity)
        {
            Logger.DebugFormat("Create {0}", entity);
            Session.Save(entity);
        }

        public override void Update(T entity)
        {
            Logger.DebugFormat("Update {0}", entity);
            var mergedEntity = Session.Merge(entity);
            Session.Update(mergedEntity);
        }

        public override void Delete(T entity)
        {
            Logger.DebugFormat("Delete {0}", entity);
            Session.Delete(entity);
        }

        //public override void Copy(T source, T target)
        //{
        //    Logger.DebugFormat("Copy {0} {1}", source, target);
        //    var metadata = Session.SessionFactory.GetClassMetadata(typeof(T));
        //    var values = metadata.GetPropertyValues(source, EntityMode.Poco);
        //    metadata.SetPropertyValues(target, values, EntityMode.Poco);
        //}

        public override void Flush()
        {
            Session.Flush();
        }
    }
}