using System;

namespace VSTSTool
{
    public enum ItemType
    {
        /// <summary>
        ///     Build Definition
        /// </summary>
        BuildDefinition,

        /// <summary>
        ///     Project
        /// </summary>
        Project,

        /// <summary>
        /// Release Definition
        /// </summary>
        ReleaseDefinition,

        /// <summary>
        ///     Git Repository
        /// </summary>
        Repository,

        /// <summary>
        ///     Task Group
        /// </summary>
        TaskGroup,

        /// <summary>
        ///     Variable Group
        /// </summary>
        VariableGroup
    }

    public enum Command
    {
        /// <summary>
        ///     Copy this item.
        /// </summary>
        Copy,

        /// <summary>
        ///     Create a new item.
        /// </summary>
        Create,

        /// <summary>
        ///     Fork a Git Repository.
        /// </summary>
        /// <remarks>Only applies to Git Repositories</remarks>
        CreateFork,

        /// <summary>
        ///     Delete this item.
        /// </summary>
        Delete,

        /// <summary>
        ///     Dump an item.
        /// </summary>
        Dump,

        /// <summary>
        ///     List items.
        /// </summary>
        List,

        /// <summary>
        ///     Rename an item.
        /// </summary>
        Rename
    }
}