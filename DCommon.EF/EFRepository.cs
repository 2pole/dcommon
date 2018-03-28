using System;
using System.Linq;
using Common.Logging;
using DCommon.Data;

namespace DCommon.EF
{
    public class EFRepository<T> : RepositoryBase<T>
        where T : class
    {
        public ILog Logger { get; set; }

        protected virtual IEFSessionLocator Session { get; private set; }

        public EFRepository(IUnitOfWorkManager uowManager)
        {
            var uow = uowManager.CurrentUnitOfWork as IEFUnitOfWork;
            if (uow == null)
                throw new ArgumentNullException("uowManager", "The current unit of work cann't null.");

            Session = uow.GetSession<T>();
            Logger = LogManager.GetLogger(typeof(EFRepository<T>));
        }

        /// <summary>
        /// For entities with automatatically generated Ids, such as identity, SaveOrUpdate may 
        /// be called when saving or updating an entity.
        /// 
        /// Updating also allows you to commit changes to a detached object. 
        /// </summary>
        public virtual void CreateOrUpdate(T entity)
        {
            Session.CreateOrUpdate(entity);
        }

        public override void Create(T entity)
        {
            Session.Add(entity);
        }

        public override void Update(T entity)
        {
            Session.Update(entity);
        }

        public override void Delete(T entity)
        {
            Session.Delete(entity);
        }

        public override void Flush()
        {
            Session.SaveChanges();
        }

        public override T Get(object key)
        {
            return Session.Get<T>(key);
        }

        public override IQueryable<T> Table
        {
            get { return Session.GetDbSet<T>(); }
        }
    }
}
