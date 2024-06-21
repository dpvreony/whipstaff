using System;

namespace Whipstaff.Runtime.ValueObjects
{
    public class ProtectedPositiveIntegerValueObject : AbstractIntValueObject
    {
        protected ProtectedPositiveIntegerValueObject(int value)
            : base(value)
        {
        }

        protected override void EnsureValid(int value)
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Value must be greater than 0");
            }
        }
    }
}
