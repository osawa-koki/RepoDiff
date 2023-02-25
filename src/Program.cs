
using Newtonsoft.Json;

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

      // リポジトリ一覧の重複を削除
      repos = repos.Distinct().ToList();

      // GitHubに存在しないリポジトリを取得
      var missing_repos_github = repos.Except(github_repos).ToList();

      // GitLabに存在しないリポジトリを取得
      var missing_repos_gitlab = repos.Except(gitlab_repos).ToList();

      // json文字列に変換
      var missing_repos = JsonConvert.SerializeObject(new {
        missing = new
        {
          github = missing_repos_github,
          gitlab = missing_repos_gitlab,
        }
      }, Formatting.Indented);

      // ファイルに書き込み
      File.WriteAllText(Config.config.output_path, missing_repos);

      return 0;
    } catch (Exception ex)
    {
      Console.WriteLine(ex.Message);
      return 1;
    }
  }
}
