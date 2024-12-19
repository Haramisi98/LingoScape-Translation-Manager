using LingoScape.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace LingoScape.DataAccessLayer
{
    public class LingoDbContext : DbContext
    {
        public DbSet<TranslatableTextModel> Translatables { get; set; }
        public DbSet<StaticTranslationModel> StaticTranslations { get; set; }
        public DbSet<DynamicTranslationModel> DynamicTranslations { get; set; }
        public DbSet<MetadataModel> MetadataEntries { get; set; }

        private static string _connectionString = "Database/LingoScapeLite.db";
        private static readonly string DbPath = "Database/LingoScapeLite.db";

        public LingoDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
