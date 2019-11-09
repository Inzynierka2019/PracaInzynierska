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
    vehicleSpawnFrequency: number;
    vehicleCountMaximum: number;
    vehicleSpawnChances: VehicleSpawnChance[];
    constructor() {
        this.vehicleCountMaximum = 0;
        this.vehicleSpawnFrequency = 0;
        this.vehicleSpawnChances = new Array<VehicleSpawnChance>();
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