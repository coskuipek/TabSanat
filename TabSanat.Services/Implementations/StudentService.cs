using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TabSanat.Dal.Repositories.Interfaces;
using TabSanat.Model;
using TabSanat.Services.Interfaces;

namespace TabSanat.Services.Implementations
{
    public class StudentService :IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public void DeleteStudentsPhoto(string photoPath, string webRootPath)
        {
            if (photoPath != null)
            {
                var filePath = Path.Combine(webRootPath, "images", photoPath);
                File.Delete(filePath);
            }
        }

        public async Task<IQueryable<Student>> GetAllAsync(Expression<Func<Student, bool>> filter = null, Func<IQueryable<Student>, IOrderedQueryable<Student>> orderBy = null, params Expression<Func<Student, object>>[] includes)
        {
            return await _studentRepository.GetAllAsync(filter, orderBy, includes);
        }

        public async Task<Student> GetAsync(Expression<Func<Student, bool>> predicate, params Expression<Func<Student, object>>[] includes)
        {
            return await _studentRepository.GetFirstOrDefaultAsync(predicate, includes);
        }

        public void RemoveFromDatabase(Student student)
        {
            _studentRepository.RemoveEntity(student);
        }

        public void SaveToDatabase(Student student)
        {
            _studentRepository.Add(student);
        }

        public bool StudentExists(Guid id)
        {
            return _studentRepository.GetAll().Any(e => e.Id == id);
        }

    }
}
