export interface Room {
    name: string;
    sensorsIp: string;
    heaterIp: string;
    minTemp: number;
    maxtemp: number;
    interval: number;
    autoHeatEnabled: boolean;
}
