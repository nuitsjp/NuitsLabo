import { useRef } from "react";
import { useBroadcastSync } from "./useBroadcastSync";

export function ParentWindow() {
  const { state, updateState } = useBroadcastSync();
  const childWindowRef = useRef<Window | null>(null);

  const openChild = () => {
    if (childWindowRef.current && !childWindowRef.current.closed) {
      childWindowRef.current.focus();
      return;
    }
    childWindowRef.current = window.open(
      `${window.location.origin}?mode=child`,
      "child-window",
      "width=500,height=400,left=200,top=200"
    );
  };

  return (
    <div style={{ padding: 32, fontFamily: "sans-serif" }}>
      <h1>親ウィンドウ</h1>

      <button onClick={openChild} style={{ marginBottom: 24, padding: "8px 16px", fontSize: 16 }}>
        子ウィンドウを開く
      </button>

      <div style={{ marginBottom: 16 }}>
        <label>カウンター: </label>
        <button onClick={() => updateState((s) => ({ ...s, count: s.count - 1 }))}>-</button>
        <span style={{ margin: "0 12px", fontSize: 24, fontWeight: "bold" }}>{state.count}</span>
        <button onClick={() => updateState((s) => ({ ...s, count: s.count + 1 }))}>+</button>
      </div>

      <div>
        <label>テキスト: </label>
        <input
          type="text"
          value={state.text}
          onChange={(e) => updateState((s) => ({ ...s, text: e.target.value }))}
          style={{ width: 300, padding: 4, fontSize: 16 }}
        />
      </div>

      <p style={{ marginTop: 24, color: "#666" }}>
        ↑ 上の値を変更すると、子ウィンドウにリアルタイムで同期されます。
      </p>
    </div>
  );
}
