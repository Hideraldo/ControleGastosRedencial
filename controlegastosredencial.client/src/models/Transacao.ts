/* Modelo de Transação: define estrutura e tipos relacionados a transações. */
export type TipoTransacao = 1 | 2;
export const TipoTransacaoValues = {
  Despesa: 1 as TipoTransacao,
  Receita: 2 as TipoTransacao,
};

export interface Transacao {
  id: number;
  descricao: string;
  valor: number;
  tipo: TipoTransacao;
  categoriaId: number;
  pessoaId: number;
  categoria?: { id: number; nome: string } | null;
  pessoa?: { id: number; nome: string; idade: number } | null;
}

export interface TransacaoCreate {
  descricao: string;
  valor: number;
  tipo: TipoTransacao;
  categoriaId: number;
  pessoaId: number;
}

export interface TotalPorPessoa {
  pessoaId: number;
  pessoaNome: string;
  pessoaIdade: number;
  totalReceitas: number;
  totalDespesas: number;
  saldo: number;
}

export interface TotalPorCategoria {
  categoriaId: number;
  categoriaNome: string;
  finalidade: number;
  totalReceitas: number;
  totalDespesas: number;
  saldo: number;
}

export interface TotalGeral {
  totalReceitas: number;
  totalDespesas: number;
  saldoLiquido: number;
}
