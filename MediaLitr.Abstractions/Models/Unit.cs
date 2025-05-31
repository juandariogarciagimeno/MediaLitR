// --------------------------------------------------------------------------------------------------
// <copyright file="Unit.cs" company="juandariogg">
// Licensed under the MIT license. See LICENSE file in the samples root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

namespace MediaLitr.Abstractions.Models;

using System.Runtime.InteropServices;

/// <summary>
/// Represents a unit type, replacement for void types.
/// </summary>
[StructLayout(LayoutKind.Sequential, Size = 1)]
public readonly struct Unit : IEquatable<Unit>, IComparable<Unit>, IComparable
{
    public static Task<Unit> CompletedTask => Task.FromResult(default(Unit));

    public static bool operator ==(Unit left, Unit right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Unit left, Unit right)
    {
        return !(left == right);
    }

    public static bool operator <(Unit left, Unit right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(Unit left, Unit right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(Unit left, Unit right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(Unit left, Unit right)
    {
        return left.CompareTo(right) >= 0;
    }

    public int CompareTo(object? obj)
    {
        return 0;
    }

    public int CompareTo(Unit other)
    {
        return 0;
    }

    public bool Equals(Unit other)
    {
        return true;
    }

    public override bool Equals(object? obj)
    {
        return obj is Unit;
    }

    public override int GetHashCode()
    {
        return 0;
    }
}
