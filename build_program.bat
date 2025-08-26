@echo off
REM Build script: build_program.bat [D|N]
REM   D = Debug (default)
REM   N = Normal/Release

setlocal ENABLEDELAYEDEXPANSION

set MODE=%1
if "%MODE%"=="" set MODE=D

if /I "%MODE%"=="D" (
    set BUILD_TYPE=Debug
    set BUILD_DIR=build_debug
) else if /I "%MODE%"=="N" (
    set BUILD_TYPE=Release
    set BUILD_DIR=build_release
) else (
    echo Usage: %~n0 [D^|N]
    echo   D = Debug build
    echo   N = Release build
    exit /b 1
)

echo ======================================
echo   Building %BUILD_TYPE% in %BUILD_DIR%
echo ======================================

if not exist %BUILD_DIR% mkdir %BUILD_DIR%

cmake -G "MinGW Makefiles" -S . -B %BUILD_DIR% -DCMAKE_BUILD_TYPE=%BUILD_TYPE% || goto :error
cmake --build %BUILD_DIR% -- -j %NUMBER_OF_PROCESSORS% || goto :error

echo.
echo Build finished: %BUILD_DIR%\CNC_3D.exe
echo Done.
exit /b 0

:error
echo Build FAILED (%errorlevel%).
exit /b %errorlevel%
