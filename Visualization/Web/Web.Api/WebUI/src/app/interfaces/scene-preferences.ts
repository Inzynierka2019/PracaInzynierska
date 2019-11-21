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

export class AvailableScene {
    scene: string;
    name: string;
}