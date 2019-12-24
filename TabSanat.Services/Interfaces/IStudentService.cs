using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TabSanat.Model;

namespace TabSanat.Services.Interfaces
{
    public interface IStudentService
    {
        Task<Student> GetAsync(Expression<Func<Student, bool>> predicate, params Expression<Func<Student, object>>[] includes);
        Task<IQueryable<Student>> GetAllAsync(Expression<Func<Student, bool>> filter = null, Func<IQueryable<Student>, IOrderedQueryable<Student>> orderBy = null, params Expression<Func<Student, object>>[] includes);
        void SaveToDatabase(Student student);
        void RemoveFromDatabase(Student student);
        bool StudentExists(Guid id);
        void DeleteStudentsPhoto(string photoPath, string webRootPath);

    }
}
