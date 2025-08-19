using Application.Interface.Databases;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SqlContext
{
    public class BaseReadContext : BaseContext<BaseReadContext>, ICasperReadContext
    {
        public BaseReadContext(DbContextOptions<BaseReadContext> options) : base(options)
        {

        }

        public async Task<List<T>> QueryAsync<T>(string sql, params object[] parameters) where T : class
        {
            return await Set<T>().FromSqlRaw(sql, parameters).ToListAsync();
        }
    }
}
