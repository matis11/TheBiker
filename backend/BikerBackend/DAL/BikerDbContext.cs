using BikerBackend.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikerBackend.DAL
{
    public class BikerDbContext :DbContext
    {
        public DbSet<VibrationData> VibrationDatas{ get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<FinalData> FinalDatas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=BikerDevDb;Trusted_Connection=True;");
            optionsBuilder.UseSqlServer(@"Server=tcp:bikerdbserver.database.windows.net,1433;Initial Catalog=bikerdb;Persist Security Info=False;User ID=Wituch;Password=P@ssw0rd;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"); 
        }
    }
}
