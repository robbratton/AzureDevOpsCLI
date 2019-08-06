using System;
using System.IO;

namespace DevOpsTools
{
    public static class ToolHelper
    {
        /// <summary>
        ///     Get the Personal Access Token from %UserProfile%/appdata/roaming/vsts_key.txt
        /// </summary>
        /// <returns></returns>
        public static string GetPersonalAccessToken()
        {
            // ReSharper disable StringLiteralTypo
            var userProfile = Environment.GetEnvironmentVariable("USERPROFILE");
            var output = File.ReadAllText($@"{userProfile}\appdata\roaming\vsts_key.txt");
            // ReSharper restore StringLiteralTypo
            return output;
        }
    }
}