using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AntiCafeConsoleApp.DataAccess.Repositories
{
    // Інтерфейс для загального репозиторію, який визначає базові операції з даними
    internal interface IRepository<T> where T : class
    {
        // Асинхронні методи для роботи з базою даних
        // Отримання всіх сутностей типу T
        Task<IEnumerable<T>> GetAllAsync();
        // Отримання сутності за ідентифікатором
        Task<T> GetByIdAsync(int id);
        // Додавання нової сутності
        Task AddAsync(T entity);
        // Видалення сутності
        void Remove(T entity);
        // Отримання сутностей за умовою з можливістю включення пов'язаних сутностей
        Task<IEnumerable<T>> GetByConditionAsync(
            Expression<Func<T, bool>> expression,
            string? includeProperties = null); // Дозволяє завантажувати пов'язані сутності
    }
}
