// ReSharper disable UnusedMember.Global

using System;

namespace VSTSTool.CommandInterpreter
{
    public interface ICommands
    {
        /// <summary>
        ///     Copy an Item
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="sourceName"></param>
        /// <param name="destName"></param>
        void Copy(ItemType itemType, string sourceName, string destName);

        /// <summary>
        ///     Create an item from a file.
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="path"></param>
        void Create(ItemType itemType, string path);

        /// <summary>
        ///     Create a fork of a Git Repository
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="sourceName"></param>
        /// <param name="destName"></param>
        void CreateFork(ItemType itemType, string sourceName, string destName);

        /// <summary>
        ///     Delete an Item
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="name"></param>
        void Delete(ItemType itemType, string name);

        /// <summary>
        ///     Dump an Item
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="name"></param>
        void Dump(ItemType itemType, string name);

        /// <summary>
        ///     List Items
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="pattern">optional</param>
        void List(ItemType itemType, string pattern = null);

        /// <summary>
        ///     Rename an Item
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        void Rename(ItemType itemType, string oldName, string newName);
    }
}