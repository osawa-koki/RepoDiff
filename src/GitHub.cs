
public static partial class Program
{
  public static List<string> GetGitHubRepos()
  {
    try
    {
      List<string> repos = new();

      var page = 1;
      var per_page = 100;
      while (true)
      {
        var github_repos = HttpModule.GitHub($"https://api.github.com/users/{Config.config.github.username}/repos?page={page}&per_page={per_page}");
        repos.AddRange(github_repos);
        page++;
        if (github_repos.Count < per_page)
        {
          break;
        }
      }
      return repos;
    }
    catch (Exception ex)
    {
      logger.Error(ex.Message);
      return new();
    }
  }

  public static List<string> GetGitLabRepos()
  {
    try
    {
      List<string> repos = new();

      var page = 1;
      var per_page = 100;
      while (true)
      {
        var url = $"https://gitlab.example.com/api/v4/projects?owned=true&simple=true&membership=true&page={page}&per_page={per_page}";
        var headers = new Dictionary<string, string>
            {
                { "Private-Token", "YOUR_PRIVATE_ACCESS_TOKEN" }
            };
        var gitlab_repos = HttpModule.GetJsonArray(url, headers).Select(x => x["path_with_namespace"].ToString()).ToList();
        repos.AddRange(gitlab_repos);
        page++;
        if (gitlab_repos.Count < per_page)
        {
          break;
        }
      }
      return repos;
    }
    catch (Exception ex)
    {
      logger.Error(ex.Message);
      return new();
    }
  }

}
