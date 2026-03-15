import { useBroadcastSync } from "./useBroadcastSync";

export function ChildWindow() {
  const { state, updateState } = useBroadcastSync();

  return (
    <div style={{ padding: 32, fontFamily: "sans-serif" }}>
      <h1>子ウィンドウ</h1>

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
        ↑ ここで変更すると、親ウィンドウにもリアルタイムで反映されます。
      </p>
    </div>
  );
}
