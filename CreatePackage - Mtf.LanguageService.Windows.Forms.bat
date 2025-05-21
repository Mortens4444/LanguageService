@echo off
setlocal enabledelayedexpansion

set "ProjectName=Mtf.LanguageService.Windows.Forms"
set "TargetDir=C:\NuGetTest"

for %%F in ("%TargetDir%\%ProjectName%.*.nupkg") do (
    set "FileName=%%~nxF"
    if not "!FileName!" == "!FileName:.symbols=!" (
        echo Deleting: %%F
        del "%%F"
    )
)

for %%F in ("%TargetDir%\%ProjectName%.*.nupkg") do call :ProcessFile "%%F"
goto :UpdatePackages

:ProcessFile
set "FilePath=%~1"
set "FileName=%~nx1"

call set "RestPart=%%FileName:%ProjectName%.=%%"
echo !RestPart! | findstr /R "^[0-9][0-9]*\.[0-9][0-9]*\.[0-9][0-9]*" >nul

if !errorlevel! == 0 (
    echo Deleting: !FilePath!
    del "!FilePath!"
)

goto :eof

:UpdatePackages

for /R %%P in (*.csproj) do (
    echo Checking: %%~nxP
    pushd %%~dpP
    FOR /F "tokens=1,2,*" %%A IN ('dotnet list package --outdated --source "%TargetDir%"') DO (
        IF "%%A"==">" (
            set "PackageName=%%B"
            echo   Updating: !PackageName! in %%~nxP
            dotnet add package !PackageName! -s "%TargetDir%"
            IF ERRORLEVEL 1 (
                echo     Error: !PackageName! update failed.
            ) ELSE (
                echo     OK: !PackageName! update success.
            )
            echo.
        )
    )
    popd
)

powershell.exe -ExecutionPolicy Bypass -File ".\IncrementPackageVersion.ps1" -CsprojFile "%ProjectName%\%ProjectName%.csproj"
dotnet pack --include-symbols --include-source %ProjectName%\%ProjectName%.csproj -c Release /p:IncludeSymbols=true /p:IncludeSource=true /p:DebugType=full /p:EmbedAllSources=true /p:Deterministic=true /bl
move .\%ProjectName%\bin\Release\*.nupkg %TargetDir%
REM pause