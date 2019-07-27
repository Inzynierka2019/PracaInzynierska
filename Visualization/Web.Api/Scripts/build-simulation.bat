@echo off
cd ..\..\..\Simulation\App
%cd%
set projectPath=%cd%
set unityPath="C:\Program Files\Unity\Hub\Editor\2019.1.11f1\Editor\Unity.exe"

@echo Build is now starting...
call %unityPath% -quit -batchmode -logFile stdout.log -projectPath %projectPath% -executeMethod SimulationBuilder.Build -buildWindows64Player .\out\Simulation.exe
@echo Build has finished!