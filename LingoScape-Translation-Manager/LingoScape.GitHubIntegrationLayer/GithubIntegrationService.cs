using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LingoScape.GitHubIntegrationLayer
{
    public class GithubIntegrationService
    {
        private readonly string _accessToken;
        private readonly HttpClient _httpClient;

        public GithubIntegrationService(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
                throw new ArgumentException("Access token cannot be null or empty.", nameof(accessToken));

            _accessToken = accessToken;

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _accessToken);
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("OSRS-Translation-Manager");
        }

        public async Task<string> PullDatabaseFileAsync(string repoOwner, string repoName, string filePath)
        {
            string url = $"https://api.github.com/repos/{repoOwner}/{repoName}/contents/{filePath}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to fetch the file. Status Code: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var fileData = JsonSerializer.Deserialize<Dictionary<string, string>>(content);
            string base64Content = fileData["content"];
            return Encoding.UTF8.GetString(Convert.FromBase64String(base64Content));
        }

        public async Task PushDatabaseFileAsync(string repoOwner, string repoName, string filePath, string fileContent, string commitMessage, string branch = "main")
        {
            string url = $"https://api.github.com/repos/{repoOwner}/{repoName}/contents/{filePath}";
            string base64Content = Convert.ToBase64String(Encoding.UTF8.GetBytes(fileContent));

            var payload = new Dictionary<string, object>
        {
            { "message", commitMessage },
            { "content", base64Content },
            { "branch", branch }
        };

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(url, payload);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to push the file. Status Code: {response.StatusCode}");
            }
        }

        public async Task CreatePullRequestAsync(string repoOwner, string repoName, string branchName, string title, string body)
        {
            string url = $"https://api.github.com/repos/{repoOwner}/{repoName}/pulls";
            string main = "main";
            var payload = new
            {
                title,
                branchName,
                main,
                body
            };

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, payload);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to create pull request. Status Code: {response.StatusCode}");
            }
        }
    }
}
