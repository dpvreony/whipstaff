// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace Whipstaff.Runtime.TypeConversion
{
    /// <summary>
    /// Helper methods for <see cref="TypeDescriptor"/>.
    /// </summary>
    public static class TypeDescriptorHelpers
    {
#if !IOS && !ANDROID && !TVOS && !MACOS
        /// <summary>
        /// Adds a type converter to the type descriptor. This wrapper allows constraining the type converter to a subclass of <see cref="TypeConverter"/> where the built in
        /// .NET framework doesn't do this.
        /// </summary>
        /// <typeparam name="TTarget">The target type for the conversion.</typeparam>
        /// <typeparam name="TConvertor">The type that carries out the conversion.</typeparam>
        /// <returns>The newly created <see cref="TypeDescriptionProvider"/> that was used to add the specified attributes.</returns>
        public static TypeDescriptionProvider AddConverter<TTarget, TConvertor>()
            where TConvertor : TypeConverter
        {
            return TypeDescriptor.AddAttributes(
                typeof(TTarget),
                new TypeDescriptionProviderAttribute(typeof(TConvertor)));
        }
#endif
    }
}
