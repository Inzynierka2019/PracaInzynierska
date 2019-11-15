import { Personality } from './chart-models';

export interface SummaryReport {
    personality: Personality;
    driverType: string;
    avgReactionTime: number;
    avgAge: number;
    avgSpeed: number;
    avgTravelTime: Date;
    mostPopularRouteTarget: string;
    count: number;
}
