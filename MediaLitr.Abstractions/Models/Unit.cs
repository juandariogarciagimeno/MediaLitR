using System.Runtime.InteropServices;

namespace MediaLitr.Abstractions.Models
{
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public readonly struct Unit : IEquatable<Unit>, IComparable<Unit>, IComparable
    {
        public static Task<Unit> CompletedTask => Task.FromResult(new Unit());

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

        override public bool Equals(object? obj)
        {
            return obj is Unit;
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}
