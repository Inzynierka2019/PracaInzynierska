export interface VehiclePopulation {
    timestamp: Date;
    vehicleCount: number;
    vehicleStatuses: Array<VehicleStatus>;
}

export class VehiclePopulationData {
    data: number[];
    labels: string[];
    constructor() {
        this.data = new Array<number>();
        this.labels = new Array<string>();
    }
}

export interface VehicleStatus {
    id: number;
    latitude: number;
    longitude: number;
    currentSpeed: number;
    personality: Personality;
}

export interface DriverReport {
    avgSpeed: number;
    travelTime: Date;
    driver: Driver;
    routeTarget: string;
}

export interface Driver {
    personality: Personality,
    reactionTime: number;
    age: number;
}

export enum Personality {
    slow,
    normal,
    aggresive
}

export class PersonalityStats {
    slow: number;
    normal: number;
    aggresive: number;
    count: number;

    constructor() {
        this.slow = 0;
        this.normal = 0;
        this.aggresive = 0;
        this.count = 0;
    }
}

export class AvgSpeedStatsData {
    slow: number[];
    normal: number[];
    aggresive: number[];
    labels: string[];
    
    constructor() {
        this.slow = new Array<number>();
        this.normal = new Array<number>();
        this.aggresive = new Array<number>();
        this.labels = new Array<string>();
    }
}