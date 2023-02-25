
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
        var github_repos = HttpModule.GitLab($"https://gitlab.com/api/v4/users/{Config.config.gitlab.username}/projects?private_token={Config.config.gitlab.token}&per_page={per_page}&page={page}");
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

}
