export interface Room {
    name: string;
    sensorIp: string;
    heaterIp: string;
    minTemp: number;
    maxtemp: number;
    interval: number;
    autoHeatEnabled: boolean;
}
