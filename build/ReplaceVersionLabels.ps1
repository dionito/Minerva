# <copyright company="Microsoft">
#   Copyright (c) Microsoft Corporation.  All rights reserved.
# </copyright>

<#
.SYNOPSIS

Replaces version label sentinels [[VersionLabel]] with the specified label value.

.PARAMETER RootPath

The path under which JSON and XML files will have their version labels replaced.

.PARAMETER Label

The value to replace in the files

#>
param
(
    [Parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [String]
    $RootPath,

    [Parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [String]
    $Label
)

$files = Get-ChildItem -Path $RootPath -Include "*.json", "*.xml" -Recurse;
foreach ($file in $files)
{
    $content = Get-Content -Encoding "UTF8" -Raw $file.FullName;
    if (-not $content.Contains("[[VersionLabel]]"))
    {
        continue;
    }
    
    Write-Host "Replacing content for $($file.FullName)";
    $content.Replace("[[VersionLabel]]", $Label) | Out-File -Encoding "UTF8" $file.FullName;
} 