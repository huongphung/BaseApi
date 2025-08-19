using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SqlContext
{
    public partial class BaseContext<TContext> : DbContext where TContext : BaseContext<TContext>
    {
        public BaseContext(DbContextOptions<TContext> options)
        : base(options)
        {
        }

        public virtual DbSet<Object> Object { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
