Param
(
    $SourceDirectory = ".",
    $Targets = "restore,build,test",
    $Configuration = "Debug"
)

$targetNames = $Targets.Split(",")

foreach ($targetName in $targetNames)
{
    foreach ($item in (Get-ChildItem -Path "$SourceDirectory\*.sln"))
    {
        if ($targetName -eq "restore")
        {
            Write-Host "Restoring nuget packages"
            dotnet restore
            if (!$?)
            {
                exit -1
            }
        }
        elseif ($targetName -eq "build")
        {
            Write-Host "Building Minerva"
            dotnet build --no-restore --configuration $configuration
            if (!$?)
            {
                exit -1
            }
        }
        elseif ($targetName -eq "test")
        {
            Write-Host "Running tests"
            dotnet test --no-build --verbosity normal
            if (!$?)
            {
                exit -1
            }
        }
        elseif ($targetName -eq "codecoverage")
        {
            # TBD
        }
    }
}
