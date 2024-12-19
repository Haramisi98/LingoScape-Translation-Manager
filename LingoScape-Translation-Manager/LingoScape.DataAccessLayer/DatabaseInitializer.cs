using System.Data.SQLite;

namespace LingoScape.DataAccessLayer
{
    public static class DatabaseInitializer
    {
        private static readonly string DbPath = "Database/LingoScapeLite.db";
        private static readonly string ConnectionString = $"Data Source={DbPath};Version=3;";

        // TODO
        public static string GetConnectionString()
        {
            return $"Data Source={DbPath};Version=3;";
        }

        // When pull from repo fails, load empty schema
        public static void InitializeEmptyDatabase()
        {
            try
            {
                // Ensure Database Folder Exists
                var directory = Path.GetDirectoryName(DbPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Check if the database already exists
                if (!File.Exists(DbPath))
                {
                    SQLiteConnection.CreateFile(DbPath);
                    Console.WriteLine("Database file created successfully.");
                }

                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    Console.WriteLine("Database connection established.");

                    // Create Translatable Table
                    ExecuteNonQuery(connection, @"
                CREATE TABLE IF NOT EXISTS TranslatableTable (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    RawText TEXT NOT NULL,
                    Type TEXT,
                    Contributor TEXT
                );");

                    // Create StaticTranslations Table
                    ExecuteNonQuery(connection, @"
                CREATE TABLE IF NOT EXISTS StaticTranslations (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    RawText TEXT NOT NULL,
                    Translation TEXT,
                    LanguageCode TEXT NOT NULL,
                    Type TEXT,
                    Contributor TEXT
                );");

                    // Create DynamicContextTranslationTable
                    ExecuteNonQuery(connection, @"
                CREATE TABLE IF NOT EXISTS DynamicContextTranslationTable (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    RawText TEXT NOT NULL,
                    Translation TEXT,
                    LanguageCode TEXT NOT NULL
                );");

                    // Create DynamicTranslationTable
                    ExecuteNonQuery(connection, @"
                CREATE TABLE IF NOT EXISTS DynamicTranslationTable (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    RawText TEXT NOT NULL,
                    Translation TEXT,
                    LanguageCode TEXT NOT NULL,
                    Context INTEGER,
                    NPC TEXT,
                    Type TEXT,
                    Contributor TEXT,
                    FOREIGN KEY (Context) REFERENCES DynamicContextTranslationTable(Id)
                );");

                    // Create LanguageCompletion Table
                    ExecuteNonQuery(connection, @"
                CREATE TABLE IF NOT EXISTS LanguageCompletion (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    LanguageCode TEXT NOT NULL,
                    CompletionPercentage REAL,
                    TopContributors TEXT
                );");

                    Console.WriteLine("Database schema created successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
            }
        }

        private static void ExecuteNonQuery(SQLiteConnection connection, string sqlCommand)
        {
            using (var cmd = new SQLiteCommand(sqlCommand, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}
