/* Projeto: Controle de Gastos Residencial
   Arquivo: CadastroCategoria.tsx
   Objetivo: Página para cadastro, listagem e ações de Categoria (criar, editar, excluir). */
import { useEffect, useState } from "react";
import { CategoriaService } from "../../services";
import { useNavigate } from "react-router-dom";
import { paths } from "../../routes/paths";
import type { Categoria, TipoFinalidade } from "../../models";
import { Input } from "../../components/common/Input";
import { Button } from "../../components/common/Button";

/** Componente de página responsável por cadastrar e listar categorias. */
export default function CadastroCategoria() {
  const [items, setItems] = useState<Categoria[]>([]);
  const [nome, setNome] = useState("");
  const [descricao, setDescricao] = useState("");
  const [finalidade, setFinalidade] = useState<TipoFinalidade>(1 as TipoFinalidade);
  const navigate = useNavigate();

  /** Carrega a lista de categorias do serviço e atualiza o estado local. */
  const load = () => CategoriaService.list().then(setItems).catch(console.error);
  useEffect(() => { load(); }, []);

  /** Cria uma nova categoria e recarrega a lista. */
  const create = async () => {
    await CategoriaService.create({ nome, descricao, finalidade });
    setNome(""); setDescricao(""); setFinalidade(1 as TipoFinalidade);
    load();
  };

  /** Navega para a página de edição da categoria selecionada. */
  const editar = (c: Categoria) => {
    navigate(paths.categoriaEditarById(c.id));
  };

  /** Remove a categoria pelo id e recarrega a lista. */
  const remove = async (id: number) => {
    await CategoriaService.remove(id);
    load();
  };

  return (
    <div className="page-container">
      <h2>Cadastro de Categoria</h2>
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
                      onChange={(e) =>
                          setFinalidade(parseInt(e.target.value, 10) as TipoFinalidade)} style={{ width: "100px", height: "32px", marginTop: "4px" }}>
                <option value={1}>Despesa</option>
                <option value={2}>Receita</option>
            </select>
        </label>
        <Button onClick={create}>Salvar</Button>
      </div>
      <table className="page-table">
        <thead>
          <tr>
            <th>Nome</th>
            <th>Descrição</th>
            <th>Finalidade</th>
            <th>Ações</th>
          </tr>
        </thead>
        <tbody>
          {items.map(c => (
            <tr key={c.id}>
              <td>{c.nome}</td>
              <td>{c.descricao ?? ""}</td>
              <td>{c.finalidade === 1 ? "Despesa" : c.finalidade === 2 ? "Receita" : "Ambas"}</td>
              <td style={{ textAlign: 'center' }}>
                 <Button variant="warning" onClick={() => editar(c)}>Atualizar</Button>
                 <Button variant="danger" onClick={() => remove(c.id)}>Excluir</Button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
