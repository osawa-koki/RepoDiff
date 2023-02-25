
public static partial class Program
{
  internal static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
  public static int Main()
  {
    try
    {
      var config_load_result = Config.LoadConfig();
      if (config_load_result == false)
      {
        Console.WriteLine("Failed to load config.");
        return 1;
      }

      List<string> repos = new();

      // GitHubリポジトリ一覧
      var github_repos = GetGitHubRepos();
      repos.AddRange(github_repos);

      // 認証情報削除
      HttpModule.RemoveAuth();

      // GitLabリポジトリ一覧削除
      var gitlab_repos = GetGitLabRepos();
      repos.AddRange(gitlab_repos);

      return 0;
    } catch (Exception ex)
    {
      Console.WriteLine(ex.Message);
      return 1;
    }
  }
}
