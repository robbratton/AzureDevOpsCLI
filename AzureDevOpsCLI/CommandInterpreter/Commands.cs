using DevOpsTools.Tools.Interfaces;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text.RegularExpressions;

// ReSharper disable UnusedMember.Global

namespace VSTSTool.CommandInterpreter
{
    /// <summary>
    ///     Commands handled by this tool.
    /// </summary>
    public class Commands : ICommands
    {
        #region Constructor

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="buildDefinitionTool"></param>
        /// <param name="releaseDefinitionTool"></param>
        /// <param name="projectTool"></param>
        /// <param name="repositoryTool"></param>
        /// <param name="taskGroupTool"></param>
        /// <param name="variableGroupTool"></param>
        public Commands(
            IBuildDefinitionTool buildDefinitionTool,
            IReleaseDefinitionTool releaseDefinitionTool,
            IProjectTool projectTool,
            IRepositoryTool repositoryTool,
            ITaskGroupTool taskGroupTool,
            IVariableGroupTool variableGroupTool
        )
        {
            _buildDefinitionTool = buildDefinitionTool ?? throw new ArgumentException(nameof(buildDefinitionTool));
            _releaseDefinitionTool = releaseDefinitionTool ?? throw new ArgumentException(nameof(releaseDefinitionTool));
            _repositoryTool = repositoryTool ?? throw new ArgumentException(nameof(repositoryTool));
            _projectTool = projectTool ?? throw new ArgumentException(nameof(projectTool));
            _taskGroupTool = taskGroupTool ?? throw new ArgumentException(nameof(taskGroupTool));
            _variableGroupTool = variableGroupTool ?? throw new ArgumentException(nameof(taskGroupTool));
        }

        #endregion Constructor

        #region Private

        private readonly IBuildDefinitionTool _buildDefinitionTool;
        private readonly IReleaseDefinitionTool _releaseDefinitionTool;
        private readonly IProjectTool _projectTool;
        private readonly IRepositoryTool _repositoryTool;
        private readonly ITaskGroupTool _taskGroupTool;
        private readonly IVariableGroupTool _variableGroupTool;

        #endregion Private

        #region Commands

        /// <inheritdoc />
        public void Copy(ItemType itemType, string sourceName, string destName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sourceName))
                {
                    throw new ArgumentException(nameof(sourceName) + " must not be null or whitespace.");
                }

                if (string.IsNullOrWhiteSpace(destName))
                {
                    throw new ArgumentException(nameof(destName) + " must not be null or whitespace.");
                }

