using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordTracker.SqliteDataLayer
{
     public class DataLayerContext:DbContext
    {
        public DataLayerContext() : base("name = SqliteConnStr") { }

        public DataLayerContext(string filename) : base(new SQLiteConnection()
        {
            ConnectionString =
                    new SQLiteConnectionStringBuilder()
                    { DataSource = filename, ForeignKeys = true }
                    .ConnectionString
        }, true)
        { }

        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            
            base.OnModelCreating(modelBuilder);

        }

        public DbSet<Beta> Betas { get; set; }

        public DbSet<Alpha> Alphas { get; set; }

        public DbSet<Gamma> Gammas { get; set; }

        public DbSet<LetterRecord> LetterRecords { get; set; }

        public DbSet<Status> Status { get; set; }

        public DbSet<Delta> Deltas { get; set; }

        public DbSet<Theta> Thetas { get; set; }

    }
   
}
