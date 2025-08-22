import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Regiao } from "../interfaces/regiao";
import { environment } from "src/environments/environment";
import { CriarRegiaoRequest } from "../interfaces/CriarRegiaoRequest";
import { AtualizarRegiaoRequest } from "../interfaces/AtualizarRegiaoRequest";

@Injectable({
  providedIn: 'root'
})
export class RegiaoService {
  constructor(private httpClient: HttpClient) { }

  listarRegioes() {
    return this.httpClient.get<Regiao[]>(environment.apiUrl + '/Regiao');
  }

  alterarStatus(id: string, ativo: { id: string, ativo: boolean }) {
    return this.httpClient.patch(`${environment.apiUrl}/Regiao/${id}/status`, ativo);
  }

  criarRegiao(regiao: CriarRegiaoRequest) {
    return this.httpClient.post<Regiao>(environment.apiUrl + '/Regiao', regiao);
  }

  atualizarRegiao(regiao: AtualizarRegiaoRequest) {
    return this.httpClient.put<Regiao>(`${environment.apiUrl}/Regiao/${regiao.id}`, regiao);
  }

  excluirRegiao(id: string) {
    return this.httpClient.delete(`${environment.apiUrl}/Regiao/${id}`);
  }

  buscarRegiaoPorId(id: string) {
    return this.httpClient.get<Regiao>(`${environment.apiUrl}/Regiao/${id}`);
  }

  exportarExcel() {
    return this.httpClient.get(`${environment.apiUrl}/Regiao/exportar`, {
      responseType: 'blob'
    });
  }
  
}