                switch (itemType)
                {
                    case ItemType.BuildDefinition:
                        _buildDefinitionTool.Copy(sourceName, destName).Wait();
                        break;
                    case ItemType.ReleaseDefinition:
                        _releaseDefinitionTool.Copy(sourceName, destName).Wait();
                        break;
                    case ItemType.TaskGroup:
                        _taskGroupTool.Copy(sourceName, destName).Wait();
                        break;
                    case ItemType.Project:
                        _projectTool.Copy(sourceName, destName).Wait();
                        break;
                    case ItemType.Repository:
                        _repositoryTool.Copy(sourceName, destName).Wait();
                        break;
                    case ItemType.VariableGroup:
                        _variableGroupTool.Copy(sourceName, destName).Wait();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(itemType));
                }
            }
            catch (Exception exception)
            {
                WriteException("Copy", exception);
                throw;
            }
        }

        /// <inheritdoc />
        public void Create(ItemType itemType, string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    throw new ArgumentException($"{nameof(path)} must not be null or whitespace.");
                }

                if (!File.Exists(path))
                {
                    throw new ArgumentException($"{nameof(path)} '{path}' must exist.");
                }

                var content = File.ReadAllText(path);

                if (string.IsNullOrWhiteSpace(content))
                {
                    throw new ArgumentException($"Content of {nameof(path)} must not be blank.");
                }

                switch (itemType)
                {
                    case ItemType.BuildDefinition:
                        _buildDefinitionTool.Create(content).Wait();
                        break;
                    case ItemType.ReleaseDefinition:
                        _releaseDefinitionTool.Create(content).Wait();
                        break;
                    case ItemType.Project:
                        _projectTool.Create(content).Wait();
                        break;
                    case ItemType.Repository:
                        _repositoryTool.Create(content).Wait();
                        break;
                    case ItemType.TaskGroup:
                        _taskGroupTool.Create(content).Wait();
                        break;
                    case ItemType.VariableGroup:
                        _variableGroupTool.Create(content).Wait();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(itemType));
                }
            }
            catch (Exception exception)
            {
                WriteException("Create", exception);
                throw;
            }
        }

        /// <inheritdoc />
        public void CreateFork(ItemType itemType, string sourceName, string destName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sourceName))
                {
                    throw new ArgumentException(nameof(sourceName) + " must not be null or whitespace.");
                }

                if (string.IsNullOrWhiteSpace(destName))
                {
                    throw new ArgumentException(nameof(destName) + " must not be null or whitespace.");
                }

                switch (itemType)
                {
                    case ItemType.Repository:
                        _repositoryTool.CreateFork(sourceName, destName).Wait();
                        break;
                    case ItemType.BuildDefinition:
                    case ItemType.ReleaseDefinition:
                    case ItemType.Project:
                    case ItemType.TaskGroup:
                    case ItemType.VariableGroup:
                        //Console.Error.WriteLine($"CreateFork is not a valid command for {itemType}");
                        throw new InvalidOperationException($"CreateFork is not a valid command for {itemType}");
                    default:
                        //Console.Error.WriteLine("itemType of " + itemType + " is not handled.");
                        throw new ArgumentOutOfRangeException(nameof(itemType));
                }
            }
            catch (Exception exception)
            {
                WriteException("CreateFork", exception);
                throw;
            }
        }

        /// <inheritdoc />
        public void Rename(ItemType itemType, string oldName, string newName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(oldName))
                {
                    throw new ArgumentException(nameof(oldName) + " must not be null or whitespace.");
                }

                if (string.IsNullOrWhiteSpace(newName))
                {
                    throw new ArgumentException(nameof(newName) + " must not be null or whitespace.");
                }

                switch (itemType)
                {
                    case ItemType.BuildDefinition:
                        _buildDefinitionTool.Rename(oldName, newName).Wait();
                        break;
                    case ItemType.ReleaseDefinition:
                        _releaseDefinitionTool.Rename(oldName, newName).Wait();
                        break;
                    case ItemType.Project:
                        _projectTool.Rename(oldName, newName).Wait();
                        break;
                    case ItemType.Repository:
                        _repositoryTool.Rename(oldName, newName).Wait();
                        break;
                    case ItemType.TaskGroup:
                        _taskGroupTool.Rename(oldName, newName).Wait();
                        break;
                    case ItemType.VariableGroup:
                        _variableGroupTool.Rename(oldName, newName).Wait();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(itemType));
                }
            }
            catch (Exception exception)
            {
                WriteException("Rename", exception);
                throw;
            }
        }

        /// <inheritdoc />
        public void List(ItemType itemType, string pattern = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(pattern))
                {
                    pattern = ".*";
                }

                dynamic items;

                switch (itemType)
                {
                    case ItemType.BuildDefinition:
                        items = JsonConvert.DeserializeObject(_buildDefinitionTool.GetMany().Result);
                        break;
                    case ItemType.ReleaseDefinition:
                        items = JsonConvert.DeserializeObject(_releaseDefinitionTool.GetMany().Result);
                        break;
                    case ItemType.Project:
                        items = JsonConvert.DeserializeObject(_projectTool.GetMany().Result);
                        break;
                    case ItemType.Repository:
                        items = JsonConvert.DeserializeObject(_repositoryTool.GetMany().Result);
                        break;
                    case ItemType.TaskGroup:
                        items = JsonConvert.DeserializeObject(_taskGroupTool.GetMany().Result);
                        break;
                    case ItemType.VariableGroup:
                        items = JsonConvert.DeserializeObject(_variableGroupTool.GetMany().Result);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(itemType));
                }

                var filterRegex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                foreach (var item in items.value)
                {
                    if (filterRegex.IsMatch(item.name.Value))
                    {
                        Console.WriteLine(item.name.Value + " " + item.id.Value);
                    }
                }
            }
            catch (Exception exception)
            {
                WriteException("List", exception);
                throw;
            }
        }

        /// <inheritdoc />
        public void Dump(ItemType itemType, string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException(nameof(name) + " must not be null or whitespace.");
                }

                string output;

                switch (itemType)
                {
                    case ItemType.BuildDefinition:
                        output = _buildDefinitionTool.Get(name).Result;
                        break;
                    case ItemType.ReleaseDefinition:
                        output = _releaseDefinitionTool.Get(name).Result;
                        break;
                    case ItemType.Project:
                        output = _projectTool.Get(name).Result;
                        break;
                    case ItemType.Repository:
                        output = _repositoryTool.Get(name).Result;
                        break;
                    case ItemType.TaskGroup:
                        output = _taskGroupTool.Get(name).Result;
                        break;
                    case ItemType.VariableGroup:
                        output = _variableGroupTool.Get(name).Result;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(itemType));
                }

                dynamic item = JsonConvert.DeserializeObject(output);

                Console.WriteLine(item);
            }
            catch (Exception exception)
            {
                WriteException("Dump", exception);
                throw;
            }
        }

        /// <inheritdoc />
        public void Delete(ItemType itemType, string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException(nameof(name) + " must not be null or whitespace.");
                }

                switch (itemType)
                {
                    case ItemType.BuildDefinition:
                        _buildDefinitionTool.Delete(name).Wait();
                        break;
                    case ItemType.ReleaseDefinition:
                        _releaseDefinitionTool.Delete(name).Wait();
                        break;
                    case ItemType.Project:
                        _projectTool.Delete(name).Wait();
                        break;
                    case ItemType.Repository:
                        _repositoryTool.Delete(name).Wait();
                        break;
                    case ItemType.TaskGroup:
                        _taskGroupTool.Delete(name).Wait();
                        break;
                    case ItemType.VariableGroup:
                        _variableGroupTool.Delete(name).Wait();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(itemType));
                }
            }
            catch (Exception exception)
            {
                WriteException("Delete", exception);
                throw;
            }
        }

        private static void WriteException(string command, Exception exception)
        {
            Console.Error.WriteLine($"{command} failed:");
            if (exception is AggregateException ae)
            {
                foreach (var item in ae.InnerExceptions)
                {
                    Console.Error.WriteLine($"  {item.Message}");
                    WriteInnerExceptions(item.InnerException, 2);
                }
            }
            else
            {
                Console.Error.WriteLine($"  {exception.Message}");
                WriteInnerExceptions(exception.InnerException, 2);
            }
        }

        private static void WriteInnerExceptions(Exception exception, int indent)
        {
            if (exception == null)
            {
                return;
            }

            while (true)
            {
                var indentString = Helper.MakeIndentString(indent);

                Console.Error.WriteLine($"{indentString}{exception.InnerException.Message}");
                if (exception.InnerException != null)
                {
                    exception = exception.InnerException;
                    indent += 1;
                    continue;
                }

                break;
            }
        }

        #endregion Commands
    }
}