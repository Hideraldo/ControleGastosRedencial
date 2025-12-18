/* Projeto: Controle de Gastos Residencial
   Arquivo: EditarPessoa.tsx
   Objetivo: Página para edição de dados de uma Pessoa. */
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { PessoaService } from "../../services";
import type { Pessoa } from "../../models";
import { Input } from "../../components/common/Input";
import { Button } from "../../components/common/Button";
import { paths } from "../../routes/paths";

/** Componente de página para editar dados de uma pessoa específica. */
export default function EditarPessoa() {
  const { id } = useParams();
  const navigate = useNavigate();
  const pessoaId = parseInt(id ?? "0", 10);

  const [nome, setNome] = useState("");
  const [idade, setIdade] = useState<string>("0");
  const [loading, setLoading] = useState(true);

  /** Carrega dados da pessoa e popula o formulário. */
  useEffect(() => {
    if (!pessoaId) { navigate(paths.pessoas); return; }
    PessoaService.get(pessoaId).then((p: Pessoa) => {
      setNome(p.nome);
      setIdade(String(p.idade));
    }).finally(() => setLoading(false));
  }, [pessoaId, navigate]);

  /** Salva as alterações e retorna para o cadastro de pessoas. */
  const salvar = async () => {
    const idadeNum = parseInt(idade || "0", 10);
    if (!nome.trim()) {
      alert("O nome é obrigatório.");
      return;
    }
    if (nome.length > 150) {
      alert("O nome não pode ter mais de 150 caracteres.");
      return;
    }
    if (isNaN(idadeNum) || idadeNum < 0 || idadeNum > 150) {
      alert("A idade deve estar entre 0 e 150.");
      return;
    }
    try {
      await PessoaService.update(pessoaId, { nome, idade: idadeNum });
      navigate(paths.pessoas);
    } catch (e: unknown) {
      const msg = e instanceof Error ? e.message : "Erro ao salvar pessoa.";
      alert(msg);
    }
  };

  /** Cancela a edição e retorna para o cadastro de pessoas. */
  const cancelar = () => navigate(paths.pessoas);

  return (
    <div className="page-container">
      <h2>Alterar Pessoa</h2>
      {!loading && (
        <div className="page-form">
          <div style={{ width: '337px' }}>
            <Input label="Nome" value={nome} onChange={setNome} />
          </div>
          <div style={{ width: '80px' }}>
            <Input label="Idade" value={idade} onChange={setIdade} type="number" />
          </div>
          <div style={{ display: "flex", gap: 8 }}>
            <Button onClick={salvar}>Salvar</Button>
            <Button variant="secondary" onClick={cancelar}>Cancelar</Button>
          </div>
        </div>
      )}
    </div>
  );
}
