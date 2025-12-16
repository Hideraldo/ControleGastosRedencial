/* Projeto: Controle de Gastos Residencial
   Arquivo: CadastroPessoa.tsx
   Objetivo: Página para cadastro, listagem e ações de Pessoa (criar, editar, excluir). */
import { useEffect, useState } from "react";
import { PessoaService } from "../../services";
import { useNavigate } from "react-router-dom";
import { paths } from "../../routes/paths";
import type { Pessoa } from "../../models";
import { Input } from "../../components/common/Input";
import { Button } from "../../components/common/Button";

/** Componente de página responsável por cadastrar e listar pessoas. */
export default function CadastroPessoa() {
  const [items, setItems] = useState<Pessoa[]>([]);
  const [nome, setNome] = useState("");
  const [idade, setIdade] = useState<string>("0");
  const navigate = useNavigate();

  /** Carrega a lista de pessoas do serviço e atualiza o estado local. */
  const load = () => PessoaService.list().then(setItems).catch(console.error);
  useEffect(() => { load(); }, []);

  /** Cria uma nova pessoa com nome e idade e recarrega a lista. */
  const create = async () => {
    await PessoaService.create({ nome, idade: parseInt(idade || "0", 10) });
    setNome("");
    setIdade("0");
    load();
  };

  /** Navega para a página de edição da pessoa selecionada. */
  const editar = (p: Pessoa) => {
    navigate(paths.pessoaEditarById(p.id));
  };

  /** Remove a pessoa pelo id e recarrega a lista. */
  const remove = async (id: number) => {
    await PessoaService.remove(id);
    load();
  };

  return (
    <div className="page-container">
      <h2>Cadastro de Pessoa</h2>
      <div className="page-form">
        <div style={{ width: '350px' }}>
            <Input label="Nome" value={nome} onChange={setNome} />
        </div>
              <div style={{ width: '100px' }}>
            <Input label="Idade" value={idade} onChange={setIdade} type="number" />
        </div>
        <Button onClick={create}>Salvar</Button>
      </div>
      <table className="page-table">
        <thead>
          <tr>
            <th>Nome</th>
            <th>Idade</th>
            <th>Ações</th>
          </tr>
        </thead>
        <tbody>
          {items.map(p => (
              <tr key={p.id}>
              <td width = "60%">{p.nome}</td>
              <td className="page-num">{p.idade}</td>
              <td style={{ textAlign: 'center' }}>
                 <Button variant="warning" onClick={() => editar(p)}>Atualizar</Button>
                <Button variant="danger" onClick={() => remove(p.id)}>Excluir</Button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
