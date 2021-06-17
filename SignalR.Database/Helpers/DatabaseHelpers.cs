#region Usings

#endregion

using Microsoft.EntityFrameworkCore;

namespace SignalR.Database.Helpers
{
    public static class DatabaseHelpers
    {
        #region Public Methods

        public static DbContextOptionsBuilder<T> CreateOptionsBuilder<T>(string connectionString) where T : DbContext
        {
            return new DbContextOptionsBuilder<T>().UseSqlite(connectionString).EnableSensitiveDataLogging();
        }

        #endregion
    }
}
