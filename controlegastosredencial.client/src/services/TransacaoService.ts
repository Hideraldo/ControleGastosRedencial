/* Serviço de Transações: operações de CRUD e consultas agregadas para transações. */
import { api } from "./api/client";
import { endpoints } from "./api/endpoints";
import type {
  Transacao,
  TransacaoCreate,
  TotalPorPessoa,
  TotalPorCategoria,
  TotalGeral,
} from "../models";

export const TransacaoService = {
  /** Lista todas as transações registradas. */
  list: () => api.get<Transacao[]>(endpoints.transacao),
  /** Busca uma transação pelo identificador. */
  get: (id: number) => api.get<Transacao>(`${endpoints.transacao}/${id}`),
  /** Cria uma nova transação. */
  create: (data: TransacaoCreate) => api.post<Transacao>(endpoints.transacao, data),
  /** Atualiza uma transação existente. */
  update: (id: number, data: TransacaoCreate) =>
    api.put<void>(`${endpoints.transacao}/${id}`, data),
  /** Remove uma transação pelo identificador. */
  remove: (id: number) => api.delete<void>(`${endpoints.transacao}/${id}`),
  /** Obtém totais agregados por pessoa e total geral. */
  totaisPorPessoa: () => api.get<{ pessoas: TotalPorPessoa[]; totalGeral: TotalGeral }>(endpoints.transacaoTotaisPessoas),
  /** Obtém totais agregados por categoria e total geral. */
  totaisPorCategoria: () => api.get<{ categorias: TotalPorCategoria[]; totalGeral: TotalGeral }>(endpoints.transacaoTotaisCategorias),
  /** Obtém o total geral agregado. */
  totalGeral: () => api.get<TotalGeral>(endpoints.transacaoTotalGeral),
};
