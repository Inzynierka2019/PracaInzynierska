import { Personality } from './chart-models';

export class SimulationPreferences {
    currentSceneName: string;
    availableScenes: AvailableScene[];
    scenePreferences: ScenePreference;
    constructor() {
        this.currentSceneName = '';
        this.availableScenes = new Array<AvailableScene>();
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

export class AvailableScene {
    scene: string;
    name: string;
}