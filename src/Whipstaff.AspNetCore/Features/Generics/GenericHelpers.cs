﻿// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Whipstaff.AspNetCore.Features.Generics
{
    /// <summary>
    /// Helper methods for dealing with generics.
    /// </summary>
    public static class GenericHelpers
    {
        /// <summary>
        /// Checks to see if a class is a subclass of a generic class.
        /// </summary>
        /// <remarks>
        /// taken from https://stackoverflow.com/questions/457676/check-if-a-class-is-derived-from-a-generic-class.
        /// </remarks>
        /// <param name="generic">The generic class type.</param>
        /// <param name="toCheck">The type to check.</param>
        /// <returns>Whether the class is a subclass of a specific generic.</returns>
        public static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            Type? temp = toCheck;
            while (temp != null && temp != typeof(object))
            {
                var cur = temp.IsGenericType ? temp.GetGenericTypeDefinition() : temp;
                if (generic == cur)
                {
                    return true;
                }

                temp = temp.BaseType;
            }

            return false;
        }
    }
}
