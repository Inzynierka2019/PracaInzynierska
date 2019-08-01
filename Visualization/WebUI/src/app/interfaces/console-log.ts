import { LogType, ConsoleColor, LogTypeToColorMap } from '../interfaces/log-message-type.enum';

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
        var test = LogTypeToColorMap.get(this.logType);
        return LogTypeToColorMap.get(this.logType);
    };
}

