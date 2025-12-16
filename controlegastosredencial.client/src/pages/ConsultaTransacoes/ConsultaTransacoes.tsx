/* Projeto: Controle de Gastos Residencial
   Arquivo: ConsultaTransacoes.tsx
   Objetivo: Página para consulta de transações e exibição de totais por pessoa e categoria. */
import { useEffect, useState } from "react";
import { TransacaoService } from "../../services";
import type { Transacao, TotalPorPessoa, TotalPorCategoria, TotalGeral } from "../../models";
 

/** Componente de página para consulta e agregação de transações. */
export default function ConsultaTransacoes() {
  const [transacoes, setTransacoes] = useState<Transacao[]>([]);
  const [pessoas, setPessoas] = useState<TotalPorPessoa[]>([]);
  const [categorias, setCategorias] = useState<TotalPorCategoria[]>([]);
  const [totalGeral, setTotalGeral] = useState<TotalGeral | null>(null);
 

  /** Busca transações e totais agregados (por pessoa e categoria). */
  useEffect(() => {
    TransacaoService.list().then(setTransacoes).catch(console.error);
    TransacaoService.totaisPorPessoa().then((r) => {
      setPessoas(r.pessoas);
      setTotalGeral(r.totalGeral);
    }).catch(console.error);
    TransacaoService.totaisPorCategoria().then((r) => {
      setCategorias(r.categorias);
    }).catch(console.error);
  }, []);

 

  const pessoaNomeById = Object.fromEntries(pessoas.map(p => [p.pessoaId, p.pessoaNome]));
  const categoriaNomeById = Object.fromEntries(categorias.map(c => [c.categoriaId, c.categoriaNome]));

  return (
    <div className="page-container">
      <h2>Consulta de Transações</h2>
      <table className="page-table">
        <thead>
          <tr>
            <th>Descrição</th>
            <th>Valor</th>
            <th>Tipo</th>
            <th>Categoria</th>
            <th>Pessoa</th>
          </tr>
        </thead>
        <tbody>
          {transacoes.map(t => (
            <tr key={t.id}>
              <td>{t.descricao}</td>
              <td className="page-num">{t.valor.toFixed(2)}</td>
              <td>{t.tipo === 1 ? "Despesa" : "Receita"}</td>
              <td>{t.categoria?.nome ?? categoriaNomeById[t.categoriaId] ?? t.categoriaId}</td>
              <td>{t.pessoa?.nome ?? pessoaNomeById[t.pessoaId] ?? t.pessoaId}</td>
            </tr>
          ))}
        </tbody>
      </table>

      <h3>Totais por Pessoa</h3>
      <table className="page-table">
        <thead>
          <tr>
            <th>Pessoa</th>
            <th>Receitas</th>
            <th>Despesas</th>
            <th>Saldo</th>
          </tr>
        </thead>
        <tbody>
          {pessoas.map(p => (
            <tr key={p.pessoaId}>
              <td>{p.pessoaNome}</td>
              <td className="page-num">{p.totalReceitas.toFixed(2)}</td>
              <td className="page-num">{p.totalDespesas.toFixed(2)}</td>
              <td className="page-num">{p.saldo.toFixed(2)}</td>
            </tr>
          ))}
          {totalGeral && (
            <tr className="page-total-row">
              <td>Total Geral</td>
              <td className="page-num">{totalGeral.totalReceitas.toFixed(2)}</td>
              <td className="page-num">{totalGeral.totalDespesas.toFixed(2)}</td>
              <td className="page-num">{totalGeral.saldoLiquido.toFixed(2)}</td>
            </tr>
          )}
        </tbody>
      </table>

      <h3>Totais por Categoria</h3>
      <table className="page-table">
        <thead>
          <tr>
            <th>Categoria</th>
            <th>Receitas</th>
            <th>Despesas</th>
            <th>Saldo</th>
          </tr>
        </thead>
        <tbody>
          {categorias.map(c => (
            <tr key={c.categoriaId}>
              <td>{c.categoriaNome}</td>
              <td className="page-num">{c.totalReceitas.toFixed(2)}</td>
              <td className="page-num">{c.totalDespesas.toFixed(2)}</td>
              <td className="page-num">{c.saldo.toFixed(2)}</td>
            </tr>
          ))}
          {totalGeral && (
              <tr className="page-total-row">
                  <td>Total Geral</td>
                  <td className="page-num">{totalGeral.totalReceitas.toFixed(2)}</td>
                  <td className="page-num">{totalGeral.totalDespesas.toFixed(2)}</td>
                  <td className="page-num">{totalGeral.saldoLiquido.toFixed(2)}</td>
              </tr>
          )}
        </tbody>
      </table>
    </div>
  );
}
