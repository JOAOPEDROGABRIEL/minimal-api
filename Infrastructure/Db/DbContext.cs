using Microsoft.EntityFrameworkCore;
using minimal_api.Domain.Entities;

namespace minimal_api.Infrastructure.Db
{
    public class DbContexto : DbContext
    {
        private readonly IConfiguration _configurationAppSettings;
        
        public DbContexto(IConfiguration configurationAppSettings)
        {
            _configurationAppSettings = configurationAppSettings;
        }

        public DbSet<Administrador> Administradores { get; set; } = default!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var StringConnection = _configurationAppSettings.GetConnectionString("mysql")?.ToString();
                if (!string.IsNullOrWhiteSpace(StringConnection))
                {
                    optionsBuilder.UseMySql(
                        StringConnection, 
                        ServerVersion.AutoDetect(StringConnection)
                    );
                }
            } 
        }
    }
}