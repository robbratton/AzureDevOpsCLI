using System;

namespace DevOpsTools
{
    public class Validators : IValidators
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
        public void ValidateId(object id)
        {
            switch (id)
            {
                case Guid x:
                    if (x == Guid.Empty)
                    {
                        throw new ArgumentException(nameof(id) + " must not be Guid.Empty.");
                    }

                    break;
                case long x:
                    if (x < 0)
                    {
                        throw new ArgumentException(nameof(id) + " must be >= 0.");
                    }

                    break;
                default:
                    throw new InvalidOperationException("Unsupported type " + id.GetType().Name);
            }
        }
    }
}
