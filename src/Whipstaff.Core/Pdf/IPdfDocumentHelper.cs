// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Security.Cryptography.X509Certificates;
using Whipstaff.Core.PasswordGeneration;

namespace Whipstaff.Core.Pdf
{
    /// <summary>
    /// Helpers for working on PDF files.
    /// </summary>
    /// <typeparam name="TPdfDocument">The type of the PDF document used by the implementing library.</typeparam>
    public interface IPdfDocumentHelper<in TPdfDocument>
    {
        /// <summary>
        /// Applies the owner password to a pdf document.
        /// </summary>
        /// <param name="pdfDocument">The PDF document to change.</param>
        /// <param name="password">The password to apply.</param>
        void ApplyOwnerPassword(
            TPdfDocument pdfDocument,
            string password);

        /// <summary>
        /// Applies a digital signature to a PDF document.
        /// </summary>
        /// <param name="pdfDocument">The PDF document to change.</param>
        /// <param name="x509Certificate2">The digital certificate to use.</param>
        /// <param name="signatureName">The signature name to show and sign on the PDF.</param>
        /// <param name="contactInformation">The contact information to show and sign on the PDF.</param>
        /// <param name="locationInformation">The location information to show and sign on the PDF.</param>
        /// <param name="signingReason">The signing reason to show and sign on the PDF.</param>
        /// <param name="enableLongTermValidation">Whether to enable long term validation on the digital signature.</param>
        void ApplyDigitalSignature(
            TPdfDocument pdfDocument,
            X509Certificate2 x509Certificate2,
            string signatureName,
            string contactInformation,
            string locationInformation,
            string signingReason,
            bool enableLongTermValidation);

        /// <summary>
        /// Makes a PDF read only whilst using a randomly generated password.
        /// </summary>
        /// <param name="pdfDocument">The PDF document to change.</param>
        /// <param name="passwordGenerator">The password generator to use.</param>
        void MakeReadOnlyWithRandomPassword(
            TPdfDocument pdfDocument,
            IPasswordGenerator passwordGenerator);
    }
}
