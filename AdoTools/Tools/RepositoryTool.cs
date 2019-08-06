using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DevOpsTools.Tools.Interfaces;
using Newtonsoft.Json;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

namespace DevOpsTools.Tools
{
    /// <inheritdoc cref="ToolBase" />
    /// <inheritdoc cref="IRepositoryTool" />
    /// <summary>
    ///     Tools for accessing Git repositories.
    /// </summary>
    /// <remarks>
    ///     https://docs.microsoft.com/en-us/rest/api/azure/devops/git/repositories?view=azure-devops-rest-5.1
    /// </remarks>
    public sealed class RepositoryTool : ToolBase, IRepositoryTool
    {
        public const string LocalAPIVersionSuffix = "api-version=5.1-preview.1";
        private readonly Guid _projectId;

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="client"></param>
        /// <param name="organization"></param>
        /// <param name="project"></param>
        /// <param name="projectId"></param>
        public RepositoryTool(
            IClient client,
            string organization,
            string project,
            Guid projectId
        ) : base(
            client,
            project,
            $"https://dev.azure.com/{organization}/{project}/_apis/git/repositories",
            LocalAPIVersionSuffix)
        {
            _projectId = projectId;
            BasePath = $"https://dev.azure.com/{organization}/{_projectId}/_apis/git/repositories";
            ToolItem = "Repository";
        }

        #endregion Constructors

        #region Overrides

        protected override Task CopyImpl(string oldName, string newName)
        {
            throw new NotImplementedException();
        }

        protected override Task CopyImpl(object id, string newName)
        {
            throw new NotImplementedException();
        }

        #endregion Overrides

        /// <inheritdoc />
        public async Task CreateFork(Guid id, string destRepoName)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException(nameof(id));
            }

            if (string.IsNullOrWhiteSpace(destRepoName))
            {
                throw new ArgumentException(nameof(destRepoName));
            }

            await CreateFork(id.ToString(), destRepoName).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task CreateFork(string sourceRepoName, string destRepoName)
        {
            if (string.IsNullOrWhiteSpace(sourceRepoName))
            {
                throw new ArgumentException(nameof(sourceRepoName));
            }

            if (string.IsNullOrWhiteSpace(destRepoName))
            {
                throw new ArgumentException(nameof(destRepoName));
            }

            var oldId = await GetId(sourceRepoName).ConfigureAwait(false);

            var bodyObject = new Repository
            {
                Name = destRepoName,
                Project = new Project
                {
                    Id = _projectId,
                    Name = Project
                },
                ParentRepository = new Repository
                {
                    Id = (Guid)oldId,
                    Project = new Project
                    {
                        Id = _projectId,
                        Name = Project
                    }
                }
            };

            var body = JsonConvert.SerializeObject(bodyObject, Formatting.Indented);

            // branches to duplicate
            const string queryString = "sourceRef=refs/heads/develop";

            var uri = Helpers.MakeUri(BasePath, LocalAPIVersionSuffix, "", queryString);

            var result = Client.PostStringAsync(uri, body).Result;

            var output = result.Content.ReadAsStringAsync().Result;

            if (result.StatusCode != HttpStatusCode.Created)
            {
                throw new Exception($"HTTP return code was not Created: {result.StatusCode} -- {output}");
            }

            // Wait for the new repo to show up. Sometimes it's not instant.
            Thread.Sleep(2000);

            // Create master branch from develop.
            await CreateBranch(destRepoName, "refs/heads/develop", "refs/heads/master").ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task CreateBranch(string repository, string sourceBranchName, string destBranchName)
        {
            if (string.IsNullOrWhiteSpace(sourceBranchName))
            {
                throw new ArgumentException(nameof(sourceBranchName));
            }

            if (string.IsNullOrWhiteSpace(destBranchName))
            {
                throw new ArgumentException(nameof(destBranchName));
            }

            var repositoryId = await GetId(repository).ConfigureAwait(false);
            var sourceBranchId = await GetBranchId((Guid)repositoryId, sourceBranchName).ConfigureAwait(false);

            // branches to duplicate
            var bodyObject = new[]
            {
                new RefItem(destBranchName, "0000000000000000000000000000000000000000", sourceBranchId)
            };
            var body = JsonConvert.SerializeObject(bodyObject, Formatting.Indented);

            var suffix = $"{repository}/refs";

            var uri = Helpers.MakeUri(BasePath, LocalAPIVersionSuffix, suffix, "");

            var result = Client.PostStringAsync(uri, body).Result;

            var output = result.Content.ReadAsStringAsync().Result;

            if (result.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"HTTP return code was not Created: {result.StatusCode} -- {output}");
            }

            var outputObject = ((dynamic) JsonConvert.DeserializeObject(output)).value;
            if (outputObject.Count > 0)
            {
                var status = outputObject[0].updateStatus.Value;
                if (!status.Equals("succeeded"))
                {
                    //Console.Error.WriteLine($"updateStatus was {status}");
                    throw new Exception($"updateStatus was {status}");
                }
            }
        }

        /// <inheritdoc />
        public async Task<string> GetBranchId(Guid repositoryId, string branchName)
        {
            if (string.IsNullOrWhiteSpace(branchName))
            {
                throw new ArgumentException(nameof(branchName));
            }

            var definitions = await GetBranchesMany(repositoryId).ConfigureAwait(false);

            dynamic items = JsonConvert.DeserializeObject(definitions);

            string output = null;

            foreach (var item in items.value)
            {
                if (branchName.Equals(item.name.Value, StringComparison.CurrentCultureIgnoreCase))
                {
                    output = item.objectId.Value;
                    break;
                }
            }

            if (string.IsNullOrWhiteSpace(output))
            {
                var exception =
                    new KeyNotFoundException(
                        $"Branch Id not found. repositoryId: {repositoryId}, branchName: {branchName}");
                exception.Data.Add("repositoryId", repositoryId);
                exception.Data.Add("branchName", branchName);

                throw exception;
            }

            return output;
        }

        /// <inheritdoc />
        public async Task<string> GetBranchesMany(Guid repositoryId)
        {
            if (repositoryId == Guid.Empty)
            {
                throw new ArgumentException(nameof(repositoryId));
            }

            var uri = Helpers.MakeUri(BasePath, LocalAPIVersionSuffix, $"{repositoryId}/refs", "");

            return await Client.GetStringAsync(uri).ConfigureAwait(false);
        }
    }


    #region Internal Classes for JSON 

    [JsonObject]
    [ExcludeFromCodeCoverage]
    internal class RefItem
    {
        public RefItem(string name, string oldObjectId, string newObjectId)
        {
            Name = name;
            OldObjectId = oldObjectId;
            NewObjectId = newObjectId;
        }

        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("oldObjectId")] public string OldObjectId { get; set; }
        [JsonProperty("newObjectId")] public string NewObjectId { get; set; }
    }

    [JsonObject]
    [ExcludeFromCodeCoverage]
    internal class Project
    {
        [JsonProperty("id")] public Guid? Id { get; set; }

        [JsonProperty("name")] public string Name { get; set; }
    }

    [JsonObject]
    [ExcludeFromCodeCoverage]
    internal class Repository
    {
        [JsonProperty("id")] public Guid? Id { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("project")] public Project Project { get; set; }

        [JsonProperty("parentRepository")] public Repository ParentRepository { get; set; }
    }

    #endregion Internal Classes for JSON 
}