using System;

// ReSharper disable UnusedMemberInSuper.Global

namespace DevOpsTools
{
    public interface IValidators
    {
        /// <summary>
        /// Validates the identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <exception cref="ArgumentException">
        /// id
        /// or
        /// id
        /// </exception>
        /// <exception cref="InvalidOperationException">Unsupported type " + id.GetType().Name</exception>
        void ValidateId(object id);
    }
}