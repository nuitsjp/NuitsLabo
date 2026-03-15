/// <reference lib="webworker" />

import type { SyncState } from "./useSharedSync";

type ClientMessage =
  | { type: "update"; state: SyncState }
  | { type: "disconnect" };
type WorkerMessage =
  | { type: "init"; state: SyncState }
  | { type: "sync"; state: SyncState };

let currentState: SyncState = { count: 0, text: "" };
const ports = new Set<MessagePort>();

declare const self: SharedWorkerGlobalScope;

self.onconnect = (e: MessageEvent) => {
  const port = e.ports[0];
  ports.add(port);

  // 新規接続時に現在の状態を即送信
  port.postMessage({ type: "init", state: currentState } satisfies WorkerMessage);

  port.onmessage = (event: MessageEvent<ClientMessage>) => {
    const { data } = event;
    if (data.type === "disconnect") {
      ports.delete(port);
      return;
    }
    if (data.type === "update") {
      currentState = data.state;
      // 全クライアントに同期
      for (const p of ports) {
        p.postMessage({ type: "sync", state: currentState } satisfies WorkerMessage);
      }
    }
  };

  port.start();
};
