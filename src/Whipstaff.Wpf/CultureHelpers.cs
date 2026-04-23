// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using Whipstaff.Runtime.Extensions;

namespace Whipstaff.Wpf
{
    /// <summary>
    /// Helper methods for culture initialization in WPF applications. This is required to ensure that the correct culture is used for formatting and resource lookups in WPF applications.
    /// </summary>
    public static class CultureHelpers
    {
        /// <summary>
        /// Initializes the application's culture settings using the specified culture identifier.
        /// </summary>
        /// <remarks>Use this method to set the application's culture based on a numeric culture
        /// identifier, such as those defined by the CultureInfo class. This affects formatting of dates, numbers, and
        /// other culture-specific data throughout the application.</remarks>
        /// <param name="cultureId">The culture identifier, as an integer, that specifies the culture to initialize. Must correspond to a valid
        /// culture supported by the system.</param>
        public static void InitializeCulture(int cultureId)
        {
            var cultureInfo = new CultureInfo(cultureId);
            InternalInitializeCulture(cultureInfo);
        }

        /// <summary>
        /// Initializes the application's culture settings using the specified culture information.
        /// </summary>
        /// <param name="cultureInfo">The culture information to apply to the application's current culture. Cannot be null.</param>
        public static void InitializeCulture(CultureInfo cultureInfo)
        {
            ArgumentNullException.ThrowIfNull(cultureInfo);
            InternalInitializeCulture(cultureInfo);
        }

        /// <summary>
        /// Initializes the application's culture settings using the specified culture name.
        /// </summary>
        /// <remarks>This method sets the culture for the current context based on the provided culture
        /// name. Use this method to ensure that culture-specific formatting and resources are applied consistently
        /// throughout the application.</remarks>
        /// <param name="cultureName">The name of the culture to apply. Must be a valid culture identifier and cannot be null, empty, or consist
        /// only of white-space characters.</param>
        public static void InitializeCulture(string cultureName)
        {
            cultureName.ThrowIfNullOrWhitespace();

            var cultureInfo = new CultureInfo(cultureName);
            InternalInitializeCulture(cultureInfo);
        }

        private static void InternalInitializeCulture(CultureInfo cultureInfo)
        {
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    XmlLanguage.GetLanguage(cultureInfo.Name)));
        }
    }
}
