using System;
using System.Threading.Tasks;
using DevOpsTools.Tools.Interfaces;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

namespace DevOpsTools.Tools
{
    /// <inheritdoc cref="IReleaseDefinitionTool" />
    /// <inheritdoc cref="ToolBase" />
    /// <summary>
    ///     Tools for accessing information about builds.
    /// </summary>
    /// <remarks>
    ///     https://docs.microsoft.com/en-us/rest/api/azure/devops/release/?view=azure-devops-rest-5.1
    /// </remarks>
    public sealed class ReleaseDefinitionTool : ToolBase, IReleaseDefinitionTool
    {
        public const string LocalAPIVersionSuffix = "api-version=5.1-preview.3";

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="client"></param>
        /// <param name="organization"></param>
        /// <param name="project"></param>
        public ReleaseDefinitionTool(
            IClient client,
            string organization,
            string project
        ) : base(
            client,
            project,
            $"https://vsrm.dev.azure.com/{organization}/{project}/_apis/release/definitions",
            LocalAPIVersionSuffix)
        {
            ToolItem = "ReleaseDefinition";
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