@echo off
set projectDirectory=%cd%
set unityPath="C:\Progra~1\Unity\Hub\Editor\2019.1.14f1\Editor\Unity.exe"

dotnet publish Visualization\Web -c Release -o %projectDirectory%\..\Output
robocopy %projectDirectory%\Visualization\Common\Common.Communication\bin\Release %projectDirectory%\Simulation\App\Assets\Common
echo Building Simulation executables from Unity cli
%unityPath% -quit -batchmode -logFile ..\buildWindows.log -projectPath %projectDirectory%\Simulation\App -executeMethod SimulationBuilder.BuildWindows
%unityPath% -quit -batchmode -logFile ..\Output\Simulation\buildWebGL.log -projectPath %projectDirectory%\Simulation\App -executeMethod SimulationBuilder.BuildWebGL
robocopy %projectDirectory%\Simulation\App\out\Windows %projectDirectory%\..\Output\Simulation\Windows /E /XO
robocopy %projectDirectory%\Simulation\App\out\WebGL %projectDirectory%\..\Output\WebUI\dist\WebUI\assets\unity /E /XO