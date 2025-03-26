using System.Linq.Expressions;
using TheatricalPlayersRefactoringKata.Domain.Models;

namespace TheatricalPlayersRefactoringKata.Infrastructure.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? predicate = null);
        TEntity? Get(Expression<Func<TEntity, bool>> predicate);
        TEntity? Get(string? id);
        void Add(TEntity entity);
        void Update(string? id, TEntity entity);
        void Delete(TEntity entity);
        void Attach(TEntity entity, AttachmentFile file);
        AttachmentFile? GetAttachmentFor(TEntity document);
        void Commit();
    }
}
