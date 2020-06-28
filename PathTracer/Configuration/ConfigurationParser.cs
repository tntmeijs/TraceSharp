using System;
using System.Configuration;
using System.Globalization;

namespace PathTracer.Configuration
{
    static class ConfigurationParser
    {
        /// <summary>
        /// Retrieve a configuration value as a string from the App.config file
        /// </summary>
        /// <param name="key">Key to look for in App.config</param>
        /// <returns>Value as a string</returns>
        public static string GetString(string key)
        {
            string value = ConfigurationManager.AppSettings.Get(key);

            if (value == null)
            {
                Console.WriteLine("[ERROR] App.config does not contain a value for key \"" + key + "\".");
            }

            return value;
        }

        /// <summary>
        /// Attempts to retrieve a value from App.config as an integer
        /// </summary>
        /// <param name="key">Key to look for in App.config</param>
        /// <returns>Value as an integer, defaults to zero upon failure</returns>
        public static int GetInt(string key)
        {
            string value = GetString(key);

            int output;
            if (int.TryParse(value, NumberStyles.Integer, new CultureInfo("en-US"), out output))
            {
                return output;
            }

            return 0;
        }

        /// <summary>
        /// Attempts to retrieve a value from App.config as a double
        /// </summary>
        /// <param name="key">Key to look for in App.config</param>
        /// <returns>Value as an double, defaults to zero upon failure</returns>
        public static double GetDouble(string key)
        {
            string value = GetString(key);

            double output;
            if (double.TryParse(value, NumberStyles.AllowDecimalPoint, new CultureInfo("en-US"), out output))
            {
                return output;
            }

            Console.WriteLine("[ERROR] Failed parsing string \"" + value + "\" to double.");
            Console.WriteLine("        A common mistake is using commas instead of decimal points.");
            return 0.0d;
        }
    }
}
