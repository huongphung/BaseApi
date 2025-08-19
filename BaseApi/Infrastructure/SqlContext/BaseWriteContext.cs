using Application.Interface.Databases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SqlContext
{
    public class BaseWriteContext : BaseContext<BaseWriteContext>, ICasperWriteContext
    {
        public BaseWriteContext(DbContextOptions<BaseWriteContext> options) : base(options)
        {
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
