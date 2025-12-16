import { useEffect, useState } from "react";
import { TransacaoService } from "../services";
import type { Transacao } from "../models";

export function useTransacao() {
  const [transacoes, setTransacoes] = useState<Transacao[]>([]);
  useEffect(() => { TransacaoService.list().then(setTransacoes); }, []);
  return transacoes;
}
