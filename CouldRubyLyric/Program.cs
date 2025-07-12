namespace CouldRubyLyric
{
    internal static class Program
    {
        public static string GithubUrl = @"https://www.bilibili.com";

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}