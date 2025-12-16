import { useEffect, useState } from "react";
import { PessoaService } from "../services";
import type { Pessoa } from "../models";

export function usePessoa() {
  const [pessoas, setPessoas] = useState<Pessoa[]>([]);
  useEffect(() => { PessoaService.list().then(setPessoas); }, []);
  return pessoas;
}
