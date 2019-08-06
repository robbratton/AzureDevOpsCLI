using System;
using System.Threading.Tasks;
using DevOpsTools.Tools.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

namespace DevOpsTools.Tools
{
    /// <inheritdoc cref="IBuildDefinitionTool" />
    /// <inheritdoc cref="ToolBase" />
    /// <summary>
    ///     Tools for accessing information about builds.
    /// </summary>
    /// <remarks>
    ///     https://docs.microsoft.com/en-us/rest/api/azure/devops/build/builds?view=azure-devops-rest-5.1
    /// </remarks>
    public sealed class BuildDefinitionTool : ToolBase, IBuildDefinitionTool
    {
        private readonly string _organization;
        private readonly Guid _projectId;
        public const string LocalAPIVersionSuffix = "api-version=5.0-preview.7";

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="client"></param>
        /// <param name="organization"></param>
        /// <param name="project"></param>
        /// <param name="projectId"></param>
        public BuildDefinitionTool(
            IClient client,
            string organization,
            string project,
            Guid projectId
        ) : base(
            client,
            project,
            $"https://dev.azure.com/{organization}/{project}/_apis/build/definitions",
            LocalAPIVersionSuffix)
        {
            ToolItem = "BuildDefinition";
            _organization = organization;
            _projectId = projectId;
        }

        #endregion Constructors

        protected override async Task CopyImpl(string oldName, string newName)
        {
            if (string.IsNullOrWhiteSpace(oldName))
            {
                throw new ArgumentException(nameof(oldName));
            }

            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException(nameof(newName));
            }

            var id = await GetId(oldName).ConfigureAwait(false);

            await CopyImpl(id, newName).ConfigureAwait(false);
        }

        protected override async Task CopyImpl(object id, string newBuildDefinitionName)
        {
            new Validators().ValidateId(id);

            if (string.IsNullOrWhiteSpace(newBuildDefinitionName))
            {
                throw new ArgumentException(nameof(newBuildDefinitionName));
            }

            var oldDefinition = await Get(id).ConfigureAwait(false);

            var jObject = JObject.Parse(oldDefinition);

            var oldBuildDefinitionName = jObject["name"].ToString();

            // Delete fields
            jObject.Remove("createdBy");
            jObject.Remove("createdOn");
            jObject.Remove("modifiedBy");
            jObject.Remove("modifiedOn");
            jObject.Remove("revision");
            jObject.Remove("environments[0]currentRelease");
            jObject.Remove("environments[0]badgeUrl");
            jObject.Remove("artifacts[0]definitionReference:artifactSourceDefinitionUrl");
            jObject.Remove("url");
            jObject.Remove("uri");
            jObject.Remove("_links");
            jObject.Remove("id");

            // Update fields
            jObject["name"] = newBuildDefinitionName;

            // Update Repository Fields
            var newRepoName = Helpers.GetRepoNameFromBuildName(newBuildDefinitionName);

            if (newRepoName != null)
            {
                var repoTool = new RepositoryTool(Client, _organization, Project, _projectId);
                var newRepoJson = repoTool.Get(newRepoName).Result;
                var newRepo = JObject.Parse(newRepoJson);

                jObject["repository"]["name"] = newRepo["name"];
                jObject["repository"]["url"] = newRepo["url"];
                jObject["repository"]["id"] = newRepo["id"];
            }

            // This is a special case for cloning -DevBranch-Checkmarx build definitions.
            if (
                oldBuildDefinitionName.EndsWith("DevBranch-Checkmarx", StringComparison.CurrentCultureIgnoreCase)
                &&
                newBuildDefinitionName.EndsWith("DevBranch-Checkmarx", StringComparison.CurrentCultureIgnoreCase)
            )
            {
                Helpers.GetCheckmarxPresetAndProject(newBuildDefinitionName, out var preset, out var project);

                if (!string.IsNullOrWhiteSpace(preset))
                {
                    jObject["variables"]["CheckmarxPreset"]["value"] = preset;
                }

                if (!string.IsNullOrWhiteSpace(project))
                {
                    jObject["variables"]["CheckmarxProject"]["value"] = project;
                }


                // Randomize run times using the current second as the minute.
                jObject["triggers"][0]["schedules"][0]["startMinutes"] = DateTime.Now.Second;
            }

            var updatedDefinition = JsonConvert.SerializeObject(jObject, Formatting.Indented);

            await Create(updatedDefinition).ConfigureAwait(false);
        }

    }
}
