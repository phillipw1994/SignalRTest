#region Usings

#endregion

using Microsoft.EntityFrameworkCore;
using SignalR.Model.Db;

namespace SignalR.Database
{
    
    public class SignalRDbContext : DbContext, ISignalRDbContext
    {
        #region Constructors

        public SignalRDbContext()
        {
        }

        public SignalRDbContext(DbContextOptions<SignalRDbContext> options) : base(options)
        {
            //if(Database.GetPendingMigrations().Any())
            //    Database.Migrate();
        }

        #endregion

        public DbSet<ClientCredential> ClientCredentials { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //    => options.UseSqlite(@"Data Source=C:\SyncData.db");
    }
}
