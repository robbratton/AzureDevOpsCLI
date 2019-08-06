using System;
using System.Threading.Tasks;
using DevOpsTools.Tools.Interfaces;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

namespace DevOpsTools.Tools
{
    /// <inheritdoc cref="ToolBase" />
    /// <inheritdoc cref="ITaskGroupTool" />
    /// <summary>
    ///     Tools for accessing Task information.
    /// </summary>
    /// <remarks>
    ///     https://docs.microsoft.com/en-us/rest/api/azure/devops/distributedtask/taskgroups?view=azure-devops-rest-5.1
    /// </remarks>
    public sealed class TaskGroupTool : ToolBase, ITaskGroupTool
    {
        public const string LocalAPIVersionSuffix = "api-version=5.0-preview.1";

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="client"></param>
        /// <param name="organization"></param>
        /// <param name="project"></param>
        public TaskGroupTool(
            IClient client,
            string organization,
            string project
        ) : base(client,
            project,
            $"https://dev.azure.com/{organization}/{project}/_apis/distributedTask/taskGroups",
            LocalAPIVersionSuffix
        )
        {
            ToolItem = "TaskGroup";
        }

        #endregion Constructors

        #region Overrides

        protected override Task CopyImpl(string oldName, string newBuildDefinitionName)
        {
            throw new NotImplementedException();
        }

        protected override Task CopyImpl(object id, string newBuildDefinitionName)
        {
            throw new NotImplementedException();
        }

        #endregion Overrides

    }
}