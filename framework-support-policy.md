# Whipstaff support policy

The framework support policy is based on the Framework versioning policy of the following:

## MAUI Support Policy

The project tracks LTS versions of MAUI.
https://dotnet.microsoft.com/en-us/platform/support/policy/maui

# .NET Lifecycle

The project USUALLY tracks LTS versions of .NET
https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core

A NON-LTS version may be taken if there is a key fix that is required in the codebase.
Due to the complexities of support and the maintenance effort, USUALLY only ONE MAJOR version will be supported.

# Upstream projects (RX, RX-UI)

This project will only support one version upstream packages, which will typically be the latest version.

# Notes

When it comes to version alignment.
* MAUI introduces headaches, so some .NET packages may align to a NON LTS version. The principle driver for this will be ReactiveUI.
* The adoption of new MAJOR .NET versions will NOT USUALLY be in the first 2 calendar months of a release (for example most MAJOR releases are in November so the first adopting release would typically be January)