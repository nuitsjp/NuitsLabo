using System;
using System.Threading.Tasks;
using Octokit;
using CredentialManagement;
using System.Net;

const string CredentialTarget = "YourAppName:GitHubToken";
const string ClientId = "Ov23li5tAvMhYxP2SyrH";

var client = new GitHubClient(new ProductHeaderValue("YourAppName"));

string accessToken = GetStoredAccessToken();
if (string.IsNullOrEmpty(accessToken))
{
    accessToken = await PerformDeviceFlowAuthentication();
    StoreAccessToken(accessToken);
}

client.Credentials = new Credentials(accessToken);

try
{
    // トークンの有効性を確認するため、ユーザー情報を取得
    var user = await client.User.Current();
    Console.WriteLine($"Authenticated as {user.Login}");
}
catch (AuthorizationException)
{
    Console.WriteLine("Stored token is invalid. Reauthenticating...");
    accessToken = await PerformDeviceFlowAuthentication();
    StoreAccessToken(accessToken);
    client.Credentials = new Credentials(accessToken);
}

// ここで GitHub API を使用した操作を行います
string owner = "nuitsjp";
string repoName = "PrivateLabo";

try
{
    var repo = await client.Repository.Get(owner, repoName);
    Console.WriteLine($"Repository found: {repo.FullName} (Private: {repo.Private})");
}
catch (NotFoundException)
{
    Console.WriteLine("Repository not found or you don't have access.");
    return;
}

string path = $"{DateTime.Now:yyyyMMdd-HHmmss}.txt";
string content = "Hello, World!";
string commitMessage = "Add new file via Octokit.NET";

try
{
    var fileRequest = new CreateFileRequest(commitMessage, content, branch: "main");
    var result = await client.Repository.Content.CreateFile(owner, repoName, path, fileRequest);

    Console.WriteLine($"File created successfully: {result.Content.Path}");
}
catch (ApiException ex)
{
    Console.WriteLine($"Error creating file: {ex.Message}");
}


string GetStoredAccessToken()
{
    using var credential = new Credential();
    credential.Target = CredentialTarget;
    if (credential.Load())
    {
        return credential.Password;
    }
    return null;
}

void StoreAccessToken(string token)
{
    using var credential = new Credential();
    credential.Target = CredentialTarget;
    credential.Username = "GitHubAccessToken";
    credential.Password = token;
    credential.Type = CredentialType.Generic;
    credential.PersistanceType = PersistanceType.LocalComputer;
    credential.Save();
}

async Task<string> PerformDeviceFlowAuthentication()
{
    var request = new OauthDeviceFlowRequest(ClientId);
    request.Scopes.Add("repo");
    var deviceFlow = await client.Oauth.InitiateDeviceFlow(request);
    Console.WriteLine($"Please visit: {deviceFlow.VerificationUri}");
    Console.WriteLine($"And enter the code: {deviceFlow.UserCode}");
    var token = await client.Oauth.CreateAccessTokenForDeviceFlow(ClientId, deviceFlow);
    return token.AccessToken;
}

