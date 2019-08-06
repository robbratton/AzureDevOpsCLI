using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading;
using DevOpsTools;
using DevOpsTools.Tools;
using VSTSTool.CommandInterpreter;

namespace VSTSTool
{
    public static class Program
    {
        private static readonly AppSettings AppSettings = new AppSettings();

        private static int Main(string[] args)
        {
            Debug.WriteLine("Starting Application");

            if (AppSettings.Verbose)
            {
                Console.WriteLine(GetStartupInformation());
            }

            var exitCode = ExitCode.Success;

            try
            {
                if (AppSettings.Verbose)
                {
                    Console.WriteLine("Settings:");
                    Console.WriteLine(Helper.DumpProperties(AppSettings));
                }

                var commands = SetUpCommands();

                if (AppSettings.Verbose)
                {
                    Console.WriteLine("Arguments: " + string.Join(", ", args));
                }

                var errorLevel = Processor.Process(args, commands);

                if (AppSettings.Verbose)
                {
                    Console.WriteLine("Process Result ErrorLevel: " + errorLevel);
                }
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine($"An unhandled exception occurred: {exception}");
                exitCode = ExitCode.Exception;
            }

            if (Debugger.IsAttached)
            {
                Console.WriteLine("");
                Console.WriteLine("Exit code: " + exitCode + " (" + (int) exitCode + ")");
                Console.WriteLine("DEBUG: Press x to exit.");
                while (Console.ReadKey(true).KeyChar != 'x')
                {
                    Thread.Sleep(1000);
                }
            }

            Debug.WriteLine($"Stopping Application. Exit code: {exitCode} ({(int) exitCode})");
            return (int) exitCode;
        }

        private static Commands SetUpCommands()
        {
            var pat = ToolHelper.GetPersonalAccessToken();

            var client = new Client(pat);

            var projectTool = new ProjectTool(
                client,
                AppSettings.AzureDevOpsOrganization,
                AppSettings.AzureDevOpsProject
            );

            var projectId = (Guid) projectTool.GetId(AppSettings.AzureDevOpsProject).Result;

            var buildTool = new BuildDefinitionTool(
                client,
                AppSettings.AzureDevOpsOrganization,
                AppSettings.AzureDevOpsProject,
                projectId
            );

            var releaseTool = new ReleaseDefinitionTool(
                client,
                AppSettings.AzureDevOpsOrganization,
                AppSettings.AzureDevOpsProject
            );

            var repositoryTool = new RepositoryTool(
                client,
                AppSettings.AzureDevOpsOrganization,
                AppSettings.AzureDevOpsProject,
                projectId
            );

            var taskTool = new TaskGroupTool(
                client,
                AppSettings.AzureDevOpsOrganization,
                AppSettings.AzureDevOpsProject
            );

            var variableGroupTool = new VariableGroupTool(
                client,
                AppSettings.AzureDevOpsOrganization,
                AppSettings.AzureDevOpsProject
            );

            var commands = new Commands(
                buildTool,
                releaseTool,
                projectTool,
                repositoryTool,
                taskTool,
                variableGroupTool);
            return commands;
        }

        public static string GetStartupInformation()
        {
            var output = new StringBuilder();

            var thisAssembly = Assembly.GetCallingAssembly();
            var assemblyInfo = thisAssembly.GetName();
            output.AppendLine($"{assemblyInfo.Name} {assemblyInfo.Version}");

            var versionInfo = FileVersionInfo.GetVersionInfo(thisAssembly.Location);
            if (!string.IsNullOrWhiteSpace(versionInfo.CompanyName))
            {
                output.AppendLine($"Company: {versionInfo.CompanyName}");
            }

            if (!string.IsNullOrWhiteSpace(versionInfo.LegalCopyright))
            {
                output.AppendLine(versionInfo.LegalCopyright);
            }

            if (!string.IsNullOrWhiteSpace(versionInfo.Comments))
            {
                output.AppendLine($"Comments: {versionInfo.Comments}");
            }

            output.AppendLine("");

            return output.ToString();
        }

        // ExitCodes
        private enum ExitCode
        {
            Success = 0,
            Exception = 100
        }
    }
}