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
                        CREATE TABLE IF NOT EXISTS Translatable (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            RawText TEXT NOT NULL,
                            Type TEXT
                        );");

                    // Create StaticTranslation Table
                    ExecuteNonQuery(connection, @"
                        CREATE TABLE IF NOT EXISTS StaticTranslation (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            RawText TEXT NOT NULL,
                            LanguageCode TEXT NOT NULL,
                            Translation TEXT
                        );");

                    // Create DynamicTranslation Table
                    ExecuteNonQuery(connection, @"
                        CREATE TABLE IF NOT EXISTS DynamicTranslation (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            RawText TEXT NOT NULL,
                            NPCName TEXT,
                            TextType TEXT,
                            Translation TEXT
                        );");

                    // Create Metadata Table
                    ExecuteNonQuery(connection, @"
                        CREATE TABLE IF NOT EXISTS Metadata (
                            TranslationId INTEGER PRIMARY KEY,
                            LastUpdated DATETIME DEFAULT CURRENT_TIMESTAMP,
                            User TEXT,
                            Status TEXT CHECK(Status IN ('Pending', 'Accepted', 'Rejected')),
                            FOREIGN KEY (TranslationId) REFERENCES DynamicTranslation(Id)
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
