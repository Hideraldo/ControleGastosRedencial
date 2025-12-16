/* Cabeçalho da aplicação: exibe logo e navegação principal. */
import styles from "./Header.module.css";
import { Link } from "react-router-dom";

/** Componente Header com links para páginas principais. */
export function Header() {
  return (
    <header className={styles.header}>
      <div className={styles.logo}>Controle de Gastos</div>
      <nav className={styles.nav}>
        <Link to="/">Consulta</Link>
        <Link to="/pessoas">Pessoas</Link>
        <Link to="/categorias">Categorias</Link>
        <Link to="/transacoes">Transações</Link>
      </nav>
    </header>
  );
}
