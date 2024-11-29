@echo off
setlocal

set SOURCE=github

for %%F in (Mtf.WPF.LanguageService.*.nupkg) do (
    echo Pushing package: %%F
    dotnet nuget push "%%F" --source "%SOURCE%"
    
    if errorlevel 1 (
        echo Failed to push package: %%F
        pause
        exit /b 1
    )
)

echo All packages have been pushed successfully.
pause