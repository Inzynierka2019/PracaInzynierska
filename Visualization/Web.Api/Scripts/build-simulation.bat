@echo off
set unityPath=%1
set projectPath=%2

echo Unity.exe directory: %unityPath%
echo Project directory: %projectPath%
echo Working directory: %cd%

@echo on
call %unityPath% -quit -batchmode -logFile stdout.log -projectPath %projectPath% -executeMethod SimulationBuilder.BuildWindows