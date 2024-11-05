import { PhysicalActivity } from "./physical-activity";

export interface ActivityCatalog {
    userId: number;
    activities: Array<PhysicalActivity>;
    lastUserWeight: number;
}