using System;
using System.ComponentModel;

namespace Whipstaff.Runtime.TypeConversion
{
    /// <summary>
    /// Helper methods for <see cref="TypeDescriptor"/>.
    /// </summary>
    public static class TypeDescriptorHelpers
    {
        /// <summary>
        /// Adds a type convertor to the type descriptor.
        /// </summary>
        /// <typeparam name="TTarget">The target type for the conversion.</typeparam>
        /// <typeparam name="TConvertor">The type that carries out the conversion.</typeparam>
        /// <returns>The newly created <see cref="TypeDescriptionProvider"/> that was used to add the specified attributes.</returns>
        public static TypeDescriptionProvider AddConvertor<TTarget, TConvertor>()
            where TConvertor : TypeConverter
        {
            return TypeDescriptor.AddAttributes(
                typeof(TTarget),
                new Attribute[] { new TypeDescriptionProviderAttribute(typeof(TConvertor)) });
        }
    }
}
