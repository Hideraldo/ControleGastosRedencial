/* Serviço de Categorias: fornece operações de CRUD para entidade Categoria. */
import { api } from "./api/client";
import { endpoints } from "./api/endpoints";
import type { Categoria, CategoriaCreate } from "../models";

export const CategoriaService = {
  /** Lista todas as categorias cadastradas. */
  list: () => api.get<Categoria[]>(endpoints.categoria),
  /** Busca uma categoria pelo identificador. */
  get: (id: number) => api.get<Categoria>(`${endpoints.categoria}/${id}`),
  /** Cria uma nova categoria. */
  create: (data: CategoriaCreate) => api.post<Categoria>(endpoints.categoria, data),
  /** Atualiza dados de uma categoria existente. */
  update: (id: number, data: CategoriaCreate) =>
    api.put<void>(`${endpoints.categoria}/${id}`, data),
  /** Remove uma categoria pelo identificador. */
  remove: (id: number) => api.delete<void>(`${endpoints.categoria}/${id}`),
};
