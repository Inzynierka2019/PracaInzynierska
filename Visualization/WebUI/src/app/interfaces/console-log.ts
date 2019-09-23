import { LogType, ConsoleColor, LogTypeToColorMap } from './utilities';

export class ConsoleLog {
    message: string;
    
    timeStamp: string;
    
    logType: LogType;

    public constructor(message: string, timeStamp: string, logType: LogType) {
        this.message = message;
        this.timeStamp = timeStamp;
        this.logType = logType;
    }

    getConsoleColor(): ConsoleColor {
        return LogTypeToColorMap.get(this.logType);
    };
}

