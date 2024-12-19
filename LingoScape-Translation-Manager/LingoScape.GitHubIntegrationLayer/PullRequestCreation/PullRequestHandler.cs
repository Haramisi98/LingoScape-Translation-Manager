using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace LingoScape.GitHubIntegrationLayer.PullRequestCreation
{
    public class PullRequestHandler
    {
        private readonly HttpClient _httpClient;
        private readonly string _repoBaseUrl;

        public PullRequestHandler(HttpClient httpClient, string repoOwner, string repoName)
        {
            _httpClient = httpClient;
            _repoBaseUrl = $"https://api.github.com/repos/{repoOwner}/{repoName}";
        }

        public async Task<bool> HandlePullRequestAsync(string baseBranch, string featureBranch, string filePath, string fileContent, string commitMessage, string prTitle, string prBody)
        {
            try
            {
                // Step 1: Get the SHA of the base branch
                string baseSha = await GetBranchShaAsync(baseBranch);

                // Step 2: Create a new feature branch from the base branch
                await CreateBranchAsync(featureBranch, baseSha);

                // Step 3: Push the changed .db file to the feature branch
                await PushFileToBranchAsync(filePath, fileContent, featureBranch, commitMessage);

                // Step 4: Create the pull request
                await SubmitPullRequestAsync(featureBranch, baseBranch, prTitle, prBody);

                // Everything succeeded
                return true;
            }
            catch (HttpRequestException ex)
            {
                // TODO error msg.
                return false;
            }
        }

        private async Task<string> GetBranchShaAsync(string branchName)
        {
            string url = $"{_repoBaseUrl}/git/ref/heads/{branchName}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to get branch SHA. Status Code: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var branchData = JsonSerializer.Deserialize<Dictionary<string, object>>(content);

            return branchData["object"] is JsonElement obj && obj.TryGetProperty("sha", out var sha)
                ? sha.GetString()
                : throw new Exception("SHA not found in branch data.");
        }

        private async Task CreateBranchAsync(string newBranch, string baseSha)
        {
            string url = $"{_repoBaseUrl}/git/refs";
            var payload = new
            {
                @ref = $"refs/heads/{newBranch}",
                sha = baseSha
            };

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, payload);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to create branch. Status Code: {response.StatusCode}");
            }
        }

        private async Task PushFileToBranchAsync(string filePath, string fileContent, string branchName, string commitMessage)
        {
            string url = $"{_repoBaseUrl}/contents/{filePath}";
            string base64Content = Convert.ToBase64String(Encoding.UTF8.GetBytes(fileContent));

            var payload = new
            {
                message = commitMessage,
                content = base64Content,
                branch = branchName
            };

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(url, payload);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to push file. Status Code: {response.StatusCode}");
            }
        }

        private async Task SubmitPullRequestAsync(string headBranch, string baseBranch, string title, string body)
        {
            string url = $"{_repoBaseUrl}/pulls";

            var payload = new
            {
                title,
                headBranch,
                baseBranch,
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
