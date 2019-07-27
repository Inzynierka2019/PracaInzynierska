# Praca Inżynierska
Symulator ruchu miejskiego z wizualizacją w aplikacji
internetowej.

Constructive simulation of city traffic with visualization on web
application.

## Dokumentacja
Dokumentacja pracy dyplomowej znajduje się w folderze [Documentation](https://github.com/Inzynierka2019/PracaInzynierska/tree/dev/Documentation)

## Lista projektów:
+ Simulation
    + Libs
      + Core
      + Core.Tests
    + Unity
+ Visualization
    + Web.Api
    + Web.Logic
    + Web.RestModels
    + Web.Utils
    + Tests
      + Web.Api.Tests
      + Web.Logic.Tests
+ Common
    + Common.Models
    + Common.Utils
    + HubClient (Broker)
    + HubClient.Tests

## Warunki wstępne
Do poprawnego działania aplikacji wymagane są:
- Unity [2019.1.1f1](https://unity3d.com/get-unity/download/archive)
- .Net Core SDK [2.2](https://dotnet.microsoft.com/download/dotnet-core/2.2)
- Node.js [v10.16.0](https://nodejs.org/en/download/)
- Node Package Manager 6.9.0 (razem z Node.js)
- @angular/cli (Patrz poniżej)
- [MapBox Unity SDK](https://www.mapbox.com/install/unity/) v2.0.0 (?)
- Docker for Windows

### Instrukcja Simulation
Zainstaluj Unity w wersji 2019.1.1f1
Uruchom i zbuduj projekt Core w solution Simulation/Libs/Simulation.sln
Uruchom edytor unity.
Załaduj domyślną scenę Scenes/Scene.unity
Wciśnij Play.
Jeśli w konsoli wyświetli się napis "It works !!!", to wszystko jest skonfigurowane poprawnie.

Instalacja Mapbox SDK for Unity:
https://www.mapbox.com/install/unity/
> Assets -> Import Package -> Custom Package -> wybierz plik z sdk


### Instrukcja Visualization

#### WebUI (Angular 8)
Aktualizacja cli/core do najnowszej wersji
> $> ng update @angular/cli @angular/core

Instalacja @angular/cli
> $> npm -g @angular/cli

Aktualizacja pakietów npm (w folderze Visualization/WebUI)
> $> npm install

Uruchomienie WebUI (domyślnie http://localhost:4200/)
> $> ng serve -o

#### Web.Api (.netcore)
Budowanie projektu
> dotnet build

Uruchomienie projektu
> dotnet run


### Docker-compose
W głównym folderze komenda:
> docker-compose up
stawia kontener linuxowy z aplikacją webową oraz kopiuje artefakty do folderu out.


### Autorzy:
  - Kamil Dakus
  - Jacek Ardanowski
  - Jan Czubiak

> 2019