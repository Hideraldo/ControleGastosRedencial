/* Modelo de Pessoa: define a estrutura dos dados de pessoa. */
export interface Pessoa {
  id: number;
  nome: string;
  idade: number;
}

/* Payload para criação de Pessoa. */
export interface PessoaCreate {
  nome: string;
  idade: number;
}
