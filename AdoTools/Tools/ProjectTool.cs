using System;
using System.Threading.Tasks;
using DevOpsTools.Tools.Interfaces;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

namespace DevOpsTools.Tools
{
    /// <inheritdoc cref="ToolBase" />
    /// <inheritdoc cref="IProjectTool" />
    /// <summary>
    ///     Tools for accessing Task information.
    /// </summary>
    /// <remarks>
    ///     https://docs.microsoft.com/en-us/rest/api/azure/devops/core/projects?view=azure-devops-rest-5.1
    /// </remarks>
    public sealed class ProjectTool : ToolBase, IProjectTool
    {
        public const string LocalAPIVersionSuffix = "api-version=5.1-preview.4";

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="client"></param>
        /// <param name="organization"></param>
        /// <param name="project"></param>
        public ProjectTool(
            IClient client,
            string organization,
            string project
        ) : base(client,
            project,
            $"https://dev.azure.com/{organization}/_apis/projects",
            LocalAPIVersionSuffix
        )
        {
            ToolItem = "Project";
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