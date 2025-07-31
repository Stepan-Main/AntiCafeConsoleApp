using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AntiCafeConsoleApp.DataAccess.Repositories
{
    // Репозиторій для роботи з базою даних
    internal class GenericRepository<T> : IRepository<T> where T : class
    {
        // Контекст бази даних та набір даних для роботи з сутностями
        protected readonly AppDbContext _context;
        // Набір даних для роботи з сутностями типу T
        protected readonly DbSet<T> _dbSet;

        // Конструктор, який приймає контекст бази даних
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        // Асинхронні методи для роботи з базою даних
        // Отримання всіх сутностей типу T
        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
        // Отримання сутності за ідентифікатором
        public async Task<T> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
        // Додавання нової сутності
        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
        // Видалення сутності
        public void Remove(T entity) => _dbSet.Remove(entity);

        // Отримання сутностей за умовою з можливістю включення пов'язаних сутностей
        public async Task<IEnumerable<T>> GetByConditionAsync(
            // Умова для фільтрації сутностей
            Expression<Func<T, bool>> expression,
            string? includeProperties = null)
        {
            // Перевірка наявності умови
            IQueryable<T> query = _dbSet.Where(expression);

            if (includeProperties != null) // Якщо вказані пов'язані сутності для включення
            {
                // Розділення рядка на окремі властивості для включення
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty); // Додаємо включення для кожної властивості
                }
            }

            // Повертаємо список сутностей, які відповідають умові
            return await query.ToListAsync();
        }
    }
}
