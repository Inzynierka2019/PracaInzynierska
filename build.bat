@echo off
set projectDirectory=%cd%
set outputDirectory=%projectDirectory%\..\Output
set unityPath="C:\Progra~1\Unity\Hub\Editor\2019.1.14f1\Editor\Unity.exe"

echo Building Visualization with dotnet publish
dotnet publish Visualization\Web -c Release -o %projectDirectory%\..\Output
robocopy %outputDirectory% %projectDirectory%\Simulation\App\Assets\Common Common.Models.dll
robocopy %outputDirectory% %projectDirectory%\Simulation\App\Assets\Common Newtonsoft.Json.dll

echo Building Simulation executables from Unity cli
%unityPath% -quit -batchmode -logFile ..\Output\Simulation\buildWindows.log -projectPath %projectDirectory%\Simulation\App -executeMethod SimulationBuilder.BuildWindows
robocopy %projectDirectory%\Simulation\App\out\Windows %projectDirectory%\..\Output\Simulation\Windows /E /XO