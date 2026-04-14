

export interface Errorlog{
    id: number;
    batch_id: string;
    app_code: string;
    entity_id: string;
    message: string;
    message_type: string;
    source_system: string;
    entity_type: string;
    current_status: string;
    start_time: Date;
    completion_time: Date;
    current_retry_count: string;
    Dur: Date;
    batch_code:string;
    ym: String;
}
