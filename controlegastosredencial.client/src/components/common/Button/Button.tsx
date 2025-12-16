/* Componente de Button: botão com variantes de estilo e estados. */
import styles from "./Button.module.css";
import type { ReactNode } from "react";

type Props = {
  children: ReactNode;
  onClick?: () => void;
  type?: "button" | "submit" | "reset";
  variant?: "primary" | "secondary" | "warning" | "danger";
  disabled?: boolean;
};

/** Botão reutilizável com variantes (primary, secondary, warning, danger). */
export function Button({ children, onClick, type = "button", variant = "primary", disabled }: Props) {
  return (
    <button className={`${styles.button} ${styles[variant]}`} onClick={onClick} type={type} disabled={disabled}>
      {children}
    </button>
  );
}
