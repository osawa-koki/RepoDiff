using Newtonsoft.Json;
using System.Net.Http.Headers;

internal class HttpModule
{
  internal static HttpClient client = new();

  internal static List<string> GitHub(string uri)
  {
    var token = Config.config.github.token;

    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{token}");
    client.DefaultRequestHeaders.Add("User-Agent", "C# App");

    Program.logger.Info($"GitHub API Request: {uri} | Token: {token}");
    using var response = client.GetAsync(uri).Result;
    response.EnsureSuccessStatusCode();

    var json = response.Content.ReadAsStringAsync().Result;
    dynamic repos_data = JsonConvert.DeserializeObject(json)!;

    List<string> repos = new();

    foreach (var repo_data in repos_data)
    {
      try
      {
        repos.Add(repo_data.name.ToString());
      } catch { }
    }

    return repos;
  }

  internal static List<string> GitLab(string uri)
  {
    Program.logger.Info($"GitLab API Request: {uri}");
    using var response = client.GetAsync(uri).Result;
    response.EnsureSuccessStatusCode();

    var json = response.Content.ReadAsStringAsync().Result;
    dynamic repos_data = JsonConvert.DeserializeObject(json)!;

    List<string> repos = new();

    foreach (var repo_data in repos_data)
    {
      try
      {
        repos.Add(repo_data.name.ToString());
      }
      catch { }
    }

    return repos;
  }

  internal static void RemoveAuth()
  {
    client.DefaultRequestHeaders.Authorization = null;
  }
}
