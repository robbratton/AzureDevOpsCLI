using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// ReSharper disable UnusedMemberInSuper.Global

namespace DevOpsTools.Tools.Interfaces
{
    public interface ITool
    {
        /// <summary>
        ///     Create an item
        /// </summary>
        /// <param name="content"></param>
        Task Create(string content);

        /// <summary>
        /// Copies the specified item with the provided identifier to a new item with the provided name.
        /// </summary>
        /// <param name="oldName">The old name.</param>
        /// <param name="newName">The new name.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">
        /// oldName
        /// or
        /// newName
        /// </exception>
        Task Copy(string oldName, string newName);

        /// <summary>
        /// Copies the specified item with the provided identifier to a new item with the provided name.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="newName">The new name.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">newName</exception>
        Task Copy(object id, string newName);

        /// <summary>
        /// Deletes the item with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">name</exception>
        Task Delete(string name);

        /// <summary>
        ///     Delete an item by the Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(object id);

        /// <summary>
        /// Get an item by Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<string> Get(string name);

        /// <summary>
        /// Get an item by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<string> Get(object id);

        /// <summary>
        /// Gets the identifier by the name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">name</exception>
        /// <exception cref="KeyNotFoundException"></exception>
        Task<object> GetId(string name);

        /// <summary>
        ///     Get a collection of items in a single JSON string.
        /// </summary>
        /// <returns></returns>
        Task<string> GetMany();

        /// <summary>
        /// Renames the item with the specified ID to newName
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="newName">The new name.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">newName</exception>
        Task Rename(object id, string newName);

        /// <summary>
        /// Renames the item specified by oldName to newName
        /// </summary>
        /// <param name="oldName">The old name.</param>
        /// <param name="newName">The new name.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">
        /// oldName
        /// or
        /// newName
        /// </exception>
        Task Rename(string oldName, string newName);

        /// <summary>
        /// Updates the specified content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">content</exception>
        Task Update(string content);
    }
}