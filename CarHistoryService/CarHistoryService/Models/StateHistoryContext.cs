using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CarHistoryService;

namespace CarHistoryService.Models
{
    public class StateHistoryDbContext : DbContext
    {
        private ConnectionSettings connectionSettings = ConnectionSettings.getInstance();

        public DbSet<TimeCodeHistory> TimeCodes { get; set; }
        public DbSet<AccuHistory> AccuStates { get; set; }

        public StateHistoryDbContext() : base() { }
        public StateHistoryDbContext(DbContextOptions<StateHistoryDbContext> ops) : base(ops) { }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(connectionSettings.ConnectionString);
            base.OnConfiguring(builder);
        }
    }
}
