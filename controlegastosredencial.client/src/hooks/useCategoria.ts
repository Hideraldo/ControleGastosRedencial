import { useEffect, useState } from "react";
import { CategoriaService } from "../services";
import type { Categoria } from "../models";

export function useCategoria() {
  const [categorias, setCategorias] = useState<Categoria[]>([]);
  useEffect(() => { CategoriaService.list().then(setCategorias); }, []);
  return categorias;
}
