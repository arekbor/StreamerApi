import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "../environments/environment";
import { Pager, SteamStat } from "./models";

@Injectable()
export class StreamerService {
  constructor(private httpClient: HttpClient) {}

  public getStats(
    pageNumber: number,
    pageSize: number
  ): Observable<Pager<SteamStat>> {
    let params = new HttpParams();

    params = params.append("pageNumber", pageNumber);
    params = params.append("pageSize", pageSize);

    return this.httpClient.get<Pager<SteamStat>>(
      `${environment.apiUrl}/Streamer/stats`,
      {
        params: params,
      }
    );
  }
}
