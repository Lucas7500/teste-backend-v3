using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System.Linq.Expressions;
using TheatricalPlayersRefactoringKata.Domain.Models;

namespace TheatricalPlayersRefactoringKata.Infrastructure.Repositories
{
    public class Repository<TEntity> : IDisposable, IRepository<TEntity> where TEntity : class
    {
        private readonly Lazy<IDocumentSession> _session = new(DocumentStoreHolder.Store.OpenSession);

        private IDocumentSession Session => _session.Value;

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? predicate = null)
        {
            var query = predicate != null
                ? Session.Query<TEntity>().Where(predicate)
                : Session.Query<TEntity>();

            return query.Customize(x => x.NoTracking()).ToList();
        }

        public TEntity? Get(string? id)
        {
            return Session.Load<TEntity>(id);
        }

        public TEntity? Get(Expression<Func<TEntity, bool>> predicate)
        {
            return Session.Query<TEntity>().Where(predicate).FirstOrDefault();
        }

        public void Add(TEntity entity)
        {
            Session.Store(entity);
        }

        public void Update(string? id, TEntity entity)
        {
            _ = Get(id) ?? throw new InvalidOperationException(string.Format("Entity with Id {0} was not found!", id));
            
            Session.Store(entity);
            Session.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            Session.Delete(entity);
        }

        public void Attach(TEntity entity, AttachmentFile file)
        {
            Session.Advanced.Attachments.Store(entity, file.Name, file.Stream, file.ContentType);
        }

        public AttachmentFile? GetAttachmentFor(TEntity document)
        {
            var attachmentName = Session.Advanced.Attachments.GetNames(document).FirstOrDefault();

            if (attachmentName == null)
            {
                return null;
            }

            var attachment = Session.Advanced.Attachments.Get(document, attachmentName.Name);

            return new(attachment.Details.Name, attachment.Stream, attachment.Details.ContentType);
        }

        public void Commit()
        {
            Session.SaveChanges();
        }

        public void Dispose()
        {
            if (_session.IsValueCreated)
            {
                Session.Dispose();
            }

            GC.SuppressFinalize(this);
        }
    }
}
