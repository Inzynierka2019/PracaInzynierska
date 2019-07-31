import { LogMessageType, ConsoleColor, LogMessageTypeToColorMap } from '../interfaces/log-message-type.enum';

export class ConsoleLog {
    message: string;
    
    timeStamp: string;
    
    logType: LogMessageType;

    getConsoleColor(): ConsoleColor {
        return LogMessageTypeToColorMap.get(this.logType);
    };
}

