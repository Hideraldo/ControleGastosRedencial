import { createContext } from "react";
import type { ReactNode } from "react";

type AppContextValue = {
  apiBaseUrl: string;
};

const AppContext = createContext<AppContextValue>({ apiBaseUrl: "" });

export function AppProvider({ children }: { children: ReactNode }) {
  const apiBaseUrl =
    (import.meta.env.VITE_API_BASE_URL as string) ?? "http://localhost:5243";
  return (
    <AppContext.Provider value={{ apiBaseUrl }}>
      {children}
    </AppContext.Provider>
  );
}
