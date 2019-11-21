import { Personality } from './chart-models';

export class SimulationPreferences {
    currentScene: string;
    availableScenes: string[];
    scenePreferences: ScenePreference;
    constructor() {
        this.currentScene = '';
        this.availableScenes = new Array<string>();
        this.scenePreferences = new ScenePreference();
    }
}

export class ScenePreference {
    trafficLightsPeriod: number;
    vehicleSpawnFrequency: number;
    vehicleCountMaximum: number;
    vehicleSpawnChances: VehicleSpawnChance[];
    driverSpawnChances: DriverSpawnChances[];
    constructor() {
        this.vehicleCountMaximum = 0;
        this.vehicleSpawnFrequency = 0;
        this.trafficLightsPeriod = 0;
        this.vehicleSpawnChances = new Array<VehicleSpawnChance>();
        this.driverSpawnChances = new Array<DriverSpawnChances>();
    }
}

export class DriverSpawnChances {
    personality: string;
    spawnChance: number;
    constructor() {
        this.personality = null;
        this.spawnChance = 0;
    }
}

export class VehicleSpawnChance {
    routeType: string;
    spawnChance: string;
    constructor() {
        this.routeType = '';
        this.spawnChance = '';
    }
}