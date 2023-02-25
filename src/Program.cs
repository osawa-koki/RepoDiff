
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

      var github_repos = GetGitHubRepos();
      repos.AddRange(github_repos);

      return 0;
    } catch (Exception ex)
    {
      Console.WriteLine(ex.Message);
      return 1;
    }
  }
}
