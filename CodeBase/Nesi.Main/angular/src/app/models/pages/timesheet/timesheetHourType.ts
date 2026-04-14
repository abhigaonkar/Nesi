export class TimesheetHourTypeRecord {
    hourTypeId: number;
    hourType: string;
    hours: number;
}


export class TimesheetTransactionRecord {
    hourTypeId: number
    hourType: string;
    hours: number;
    error: string;
}

export class BatchInsertionResult {
    okay: number;
    result: string;
    list: Array<TimesheetTransactionRecord>
}

export class ParseResult {
    continue: boolean;
    message: string;
}