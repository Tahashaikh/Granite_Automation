namespace Framework.Config
{
    public static class PlaywrightConfig
    {
        public static bool Headless = false;
        public static string Browser = "chromium"; // "webkit", "firefox"
        public static string Channel = "chrome";   // or "msedge"
    }
}