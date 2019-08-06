using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;

// ReSharper disable MemberCanBePrivate.Global

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace VSTSTool
{
    /// <summary>
    ///     Contains the settings read from the AppSettings.json file.
    /// </summary>
    public class AppSettings
    {
        #region Constructor

        /// <summary>
        ///     Constructor
        /// </summary>
        public AppSettings()
        {
            var configDirectory = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            var builder = new ConfigurationBuilder()
                    .SetBasePath(configDirectory)
                    .AddJsonFile("AppSettings.json", false, true)
                ;
            var configuration = builder.Build();

            configuration.Bind(this);
        }

        #endregion Constructor

        #region Settings

        public string AzureDevOpsOrganization { get; set; }

        public string AzureDevOpsProject { get; set; }

        public bool Verbose { get; set; }

        #endregion Settings
    }
}