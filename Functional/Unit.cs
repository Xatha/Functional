namespace Functional
{
    public readonly struct Unit : IEquatable<Unit>, IComparable<Unit>
    {
        public static Unit Default => default;
        
        public static implicit operator Unit(ValueTuple _) => Default;
        
        // These methods and overrides gives Unit the required properties to be an empty set.
        public override int GetHashCode() => 0;
        public override bool Equals(object? obj) => obj is Unit;
        public override string ToString() => "Unit";
        public int CompareTo(Unit other) => 0;
        public bool Equals(Unit other) => true;                         
        public static bool operator ==(Unit left, Unit right) => true; 
        public static bool operator !=(Unit left, Unit right) => false;
        public static bool operator < (Unit left, Unit right) => false;
        public static bool operator > (Unit left, Unit right) => false;
        public static bool operator >=(Unit left, Unit right) => true; 
        public static bool operator <=(Unit left, Unit right) => true; 
    }
}

