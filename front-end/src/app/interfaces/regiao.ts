import { Cidade } from "./cidade";

export interface Regiao {
  id: string;
  nome: string;
  ativo: boolean;
  cidades: Cidade[];
}