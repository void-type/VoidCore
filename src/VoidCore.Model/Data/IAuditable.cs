using System;

namespace VoidCore.Model.Data;

/// <summary>
/// An entity that keeps track of created and modified information.
/// </summary>
public interface IAuditable
{
    /// <summary>
    /// A string representing the creator of the entity
    /// </summary>
    string CreatedBy { get; set; }

    /// <summary>
    /// The date and time the entity was created
    /// </summary>
    DateTime CreatedOn { get; set; }

    /// <summary>
    /// A string representing the last modifier of the entity
    /// </summary>
    string ModifiedBy { get; set; }

    /// <summary>
    /// The date and time the entity was last modified
    /// </summary>
    DateTime ModifiedOn { get; set; }
}
