# [Praca dyplomowa inżynierska](https://github.com/Inzynierka2019/PracaInzynierska/tree/master/)
Symulator ruchu miejskiego z wizualizacją w aplikacji
internetowej.

Constructive simulation of city traffic with visualization on web
application.

## Dokumentacja
Dokumentacja pracy dyplomowej znajduje się w folderze [Documentation](https://github.com/Inzynierka2019/PracaInzynierska/tree/master/Documentation)

## Lista projektów:
+ Simulation
    + App
+ Visualization
    + Web.Api
      + WebUI
    + Web.Logic
+ Common
    + Common.Models
    + Common.Communication (Hub broker for communication)
    + Common.ConsoleTester

## Warunki wstępne
Do poprawnego działania aplikacji wymagane są:
- Unity [2019.1.1f1](https://unity3d.com/get-unity/download/archive)
- .Net Core SDK [2.2](https://dotnet.microsoft.com/download/dotnet-core/2.2)
- Node.js [v10.16.0](https://nodejs.org/en/download/)
- Node Package Manager 6.9.0 (razem z Node.js)
- @angular/cli (Patrz poniżej)

### Instrukcja Visualization

#### WebUI (Angular 8)
Instalacja @angular/cli
```
$> npm install -g @angular/cli
```

Aktualizacja cli/core do najnowszej wersji
```
$> ng update @angular/cli @angular/core
```

Aktualizacja pakietów npm (w folderze Visualization/WebUI)
```
$> npm install
```

Uruchomienie WebUI (domyślnie http://localhost:4200/)
```
$> ng serve -o
```

#### Web.Api (.Net Core)
Budowanie projektu
```
$> dotnet build
```

Uruchomienie projektu
```
$> dotnet run
```

### Autorzy:
  - Kamil Dakus
  - Jacek Ardanowski
  - Jan Czubiak

> 2019