using System;

namespace VoidCore.Model.Data
{
    /// <summary>
    /// Can save changes through a disposable connection to some form of persistence.
    /// </summary>
    public interface IPersistable : IDisposable
    {
        /// <summary>
        /// Commit changes to persistence.
        /// </summary>
        void SaveChanges();
    }
}
