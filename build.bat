@echo off
set projectDirectory=%cd%
set unityPath="C:\Progra~1\Unity\Hub\Editor\2019.1.14f1\Editor\Unity.exe"

call dotnet publish Visualization\Web -c Release -o %projectDirectory%\..\Output
call %unityPath% -quit -batchmode -logFile ..\buildWindows.log -projectPath %projectDirectory%\Simulation\App -executeMethod SimulationBuilder.BuildWindows
call %unityPath% -quit -batchmode -logFile ..\Output\Simulation\buildWebGL.log -projectPath %projectDirectory%\Simulation\App -executeMethod SimulationBuilder.BuildWebGL
call robocopy %projectDirectory%\Simulation\App\out\Windows %projectDirectory%\..\Output\Simulation\Windows /E /XO
call robocopy %projectDirectory%\Simulation\App\out\WebGL %projectDirectory%\..\Output\Simulation\WebGL /E /XO
