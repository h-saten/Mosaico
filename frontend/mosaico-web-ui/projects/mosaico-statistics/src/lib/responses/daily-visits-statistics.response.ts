import {VisitsDto} from "../models";

export interface DailyVisitsStatisticsResponse {
  tokenPageVisits: VisitsDto[];
  fundPageVisits: VisitsDto[];
}
