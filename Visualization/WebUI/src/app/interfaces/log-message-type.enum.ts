export enum LogMessageType {
    Info = 0,
    Warning = 1,
    Error = 2,
    Debug = 3,
    Fatal = 4
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

export const LogMessageTypeToColorMap: Map<number, ConsoleColor> = new Map<number, ConsoleColor>([
   [LogMessageType.Info, ConsoleColor.White],
   [LogMessageType.Warning, ConsoleColor.Yellow],
   [LogMessageType.Error, ConsoleColor.Red],
   [LogMessageType.Debug, ConsoleColor.Cyan],
   [LogMessageType.Fatal, ConsoleColor.DarkRed]
]);