using System.Text.Json;

namespace LingoScape.GitHubIntegrationLayer.DataSync
{
    public class DataSyncService
    {
        private readonly HttpClient _httpClient;
        private readonly string _repoBaseUrl;
        private readonly string _localDbFilePath;

        public DataSyncService(HttpClient httpClient, string repoOwner, string repoName, string localDbFilePath)
        {
            _httpClient = httpClient;
            _repoBaseUrl = $"https://api.github.com/repos/{repoOwner}/{repoName}";
            _localDbFilePath = localDbFilePath;
        }

        /// <summary>
        /// Pulls the latest version of the database file from the repository and saves it locally.
        /// </summary>
        public async Task<bool> PullLatestDatabaseAsync(string dbFilePathInRepo)
        {
            try
            {
                // Build the API request URL
                string url = $"{_repoBaseUrl}/contents/{dbFilePathInRepo}";

                HttpResponseMessage response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Failed to fetch the database file. Status Code: {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var fileData = JsonSerializer.Deserialize<DatabaseFileResponse>(content);

                // Decode and save the database file locally
                byte[] fileBytes = Convert.FromBase64String(fileData.Content);
                await File.WriteAllBytesAsync(_localDbFilePath, fileBytes);

                Console.WriteLine("Database file updated successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        // Model for parsing GitHub's API response
        private class DatabaseFileResponse
        {
            public string Name { get; set; }
            public string Path { get; set; }
            public string Content { get; set; } // Base64 encoded
            public string Sha { get; set; }
        }
    }
}
