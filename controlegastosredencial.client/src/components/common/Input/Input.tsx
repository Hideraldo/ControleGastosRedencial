/* Componente de Input: campo de entrada com label opcional. */
import styles from "./Input.module.css";

type Props = {
  label?: string;
  value: string | number;
  onChange: (v: string) => void;
  type?: string;
  placeholder?: string;
};

/** Componente reutilizável de input de texto/number com label. */
export function Input({ label, value, onChange, type = "text", placeholder }: Props) {
  return (
    <label className={styles.field}>
      {label && <span className={styles.label}>{label}</span>}
      <input
        className={styles.input}
        type={type}
        value={value}
        placeholder={placeholder}
        onChange={(e) => onChange(e.target.value)}
      />
    </label>
  );
}
