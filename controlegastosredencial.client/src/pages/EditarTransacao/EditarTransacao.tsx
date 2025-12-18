/* Projeto: Controle de Gastos Residencial
   Arquivo: EditarTransacao.tsx
   Objetivo: Página para edição de dados de uma Transação. */
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { TransacaoService, PessoaService, CategoriaService } from "../../services";
import type { Transacao, TransacaoCreate, TipoTransacao, Pessoa, Categoria } from "../../models";
import { Input } from "../../components/common/Input";
import { Button } from "../../components/common/Button";
import { paths } from "../../routes/paths";

/** Componente de página para editar dados de uma transação específica. */
export default function EditarTransacao() {
  const { id } = useParams();
  const navigate = useNavigate();
  const transacaoId = parseInt(id ?? "0", 10);

  const [descricao, setDescricao] = useState("");
  const [valor, setValor] = useState<string>("0");
  const [tipo, setTipo] = useState<TipoTransacao>(1 as TipoTransacao);
  const [pessoaId, setPessoaId] = useState<number>(0);
  const [categoriaId, setCategoriaId] = useState<number>(0);
  const [pessoas, setPessoas] = useState<Pessoa[]>([]);
  const [categorias, setCategorias] = useState<Categoria[]>([]);
  const [loading, setLoading] = useState(true);

  /** Carrega dados da transação, pessoas e categorias, e popula o formulário. */
  useEffect(() => {
    if (!transacaoId) { navigate(paths.transacoes); return; }
    PessoaService.list().then(setPessoas);
    CategoriaService.list().then(setCategorias);
    TransacaoService.get(transacaoId).then((t: Transacao) => {
      setDescricao(t.descricao);
      setValor(String(t.valor));
      setTipo(t.tipo);
      setPessoaId(t.pessoaId);
      setCategoriaId(t.categoriaId);
    }).finally(() => setLoading(false));
  }, [transacaoId, navigate]);

  /** Filtra pessoas conforme o tipo da transação (Receita: 18+; Despesa: todas). */
  const filteredPessoas = pessoas.filter(p => {
    if (tipo === 2) return p.idade >= 18;
    return true;
  });

  /** Filtra categorias conforme finalidade e tipo da transação. */
  const filteredCategorias = categorias.filter(c => {
    if (tipo === 1) return c.finalidade === 1 || c.finalidade === 3;
    if (tipo === 2) return c.finalidade === 2 || c.finalidade === 3;
    return true;
  });

  /** Salva as alterações e retorna para o cadastro de transações. */
  const salvar = async () => {
    const valorNum = parseFloat(valor || "0");
    if (!descricao.trim()) {
      alert("A descrição é obrigatória.");
      return;
    }
    if (descricao.length > 200) {
      alert("A descrição não pode ter mais de 200 caracteres.");
      return;
    }
    if (isNaN(valorNum) || valorNum <= 0) {
      alert("O valor deve ser maior que zero.");
      return;
    }
    if (!pessoaId) {
      alert("Selecione uma pessoa.");
      return;
    }
    if (!categoriaId) {
      alert("Selecione uma categoria.");
      return;
    }
    const data: TransacaoCreate = {
      descricao,
      valor: valorNum,
      tipo,
      pessoaId,
      categoriaId,
    };
    try {
      await TransacaoService.update(transacaoId, data);
      navigate(paths.transacoes);
    } catch (e: unknown) {
      const msg = e instanceof Error ? e.message : "Erro ao salvar transação.";
      alert(msg);
    }
  };

  /** Cancela a edição e retorna para o cadastro de transações. */
  const cancelar = () => navigate(paths.transacoes);

  return (
    <div className="page-container">
      <h2>Alterar Transação</h2>
      {!loading && (
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
              onChange={(e) => setTipo(parseInt(e.target.value, 10) as TipoTransacao)}
              style={{ width: "100px", height: "32px", marginTop: "4px" }}
            >
              <option value={1}>Despesa</option>
              <option value={2}>Receita</option>
            </select>
          </label>
          <label style={{ display: "flex", flexDirection: "column", marginBottom: "11px" }}>
            Pessoa
            <select
              value={pessoaId}
              onChange={(e) => setPessoaId(parseInt(e.target.value, 10))}
              style={{ width: "120px", height: "32px", marginTop: "4px" }}
            >
              <option value={0}>Selecione</option>
              {filteredPessoas.map(p => <option key={p.id} value={p.id}>{p.nome}</option>)}
            </select>
          </label>
          <label style={{ display: "flex", flexDirection: "column", marginBottom: "11px" }}>
            Categoria
            <select
              value={categoriaId}
              onChange={(e) => setCategoriaId(parseInt(e.target.value, 10))}
              style={{ width: "150px", height: "32px", marginTop: "4px" }}
            >
              <option value={0}>Selecione</option>
              {filteredCategorias.map(c => <option key={c.id} value={c.id}>{c.nome}</option>)}
            </select>
          </label>
          <div style={{ display: "flex", gap: 8 }}>
            <Button onClick={salvar}>Salvar</Button>
            <Button variant="secondary" onClick={cancelar}>Cancelar</Button>
          </div>
        </div>
      )}
    </div>
  );
}
