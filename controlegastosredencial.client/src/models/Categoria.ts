/* Modelo de Categoria: define finalidade e estrutura dos dados de categoria. */
export type TipoFinalidade = 1 | 2 | 3;
export const TipoFinalidadeValues = {
  Despesa: 1 as TipoFinalidade,
  Receita: 2 as TipoFinalidade,
  Ambas: 3 as TipoFinalidade,
};

export interface Categoria {
  id: number;
  nome: string;
  descricao?: string | null;
  finalidade: TipoFinalidade;
}

/* Payload para criação de Categoria. */
export interface CategoriaCreate {
  nome: string;
  descricao?: string | null;
  finalidade: TipoFinalidade;
}
