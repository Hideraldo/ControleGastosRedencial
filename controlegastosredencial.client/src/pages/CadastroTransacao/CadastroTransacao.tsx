/* Projeto: Controle de Gastos Residencial
   Arquivo: CadastroTransacao.tsx
   Objetivo: Página para cadastro, listagem e ações de Transação (criar, editar, excluir). */
import { useEffect, useState } from "react";
import { TransacaoService, PessoaService, CategoriaService } from "../../services";
import { useNavigate } from "react-router-dom";
import { paths } from "../../routes/paths";
import type { Transacao, TransacaoCreate, TipoTransacao, Pessoa, Categoria } from "../../models";
import { Input } from "../../components/common/Input";
import { Button } from "../../components/common/Button";

/** Componente de página responsável por cadastrar e listar transações. */
export default function CadastroTransacao() {
  const [items, setItems] = useState<Transacao[]>([]);
  const [descricao, setDescricao] = useState("");
  const [valor, setValor] = useState<string>("0");
  const [tipo, setTipo] = useState<TipoTransacao>(1 as TipoTransacao);
  const [pessoaId, setPessoaId] = useState<number>(0);
  const [categoriaId, setCategoriaId] = useState<number>(0);
  const [pessoas, setPessoas] = useState<Pessoa[]>([]);
  const [categorias, setCategorias] = useState<Categoria[]>([]);
  const navigate = useNavigate();

  /** Filtra pessoas conforme o tipo da transação (Receita: 18+; Despesa: todas). */
  const filteredPessoas = pessoas.filter(p => {
    if (tipo === 2) return p.idade >= 18; // Receita: apenas maiores de 18
    return true; // Despesa: todas
  });

  /** Filtra categorias conforme finalidade e tipo da transação. */
  const filteredCategorias = categorias.filter(c => {
    // TipoTransacao.Despesa (1) -> Categoria.Despesa (1) ou Ambas (3)
    if (tipo === 1) return c.finalidade === 1 || c.finalidade === 3;
    // TipoTransacao.Receita (2) -> Categoria.Receita (2) ou Ambas (3)
    if (tipo === 2) return c.finalidade === 2 || c.finalidade === 3;
    return true;
  });

  /** Carrega a lista de transações e dados auxiliares. */
  const load = () => TransacaoService.list().then(setItems).catch(console.error);
  useEffect(() => { 
    load();
    PessoaService.list().then(setPessoas);
    CategoriaService.list().then(setCategorias);
  }, []);

  /** Cria nova transação com os dados preenchidos e recarrega a lista. */
  const create = async () => {
    const data: TransacaoCreate = {
      descricao,
      valor: parseFloat(valor || "0"),
      tipo,
      pessoaId,
      categoriaId,
    };
    await TransacaoService.create(data);
    setDescricao(""); setValor("0"); setTipo(1 as TipoTransacao); setPessoaId(0); setCategoriaId(0);
    load();
  };

  /** Navega para a página de edição da transação selecionada. */
  const update = async (t: Transacao) => {
    navigate(paths.transacaoEditarById(t.id));
  };

  /** Remove a transação pelo id e recarrega a lista. */
  const remove = async (id: number) => {
    await TransacaoService.remove(id);
    load();
  };

  return (
    <div className="page-container">
      <h2>Cadastro de Transação</h2>
      <div className="page-form">
        <div style={{ width: '356px' }}>
            <Input label="Descrição" value={descricao} onChange={setDescricao} />
        </div>
        <div style={{ width: '80px' }}>
            <Input label="Valor" value={valor} onChange={setValor} type="number" />
        </div>
        <label style={{ display: "flex", flexDirection: "column", marginBottom: "11px" }}>
            Tipo
            <select
                value={tipo}
                onChange={(e) =>
                    setTipo(parseInt(e.target.value, 10) as TipoTransacao)} style={{ width: "100px", height: "32px", marginTop: "4px" }}>
                <option value={1}>Despesa</option>
                <option value={2}>Receita</option>
            </select>
        </label>
        <label style={{ display: "flex", flexDirection: "column", marginBottom: "11px" }}>
            Pessoa
            <select
                value={pessoaId}
                onChange={(e) =>
                    setPessoaId(parseInt(e.target.value, 10))} style={{ width: "120px", height: "32px", marginTop: "4px" }}>
                <option value={0}>Selecione</option>
                {filteredPessoas.map(p => <option key={p.id} value={p.id}>{p.nome}</option>)}
            </select>
        </label>
        <label style={{ display: "flex", flexDirection: "column", marginBottom: "11px" }}>
            Categoria
            <select
                value={categoriaId}
                onChange={(e) =>
                    setCategoriaId(parseInt(e.target.value, 10))} style={{ width: "150px", height: "32px", marginTop: "4px" }}>
                        <option value={0}>Selecione</option>
                        {filteredCategorias.map(c => <option key={c.id} value={c.id}>{c.nome}</option>)}
            </select>
        </label>
        <Button onClick={create}>Salvar</Button>
      </div>
      <table className="page-table">
        <thead>
          <tr>
            <th>Descrição</th>
            <th>Valor</th>
            <th>Tipo</th>
            <th>Categoria</th>
            <th>Pessoa</th>
            <th>Ações</th>
          </tr>
        </thead>
        <tbody>
          {items.map(t => (
            <tr key={t.id}>
              <td>{t.descricao}</td>
              <td className="page-num">{t.valor.toFixed(2)}</td>
              <td>{t.tipo === 1 ? "Despesa" : "Receita"}</td>
              <td>{t.categoria?.nome ?? categorias.find(c => c.id === t.categoriaId)?.nome ?? t.categoriaId}</td>
              <td>{t.pessoa?.nome ?? pessoas.find(p => p.id === t.pessoaId)?.nome ?? t.pessoaId}</td>
              <td style={{ textAlign: 'center' }}>
                  <Button variant="warning" onClick={() => update(t)}>Atualizar</Button>
                  <Button variant="danger" onClick={() => remove(t.id)}>Excluir</Button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
