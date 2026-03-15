import { useState, useEffect, useCallback, useRef } from "react";

export interface SyncState {
  count: number;
  text: string;
}

const initialState: SyncState = { count: 0, text: "" };

export function useSharedSync() {
  const [state, setState] = useState<SyncState>(initialState);
  const [supported, setSupported] = useState(true);
  const portRef = useRef<MessagePort | null>(null);

  useEffect(() => {
    try {
      const worker = new SharedWorker(
        new URL("./sync-worker.ts", import.meta.url),
        { type: "module" }
      );
      const port = worker.port;
      portRef.current = port;

      port.onmessage = (event: MessageEvent) => {
        const data = event.data;
        if (data.type === "init" || data.type === "sync") {
          setState(data.state);
        }
      };

      port.start();

      return () => {
        port.postMessage({ type: "disconnect" });
        port.close();
        portRef.current = null;
      };
    } catch {
      console.warn(
        "Module SharedWorker is not supported in this browser. " +
        "Cross-window sync will not work."
      );
      setSupported(false);
    }
  }, []);

  const updateState = useCallback((updater: (prev: SyncState) => SyncState) => {
    setState((prev) => {
      const next = updater(prev);
      portRef.current?.postMessage({ type: "update", state: next });
      return next;
    });
  }, []);

  return { state, updateState, supported };
}
