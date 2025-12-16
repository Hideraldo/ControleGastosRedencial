/* Barra lateral: navegação auxiliar para páginas do sistema. */
import styles from "./Sidebar.module.css";
import { Link } from "react-router-dom";

/** Componente Sidebar com links de navegação. */
export function Sidebar() {
  return (
    <aside className={styles.sidebar}>
      <ul>
        <li><Link to="/">Consulta Transações</Link></li>
        <li><Link to="/pessoas">Cadastro Pessoa</Link></li>
        <li><Link to="/categorias">Cadastro Categoria</Link></li>
        <li><Link to="/transacoes">Cadastro Transação</Link></li>
      </ul>
    </aside>
  );
}
