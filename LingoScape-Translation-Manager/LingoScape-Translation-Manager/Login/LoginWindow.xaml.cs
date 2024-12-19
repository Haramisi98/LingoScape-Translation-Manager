using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Windows;

namespace LingoScape_Translation_Manager.Login
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private const string GitHubClientId = "Ov23liZuZQ37iscYeO2U";
        private const string GitHubRedirectUri = "http://localhost:5000/callback";
        private const string GitHubScope = "repo";

        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void GitHubLoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RedirectToGitHub();

                string authCode = await WaitForGitHubCallbackAsync();

                if (string.IsNullOrEmpty(authCode))
                {
                    DisplayStatusMessage("Login failed or canceled.");
                    return;
                }

                // Step 3: Exchange the authorization code for an access token
                DisplayStatusMessage("Authenticating...");
                string accessToken = await ExchangeAuthCodeForTokenAsync(authCode);

                if (!string.IsNullOrEmpty(accessToken))
                {
                    DisplayStatusMessage("Login successful!");
                    // Save the token in the Application properties
                    ((App)Application.Current).AccessToken = accessToken;
                    DialogResult = true; 
                    Close();
                }
                else
                {
                    DisplayStatusMessage("Authentication failed.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Redirects the user to GitHub's login page.
        /// </summary>
        private void RedirectToGitHub()
        {
            string authUrl = $"https://github.com/login/oauth/authorize" +
                             $"?client_id={GitHubClientId}" +
                             $"&redirect_uri={Uri.EscapeDataString(GitHubRedirectUri)}" +
                             $"&scope={GitHubScope}";

            Process.Start(new ProcessStartInfo
            {
                FileName = authUrl,
                UseShellExecute = true
            });

            DisplayStatusMessage("Redirecting to GitHub...");
        }

        /// <summary>
        /// Waits for the GitHub redirect and extracts the authorization code.
        /// </summary>
        private async Task<string> WaitForGitHubCallbackAsync()
        {
            using var httpListener = new HttpListener();
            httpListener.Prefixes.Add(GitHubRedirectUri + "/");
            httpListener.Start();

            try
            {
                DisplayStatusMessage("Waiting for GitHub response...");
                var context = await httpListener.GetContextAsync();

                string authCode = context.Request.QueryString["code"];

                // Respond to the browser with auto-close script
                using var response = context.Response;
                string responseString = @"
            <html>
                <body>
                    <p>Authentication successful. You can close this tab.</p>
                    <script>window.close();</script>
                </body>
            </html>";
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);

                return authCode;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during GitHub callback handling: {ex.Message}");
                return null;
            }
        }


        /// <summary>
        /// Exchanges the authorization code for an access token.
        /// </summary>
        private async Task<string> ExchangeAuthCodeForTokenAsync(string authCode)
        {
            string clientSecret = Environment.GetEnvironmentVariable("GITHUB_CLIENT_SECRET");

            if (string.IsNullOrEmpty(clientSecret))
            {
                MessageBox.Show("Client Secret not found. Please set it in the environment variables.");
                return null;
            }

            using HttpClient client = new HttpClient();
            try
            {
                var response = await client.PostAsync(
                    "https://github.com/login/oauth/access_token",
                    new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "client_id", GitHubClientId },
                        { "client_secret", clientSecret },
                        { "code", authCode },
                        { "redirect_uri", GitHubRedirectUri }
                    })
                );

                string responseContent = await response.Content.ReadAsStringAsync();
                var tokenData = System.Web.HttpUtility.ParseQueryString(responseContent);
                return tokenData["access_token"];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during token exchange: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Displays a status message to the user.
        /// </summary>
        private void DisplayStatusMessage(string message)
        {
            StatusMessage.Text = message;
        }
    }
}
