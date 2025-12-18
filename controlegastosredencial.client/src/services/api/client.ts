/* Cliente HTTP simples baseado em fetch, com tratamento de erros e JSON. */
type HttpMethod = "GET" | "POST" | "PUT" | "DELETE";

let RESOLVED_BASE: string | null = null;
const CANDIDATES: string[] = [
  ...(import.meta.env.VITE_API_BASE_URL ? [import.meta.env.VITE_API_BASE_URL.toString()] : []),
  "http://localhost:5243",
  "https://localhost:7090",
];

async function resolveBase(): Promise<string> {
  if (RESOLVED_BASE) return RESOLVED_BASE;
  for (const base of CANDIDATES) {
    try {
      const res = await fetch(`${base}/api/health`, { method: "GET" });
      if (res.ok) {
        RESOLVED_BASE = base;
        return RESOLVED_BASE;
      }
    } catch {
      // tenta próximo
    }
  }
  // fallback final: usa primeira candidata ou origem do site
  RESOLVED_BASE = CANDIDATES[0] ?? `${location.protocol}//${location.host}`;
  return RESOLVED_BASE;
}

/** Função utilitária para requisições HTTP ao backend. */
async function request<T>(
  path: string,
  options?: { method?: HttpMethod; body?: unknown }
): Promise<T> {
  const base = await resolveBase();
  const url = `${base}${path}`;
  const res = await fetch(url, {
    method: options?.method ?? "GET",
    headers: {
      "Content-Type": "application/json",
    },
    body: options?.body ? JSON.stringify(options.body) : undefined,
  });
  if (!res.ok) {
    const text = await res.text();
    throw new Error(text || `HTTP ${res.status} on ${url}`);
  }
  const contentType = res.headers.get("content-type");
  if (contentType && contentType.includes("application/json")) {
    return (await res.json()) as T;
  }
  // No content (204) or non-json
  return undefined as unknown as T;
}

export const api = {
  /** Executa requisição GET. */
  get: <T>(path: string) => request<T>(path),
  /** Executa requisição POST com corpo JSON. */
  post: <T>(path: string, body: unknown) =>
    request<T>(path, { method: "POST", body }),
  /** Executa requisição PUT com corpo JSON. */
  put: <T>(path: string, body: unknown) =>
    request<T>(path, { method: "PUT", body }),
  /** Executa requisição DELETE. */
  delete: <T>(path: string) => request<T>(path, { method: "DELETE" }),
};
