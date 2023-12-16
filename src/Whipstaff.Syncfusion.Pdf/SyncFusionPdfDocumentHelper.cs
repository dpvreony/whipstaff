// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Security.Cryptography.X509Certificates;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;
using Whipstaff.Core.PasswordGeneration;
using Whipstaff.Core.Pdf;
using Whipstaff.Runtime.Extensions;

namespace Whipstaff.Syncfusion.Pdf
{
    /// <summary>
    /// Sync Fusion PDF Helpers.
    /// </summary>
    public sealed class SyncFusionPdfDocumentHelper : IPdfDocumentHelper<PdfLoadedDocument>
    {
        /// <inheritdoc/>
        public void ApplyOwnerPassword(
            PdfLoadedDocument pdfDocument,
            string password)
        {
            ArgumentNullException.ThrowIfNull(pdfDocument);
            password.ThrowIfNullOrWhitespace();

            var security = pdfDocument.Security;

            security.Algorithm = PdfEncryptionAlgorithm.AES;
            security.KeySize = PdfEncryptionKeySize.Key256BitRevision6;
            security.OwnerPassword = password;
            security.Permissions = PdfPermissionsFlags.Default;
        }

        /// <inheritdoc/>
        public void ApplyDigitalSignature(
            PdfLoadedDocument pdfDocument,
            X509Certificate2 x509Certificate2,
            string signatureName,
            string contactInformation,
            string locationInformation,
            string signingReason,
            bool enableLongTermValidation)
        {
            ArgumentNullException.ThrowIfNull(pdfDocument);

            var page = pdfDocument.Pages[0];
            var cert = new PdfCertificate(x509Certificate2);

            // ReSharper disable once UnusedVariable
            var pdfSignature = new PdfSignature(pdfDocument, page, cert, signatureName)
            {
                DocumentPermissions = PdfCertificationFlags.ForbidChanges,
                ContactInfo = contactInformation,
                LocationInfo = locationInformation,
                Reason = signingReason,
            };

            if (enableLongTermValidation)
            {
                pdfSignature.EnableLtv = true;
            }
        }

        /// <inheritdoc/>
        public void MakeReadOnlyWithRandomPassword(
            PdfLoadedDocument pdfDocument,
            IPasswordGenerator passwordGenerator)
        {
            ArgumentNullException.ThrowIfNull(pdfDocument);
            ArgumentNullException.ThrowIfNull(passwordGenerator);

            var password = passwordGenerator.GetPassword();
            ApplyOwnerPassword(pdfDocument, password);
        }
    }
}
