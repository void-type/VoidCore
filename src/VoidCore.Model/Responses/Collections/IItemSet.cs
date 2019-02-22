﻿using System.Collections.Generic;

namespace VoidCore.Model.Responses.Collections
{
    /// <summary>
    /// A DTO for sending a collection to the UI. Predictably brings items into memory from the database and counts them before sending them to the UI.
    /// </summary>
    /// <typeparam name="T">The entity type of the set</typeparam>
    public interface IItemSet<out T>
    {
        /// <summary>
        /// A getter for the number of the items in the set. If the set is null, will return 0.
        /// </summary>
        /// <returns>The number of items in a set</returns>
        int Count { get; }

        /// <summary>
        /// The items in the set.
        /// </summary>
        /// <value>The items in the set</value>
        IEnumerable<T> Items { get; }
    }
}
