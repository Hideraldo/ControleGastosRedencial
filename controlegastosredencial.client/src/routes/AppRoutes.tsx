/* Rotas da aplicação: configura navegação entre páginas e fallback. */
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import { Header } from "../components/layout/Header";
import { Sidebar } from "../components/layout/Sidebar";
import { ConsultaTransacoes } from "../pages/ConsultaTransacoes";
import { CadastroPessoa } from "../pages/CadastroPessoa";
import { CadastroCategoria } from "../pages/CadastroCategoria";
import { CadastroTransacao } from "../pages/CadastroTransacao";
import { EditarPessoa } from "../pages/EditarPessoa";
import { EditarCategoria } from "../pages/EditarCategoria";
import { EditarTransacao } from "../pages/EditarTransacao";
import { paths } from "./paths";
import "./AppRoutes.css";

/** Componente raiz de rotas; define o layout e as rotas públicas. */
export default function AppRoutes() {
  return (
    <BrowserRouter>
      <Header />
      <div className="app-layout">
        <Sidebar />
        <main className="app-content">
          <Routes>
            <Route path={paths.consulta} element={<ConsultaTransacoes />} />
            <Route path={paths.pessoas} element={<CadastroPessoa />} />
            <Route path={paths.pessoaEditar} element={<EditarPessoa />} />
            <Route path={paths.categorias} element={<CadastroCategoria />} />
            <Route path={paths.categoriaEditar} element={<EditarCategoria />} />
            <Route path={paths.transacoes} element={<CadastroTransacao />} />
            <Route path={paths.transacaoEditar} element={<EditarTransacao />} />
            <Route path="*" element={<Navigate to={paths.consulta} replace />} />
          </Routes>
        </main>
      </div>
    </BrowserRouter>
  );
}
