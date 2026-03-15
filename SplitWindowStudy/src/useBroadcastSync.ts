import { useState, useEffect, useCallback, useRef } from "react";

export interface SyncState {
  count: number;
  text: string;
}

const CHANNEL_NAME = "split-window-sync";

const initialState: SyncState = { count: 0, text: "" };

export function useBroadcastSync() {
  const [state, setState] = useState<SyncState>(initialState);
  const channelRef = useRef<BroadcastChannel | null>(null);
  // 受信による更新中かどうかを示すフラグ（再送防止用）
  const isReceiving = useRef(false);

  useEffect(() => {
    const channel = new BroadcastChannel(CHANNEL_NAME);
    channelRef.current = channel;

    channel.onmessage = (event: MessageEvent<SyncState>) => {
      isReceiving.current = true;
      setState(event.data);
      // setState の反映後にフラグを戻す
      requestAnimationFrame(() => {
        isReceiving.current = false;
      });
    };

    return () => {
      channel.close();
      channelRef.current = null;
    };
  }, []);

  const updateState = useCallback((updater: (prev: SyncState) => SyncState) => {
    setState((prev) => {
      const next = updater(prev);
      // 受信による更新でなければ相手側に送信
      if (!isReceiving.current && channelRef.current) {
        channelRef.current.postMessage(next);
      }
      return next;
    });
  }, []);

  return { state, updateState };
}
