import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { GetCidadeRequest } from "../interfaces/GetCidadeRequest";
import { Observable } from "rxjs";
import { Cidade } from "../interfaces/cidade";

@Injectable({
  providedIn: 'root'
})

export class CidadeService {
  constructor(private httpClient: HttpClient) { }

  listarCidades(model?: GetCidadeRequest) : Observable<Cidade[]> {
    const params = new URLSearchParams();
    if (model?.uf) {
      params.append('uf', model.uf);
    }
    if (model?.terms) {
      params.append('terms', model.terms);
    }
    return this.httpClient.get<Cidade[]>(`${environment.apiUrl}/cidade?${params.toString()}`);
  }

  buscarCidadePorId(id: number): Observable<Cidade> {
    return this.httpClient.get<Cidade>(`${environment.apiUrl}/cidade/${id}`);
  }

}