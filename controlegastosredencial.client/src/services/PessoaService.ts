/* Serviço de Pessoas: fornece operações de CRUD para entidade Pessoa. */
import { api } from "./api/client";
import { endpoints } from "./api/endpoints";
import type { Pessoa, PessoaCreate } from "../models";

export const PessoaService = {
  /** Lista todas as pessoas cadastradas. */
  list: () => api.get<Pessoa[]>(endpoints.pessoa),
  /** Busca uma pessoa pelo identificador. */
  get: (id: number) => api.get<Pessoa>(`${endpoints.pessoa}/${id}`),
  /** Cria uma nova pessoa. */
  create: (data: PessoaCreate) => api.post<Pessoa>(endpoints.pessoa, data),
  /** Atualiza dados de uma pessoa existente. */
  update: (id: number, data: PessoaCreate) =>
    api.put<void>(`${endpoints.pessoa}/${id}`, data),
  /** Remove uma pessoa pelo identificador. */
  remove: (id: number) => api.delete<void>(`${endpoints.pessoa}/${id}`),
};
