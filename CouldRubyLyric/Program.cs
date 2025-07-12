namespace CouldRubyLyric
{
    internal static class Program
    {
        public static string GithubUrl = @"https://github.com/EnableAria/CouldRubyLyric";

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}