@echo off

set unityPath="C:\Program Files\Unity\Hub\Editor\2017.4.30f1\Editor\Unity.exe"

@echo %cd%

@echo Build is now starting...

call %unityPath% -quit -batchmode -logFile stdout.log -projectPath %cd% -executeMethod SimulationBuilder.Build

@echo Build has finished!