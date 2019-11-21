export enum LogType {
    Info = 0,
    Warning = 1,
    Error = 2,
    Debug = 3,
    Fatal = 4,
    Success = 5
}

export enum ConsoleColor {
    White,
    Black,
    Gray,
    Orange,
    Green,
    Yellow,
    Blue,
    Magenta,
    Cyan,
    Red,
    DarkGreen,
    DarkYellow,
    DarkBlue,
    DarkMagenta,
    DarkCyan,
    DarkRed
}

export const LogTypeToColorMap: Map<number, ConsoleColor> = new Map<number, ConsoleColor>([
   [LogType.Info, ConsoleColor.White],
   [LogType.Warning, ConsoleColor.Yellow],
   [LogType.Error, ConsoleColor.Red],
   [LogType.Debug, ConsoleColor.Cyan],
   [LogType.Fatal, ConsoleColor.DarkRed],
   [LogType.Success, ConsoleColor.Green]
]);

export enum HubMethod {
    VehiclePopulation,
    DriverReport,
    PersonalityStats,
    AvgSpeedByPersonality
}

export const StatisticsMethods: Map<HubMethod, string> = new Map<HubMethod, string>([
    [HubMethod.VehiclePopulation, "SignalForVehiclePopulation"],
    [HubMethod.DriverReport, "SignalForDriverReports"],
    [HubMethod.PersonalityStats, "SignalForPersonalityStats"],
    [HubMethod.AvgSpeedByPersonality, "SignalForAvgSpeedByPersonality"]
]);