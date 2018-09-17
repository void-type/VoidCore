﻿namespace VoidCore.Model.Responses.File
{
    /// <summary>
    /// A view model for sending a file to be downloaded by the client.
    /// </summary>
    public interface ISimpleFile
    {
        /// <summary>
        /// The content bytes of the file.
        /// </summary>
        byte[] Content { get; }

        /// <summary>
        /// The name to save the file as on the client.
        /// </summary>
        /// <value></value>
        string Name { get; }

        /// <summary>
        /// Decodes the content bytes as a UTF8 string.
        /// </summary>
        /// <value></value>
        string ContentAsString { get; }
    }
}