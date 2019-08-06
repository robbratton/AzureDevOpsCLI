using System;
using System.Text.RegularExpressions;

namespace DevOpsTools
{
    /// <summary>
    /// Tools shared by many internal classes.
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Makes the URI.
        /// </summary>
        /// <param name="basePath">The base path.</param>
        /// <param name="apiVersionSuffix">The API version suffix.</param>
        /// <param name="pathSuffix">The path suffix.</param>
        /// <param name="queryString">The query string.</param>
        /// <returns></returns>
        public static Uri MakeUri(string basePath, string apiVersionSuffix, string pathSuffix, string queryString)
        {
            var path = UriHelper.CombineUriPath(
                basePath,
                pathSuffix);

            if (!queryString.StartsWith("?"))
            {
                queryString = "?" + queryString;
            }

            if (!queryString.EndsWith("?"))
            {
                queryString += "&";
            }

            queryString += apiVersionSuffix;

            var output = new Uri(path + queryString);

            return output;
        }

        public static string GetRepoNameFromBuildName(string buildDefinitionName)
        {
            string output = null;

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var x in new[] {MicroserviceRepoRegex, MonolithRepoRegex, NugetRepoRegex})
            {
                var match = x.Match(buildDefinitionName);
                if (match.Groups.Count > 1)
                {
                    output = match.Groups[1].Value;
                    break;
                }
            }

            return output;

        }

        public static void GetCheckmarxPresetAndProject(string newBuildDefinitionName, out string preset, out string project)
        {
            const string projectPrefix = "AppServices-POC-";

            preset = null;
            project = null;

            if (newBuildDefinitionName.ToLower().Contains("monolith"))
            {
                preset = "WIP-RobBratton-Monolith";
                project = projectPrefix +
                          newBuildDefinitionName.Replace("Services-", "").Replace("-DevBranch-Checkmarx", "");
            }

            else if (newBuildDefinitionName.ToLower().Contains("microservice"))
            {
                preset = "WIP-RobBratton-Microservices";
                project = projectPrefix + "Microservice-" +
                          newBuildDefinitionName.Replace("Microservice-", "").Replace("-DevBranch-Checkmarx", "");
            }

            else if (newBuildDefinitionName.ToLower().Contains("upmc"))
            {
                preset = "WIP-RobBratton-NuGet";
                project = projectPrefix + "NuGet-" +
                          newBuildDefinitionName.Replace("NuGet-", "").Replace("-DevBranch-Checkmarx", "");
            }
        }

        private static readonly Regex MicroserviceRepoRegex =
            new Regex("^(Microservice-[^-]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex MonolithRepoRegex =
            new Regex("^(Monolith-[^-]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex NugetRepoRegex =
            new Regex("^NuGet-([^-]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    }
}
