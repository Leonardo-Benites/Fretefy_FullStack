export class AtualizarRegiaoRequest {
  id: string;
  nome: string;
  ativo: boolean = true;
  cidades: string[];
}