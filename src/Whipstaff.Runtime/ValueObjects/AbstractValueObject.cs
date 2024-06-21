using System;
using System.Collections.Generic;
using System.Text;

namespace Whipstaff.Runtime.ValueObjects
{
    public abstract class AbstractValueObject<T>
    {
        protected AbstractValueObject(T value)
        {
            EnsureValid(value);
            Value = value;
        }

        public T Value { get; }

        protected abstract void EnsureValid(T value);
    }
}
