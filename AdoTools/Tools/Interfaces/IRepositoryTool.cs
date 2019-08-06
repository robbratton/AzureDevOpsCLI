using System;
using System.Threading.Tasks;

// ReSharper disable UnusedMember.Global

// ReSharper disable UnusedMemberInSuper.Global

namespace DevOpsTools.Tools.Interfaces
{
    public interface IRepositoryTool : ITool
    {
        /// <summary>
        ///     Create a fork of a repository
        /// </summary>
        /// <param name="id"></param>
        /// <param name="destRepoName"></param>
        /// <returns>The created repository definition</returns>
        Task CreateFork(Guid id, string destRepoName);

        /// <summary>
        ///     Create a fork of a repository
        /// </summary>
        /// <param name="sourceRepoName">Provide a value to make a fork. Leave empty to create a new repository.</param>
        /// <param name="destRepoName"></param>
        /// <returns>The created repository definition</returns>
        Task CreateFork(string sourceRepoName, string destRepoName);

        /// <summary>
        /// Creates a branch.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="sourceBranchName">Name of the source branch.</param>
        /// <param name="destBranchName">Name of the dest branch.</param>
        /// <returns></returns>
        Task CreateBranch(string repository, string sourceBranchName, string destBranchName);

        /// <summary>
        /// Gets the branch identifier for a name.
        /// </summary>
        /// <param name="repositoryId">The repository identifier.</param>
        /// <param name="branchName">Name of the branch.</param>
        /// <returns></returns>
        Task<string> GetBranchId(Guid repositoryId, string branchName);

        /// <summary>
        /// Gets all the branches.
        /// </summary>
        /// <param name="repositoryId">The repository identifier.</param>
        /// <returns></returns>
        Task<string> GetBranchesMany(Guid repositoryId);

    }
}