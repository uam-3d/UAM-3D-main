using System;

namespace UAM3D.Types;

/// <summary>
/// An Attribute to mark a given expression as temporary.
/// </summary>
/// <param name="dateAdded">The date when the given expression was added.</param>
/// <param name="dateToRemove">The date by when the given expression should be removed.</param>
[
    AttributeUsage
    (
        AttributeTargets.Class
        | AttributeTargets.Constructor
        | AttributeTargets.Delegate
        | AttributeTargets.Enum
        | AttributeTargets.Event
        | AttributeTargets.Field
        | AttributeTargets.Interface
        | AttributeTargets.Method
        | AttributeTargets.Property
        | AttributeTargets.Struct,
        Inherited = false
    )
]
public sealed class TempAttribute(string dateAdded = null, string dateToRemove = null) : Attribute
{
    private string _dateAdded = dateAdded;
    private string _dateToRemove = dateToRemove;
    
}