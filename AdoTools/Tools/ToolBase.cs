using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevOpsTools.Tools.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DevOpsTools.Tools
{
    /// <summary>
    ///     Base to be used when creating Azure DevOps Tool Classes.
    /// </summary>
    public abstract class ToolBase : ITool
    {
        #region Constructors

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="client">Azure DevOps Client</param>
        /// <param name="project">Project</param>
        /// <param name="basePath">URI Base Path</param>
        /// <param name="apiVersionSuffix">API Version Suffix</param>
        protected ToolBase(
            IClient client,
            string project,
            string basePath,
            string apiVersionSuffix)
        {
            if (string.IsNullOrWhiteSpace(basePath))
            {
                throw new ArgumentNullException(nameof(basePath));
            }

            if (string.IsNullOrWhiteSpace(apiVersionSuffix))
            {
                throw new ArgumentNullException(nameof(apiVersionSuffix));
            }

            if (string.IsNullOrWhiteSpace(project))
            {
                throw new ArgumentNullException(nameof(project));
            }

            Client = client ?? throw new ArgumentNullException(nameof(client));
            
            BasePath = basePath;

            _apiVersionSuffix = apiVersionSuffix;
            Project = project;
        }

        #endregion Constructors

        public virtual async Task Create(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException(nameof(content));
            }

            var uri = Helpers.MakeUri(BasePath, _apiVersionSuffix, "", "");

            await Client.PostStringAsync(uri, content).ConfigureAwait(false);
        }

        ///<inheritdoc/>
        public async Task Copy(string oldName, string newName)
        {
            // Force implementing class to implement it's own copy/clone.
            await CopyImpl(oldName, newName);
        }

        /// <summary>
        /// Implements the Copy for the specific tool
        /// </summary>
        /// <param name="oldName">The old name.</param>
        /// <param name="newName">The new name.</param>
        /// <returns></returns>
        protected abstract Task CopyImpl(string oldName, string newName);
        //{
        //    if (string.IsNullOrWhiteSpace(oldName))
        //    {
        //        throw new ArgumentException(nameof(oldName));
        //    }

        //    if (string.IsNullOrWhiteSpace(newName))
        //    {
        //        throw new ArgumentException(nameof(newName));
        //    }

        //    var id = await GetId(oldName).ConfigureAwait(false);

        //    await Copy(id, newName).ConfigureAwait(false);
        //}

        ///<inheritdoc/>
        public async Task Copy(object id, string newName)
        {
            // Force implementing class to implement it's own copy/clone.
            await CopyImpl(id, newName);
        }

        /// <summary>
        /// Implements the Copy for the specific tool
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="newName">The new name.</param>
        /// <returns></returns>
        protected abstract Task CopyImpl(object id, string newName);
        //{
        //    ValidateId(id);

        //    if (string.IsNullOrWhiteSpace(newName))
        //    {
        //        throw new ArgumentException(nameof(newName));
        //    }

        //    var oldDefinition = await Get(id).ConfigureAwait(false);
        //    dynamic updatedDefDynamic = JsonConvert.DeserializeObject(oldDefinition);

        //    updatedDefDynamic.id.Value = null;
        //    updatedDefDynamic.name.Value = newName;

        //    string updatedDefinition = JsonConvert.SerializeObject(updatedDefDynamic);

        //    await Create(updatedDefinition).ConfigureAwait(false);
        //}

        ///<inheritdoc/>
        public virtual async Task Delete(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(nameof(name));
            }

            var id = await GetId(name).ConfigureAwait(false);

            await Delete(id).ConfigureAwait(false);
        }

        ///<inheritdoc/>
        public virtual async Task Delete(object id)
        {
            new Validators().ValidateId(id);

            var uri = Helpers.MakeUri(BasePath, _apiVersionSuffix, id.ToString(), "");

            await Client.DeleteAsync(uri).ConfigureAwait(false);
        }

        ///<inheritdoc/>
        public virtual async Task<string> Get(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(nameof(name));
            }

            var id = await GetId(name).ConfigureAwait(false);

            return await Get(id).ConfigureAwait(false);
        }

        ///<inheritdoc/>
        public virtual Task<string> Get(object id)
        {
            new Validators().ValidateId(id);

            var uri = Helpers.MakeUri(BasePath, _apiVersionSuffix, id.ToString(), "");

            var result = Client.GetStringAsync(uri).Result;

            var token = JToken.Parse(result);

            switch (token)
            {
                case JArray ar:
                    result = JsonConvert.SerializeObject(ar[0], Formatting.Indented);
                    break;
                case JObject _ when token["value"] is JArray ar2:
                    result = JsonConvert.SerializeObject(ar2[0], Formatting.Indented);
                    break;
                case JObject jo:
                    result = JsonConvert.SerializeObject(jo, Formatting.Indented);
                    break;
                default:
                    throw new Exception("Cannot parse request result.");
            }

            return Task.FromResult(result);
        }

        ///<inheritdoc/>
        public virtual async Task<object> GetId(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(nameof(name));
            }

            var result = await GetMany().ConfigureAwait(false);

            dynamic items = JsonConvert.DeserializeObject(result);

            object output = null;

            foreach (var item in items.value)
            {
                if (name.Equals(item.name.Value, StringComparison.CurrentCultureIgnoreCase))
                {
                    output = item.id.Value is string && Guid.TryParse(item.id.Value, out Guid _)
                        ? Guid.Parse(item.id.Value)
                        : item.id.Value;
                    break;
                }
            }
        
            if (output == null)
            {
                throw new KeyNotFoundException($"{ToolItem} with name '{name}' was not found.");
            }

            return output;
        }

        ///<inheritdoc/>
        public async Task<string> GetMany()
        {
            var uri = Helpers.MakeUri(BasePath, _apiVersionSuffix, "", "");

            return await Client.GetStringAsync(uri).ConfigureAwait(false);
        }

        ///<inheritdoc/>
        public async Task Rename(object id, string newName)
        {
            new Validators().ValidateId(id);

            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException(nameof(newName));
            }

            var oldDefinition = await Get(id).ConfigureAwait(false);
            dynamic updatedDefDynamic = JsonConvert.DeserializeObject(oldDefinition);

            updatedDefDynamic.name.Value = newName;

            string updatedDefinition = JsonConvert.SerializeObject(updatedDefDynamic);

            await Update(updatedDefinition).ConfigureAwait(false);
        }

        ///<inheritdoc/>
        public async Task Rename(string oldName, string newName)
        {
            if (string.IsNullOrWhiteSpace(oldName))
            {
                throw new ArgumentException(nameof(oldName));
            }

            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException(nameof(newName));
            }

            var oldDefinition = await Get(oldName).ConfigureAwait(false);
            dynamic updatedDefDynamic = JsonConvert.DeserializeObject(oldDefinition);

            updatedDefDynamic.name.Value = newName;

            string updatedDefinition = JsonConvert.SerializeObject(updatedDefDynamic);

            await Update(updatedDefinition).ConfigureAwait(false);
        }

        ///<inheritdoc/>
        public async Task Update(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException(nameof(content));
            }

            var uri = Helpers.MakeUri(BasePath, _apiVersionSuffix, "", "");

            await Client.PutStringAsync(uri, content).ConfigureAwait(false);
        }

        #region Private and Protected Fields

        /// <summary>
        ///     The query suffix which defines the version of the Azure DevOps API
        /// </summary>
        /// <example>api-version=5.0-preview.7</example>
        private readonly string _apiVersionSuffix;

        /// <summary>
        ///     The Azure DevOps Client
        /// </summary>
        protected readonly IClient Client;
 
        /// <summary>
        ///     Azure DevOps Project used in the URI
        /// </summary>
        protected readonly string Project;

        /// <summary>
        ///     The base path for each tool
        /// </summary>
        protected string BasePath { get; set; }

        protected string ToolItem;

        #endregion Private and Protected Fields
    }
}