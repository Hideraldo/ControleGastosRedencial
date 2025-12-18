/* Projeto: Controle de Gastos Residencial
   Arquivo: EditarCategoria.tsx
   Objetivo: Página para edição de dados de uma Categoria. */
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { CategoriaService } from "../../services";
import type { Categoria, TipoFinalidade } from "../../models";
import { Input } from "../../components/common/Input";
import { Button } from "../../components/common/Button";
import { paths } from "../../routes/paths";

/** Componente de página para editar dados de uma categoria específica. */
export default function EditarCategoria() {
  const { id } = useParams();
  const navigate = useNavigate();
  const categoriaId = parseInt(id ?? "0", 10);

  const [nome, setNome] = useState("");
  const [descricao, setDescricao] = useState("");
  const [finalidade, setFinalidade] = useState<TipoFinalidade>(1 as TipoFinalidade);
  const [loading, setLoading] = useState(true);

  /** Carrega dados da categoria e popula o formulário. */
  useEffect(() => {
    if (!categoriaId) { navigate(paths.categorias); return; }
    CategoriaService.get(categoriaId).then((c: Categoria) => {
      setNome(c.nome);
      setDescricao(c.descricao ?? "");
      setFinalidade(c.finalidade);
    }).finally(() => setLoading(false));
  }, [categoriaId, navigate]);

  /** Salva as alterações e retorna para o cadastro de categorias. */
  const salvar = async () => {
    if (!nome.trim()) {
      alert("O nome é obrigatório.");
      return;
    }
    if (nome.length > 100) {
      alert("O nome não pode ter mais de 100 caracteres.");
      return;
    }
    try {
      await CategoriaService.update(categoriaId, { nome, descricao, finalidade });
      navigate(paths.categorias);
    } catch (e: unknown) {
      const msg = e instanceof Error ? e.message : "Erro ao salvar categoria.";
      alert(msg);
    }
  };

  /** Cancela a edição e retorna para o cadastro de categorias. */
  const cancelar = () => navigate(paths.categorias);

  return (
    <div className="page-container">
      <h2>Alterar Categoria</h2>
      {!loading && (
        <div className="page-form">
          <div style={{ width: '337px' }}>
            <Input label="Nome" value={nome} onChange={setNome} />
          </div>
          <div style={{ width: '350px' }}>
            <Input label="Descrição" value={descricao} onChange={setDescricao} />
          </div>
          <label style={{ display: "flex", flexDirection: "column", marginBottom: "11px" }}>
            Finalidade
            <select
              value={finalidade}
              onChange={(e) => setFinalidade(parseInt(e.target.value, 10) as TipoFinalidade)}
              style={{ width: "100px", height: "32px", marginTop: "4px" }}
            >
              <option value={1}>Despesa</option>
              <option value={2}>Receita</option>
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
