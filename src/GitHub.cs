
public static partial class Program
{
  public static int GetGitHubRepos()
  {
    try
    {
      return 0;
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex.Message);
      return 1;
    }
  }
}
