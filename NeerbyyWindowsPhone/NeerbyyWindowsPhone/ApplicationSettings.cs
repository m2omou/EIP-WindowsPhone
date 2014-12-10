using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO.IsolatedStorage;

namespace NeerbyyWindowsPhone
{
    class ApplicationSettings
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="roaming"></param>
        public static void SetSetting<T>(string key, T value)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            settings[key] = value;
            settings.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="roaming"></param>
        /// <returns></returns>
        public static T GetSetting<T>(string key)
        {
            return GetSetting(key, default(T));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <param name="roaming"></param>
        /// <returns></returns>
        public static T GetSetting<T>(string key, T defaultValue)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            return settings.Contains(key) &&
                   settings[key] is T ?
                   (T)settings[key] : defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="roaming"></param>
        /// <returns></returns>
        public static bool HasSetting<T>(string key)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            return settings.Contains(key) && settings[key] is T;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="roaming"></param>
        /// <returns></returns>
        public static bool RemoveSetting(string key)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (settings.Contains(key) && settings.Remove(key))
            {
                settings.Save();
                return true;
            }
            return false;
        }
    }
}
