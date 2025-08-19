using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Databases
{
    public interface ICasperReadContext : ICasperContext
    {
        Task<List<T>> QueryAsync<T>(string sql, params object[] parameters) where T : class;
    }
}
