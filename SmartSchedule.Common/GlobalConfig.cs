namespace SmartSchedule.Common
{
    public static class GlobalConfig
    {
        public const bool DEV_MODE = true;

        public const string DEV_APPSETTINGS = "appsettings.Development.json";
        public const string PROD_APPSETTINGS = "appsettings.json";

        public static string AppSettingsFileName
        {
            get
            {
#pragma warning disable CS0162 // Unreachable code detected
                if (DEV_MODE)
                    return DEV_APPSETTINGS;
                return PROD_APPSETTINGS;
#pragma warning restore CS0162 // Unreachable code detected
            }
        }

        public const string CONNECTION_STRING_NAME = "SmartScheduleDatabase";
    }
}
