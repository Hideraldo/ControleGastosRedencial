/* Mapa de caminhos de navegação da aplicação. */
export const paths = {
  consulta: "/",
  pessoas: "/pessoas",
  categorias: "/categorias",
  transacoes: "/transacoes",
  pessoaEditar: "/pessoas/:id/editar",
  categoriaEditar: "/categorias/:id/editar",
  transacaoEditar: "/transacoes/:id/editar",
  /** Gera caminho para edição de pessoa pelo id. */
  pessoaEditarById: (id: number) => `/pessoas/${id}/editar`,
  /** Gera caminho para edição de categoria pelo id. */
  categoriaEditarById: (id: number) => `/categorias/${id}/editar`,
  /** Gera caminho para edição de transação pelo id. */
  transacaoEditarById: (id: number) => `/transacoes/${id}/editar`,
};
