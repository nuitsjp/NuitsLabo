# SplitWindowStudy

React で親ウィンドウからポップアップ（子ウィンドウ）を開き、**BroadcastChannel API** を用いて双方向にデータをリアルタイム同期するサンプルアプリケーションです。

## 起動方法

```bash
npm install
npm run dev
```

ブラウザで表示された URL（例: `http://localhost:5173/`）を開き、「子ウィンドウを開く」ボタンを押してください。

---

## アーキテクチャ概要

### コンポーネント構成図

```mermaid
graph TB
    subgraph Browser["ブラウザ (同一オリジン)"]
        subgraph ParentTab["親ウィンドウ<br/>URL: /"]
            main_p["main.tsx<br/>(mode判定)"] --> PW["ParentWindow.tsx"]
            PW --> hook_p["useBroadcastSync()"]
        end

        subgraph ChildTab["子ウィンドウ (ポップアップ)<br/>URL: /?mode=child"]
            main_c["main.tsx<br/>(mode判定)"] --> CW["ChildWindow.tsx"]
            CW --> hook_c["useBroadcastSync()"]
        end

        hook_p <-->|"BroadcastChannel<br/>split-window-sync"| hook_c
    end
```

### ファイル構成と責務

```mermaid
graph LR
    subgraph src
        M["main.tsx"] -->|"?mode=child なし"| P["ParentWindow.tsx"]
        M -->|"?mode=child あり"| C["ChildWindow.tsx"]
        P --> H["useBroadcastSync.ts"]
        C --> H
    end
```

| ファイル | 責務 |
|---|---|
| `main.tsx` | エントリポイント。URL の `?mode=child` パラメータで描画コンポーネントを切り替え |
| `ParentWindow.tsx` | 親画面 UI。`window.open()` で子ウィンドウをポップアップとして起動 |
| `ChildWindow.tsx` | 子画面 UI。親と同じ操作（カウンター・テキスト入力）を提供 |
| `useBroadcastSync.ts` | 双方向同期のカスタムフック。BroadcastChannel による送受信ロジックをカプセル化 |

---

## データ同期の仕組み

### 同期対象のデータ構造

```typescript
interface SyncState {
  count: number;  // カウンター値
  text: string;   // テキスト入力値
}
```

### シーケンス図: 親 → 子の同期

```mermaid
sequenceDiagram
    actor User as ユーザー
    participant PW as 親ウィンドウ<br/>(ParentWindow)
    participant Hook_P as useBroadcastSync<br/>(親側インスタンス)
    participant BC as BroadcastChannel<br/>"split-window-sync"
    participant Hook_C as useBroadcastSync<br/>(子側インスタンス)
    participant CW as 子ウィンドウ<br/>(ChildWindow)

    User->>PW: カウンター「+」ボタンをクリック
    PW->>Hook_P: updateState(s => ({...s, count: s.count + 1}))
    Hook_P->>Hook_P: setState で count を更新
    Hook_P->>BC: postMessage({ count: 1, text: "" })
    BC-->>Hook_C: onmessage イベント発火
    Hook_C->>Hook_C: isReceiving = true
    Hook_C->>Hook_C: setState({ count: 1, text: "" })
    Hook_C-->>CW: 再レンダリング (count: 1)
    Hook_C->>Hook_C: requestAnimationFrame で<br/>isReceiving = false に戻す
```

### シーケンス図: 子 → 親の同期

```mermaid
sequenceDiagram
    actor User as ユーザー
    participant CW as 子ウィンドウ<br/>(ChildWindow)
    participant Hook_C as useBroadcastSync<br/>(子側インスタンス)
    participant BC as BroadcastChannel<br/>"split-window-sync"
    participant Hook_P as useBroadcastSync<br/>(親側インスタンス)
    participant PW as 親ウィンドウ<br/>(ParentWindow)

    User->>CW: テキスト入力欄に "Hello" と入力
    CW->>Hook_C: updateState(s => ({...s, text: "Hello"}))
    Hook_C->>Hook_C: setState で text を更新
    Hook_C->>BC: postMessage({ count: 1, text: "Hello" })
    BC-->>Hook_P: onmessage イベント発火
    Hook_P->>Hook_P: isReceiving = true
    Hook_P->>Hook_P: setState({ count: 1, text: "Hello" })
    Hook_P-->>PW: 再レンダリング (text: "Hello")
    Hook_P->>Hook_P: requestAnimationFrame で<br/>isReceiving = false に戻す
```

### 再送防止メカニズム

BroadcastChannel は受信側で `setState` を呼ぶと `updateState` 経由で再度 `postMessage` してしまう可能性があります。これを防ぐために `isReceiving` フラグを使用しています。

```mermaid
flowchart TD
    A["updateState が呼ばれる"] --> B{"isReceiving<br/>フラグは true?"}
    B -->|"Yes (受信による更新)"| C["setState のみ実行<br/>postMessage しない"]
    B -->|"No (ユーザー操作)"| D["setState 実行"]
    D --> E["postMessage で<br/>相手ウィンドウに送信"]
```

---

## 子ウィンドウの起動フロー

```mermaid
sequenceDiagram
    actor User as ユーザー
    participant PW as ParentWindow
    participant W as window.open()
    participant Browser as ブラウザ

    User->>PW: 「子ウィンドウを開く」クリック
    PW->>PW: childWindowRef が<br/>既に開いている？

    alt 既に開いている
        PW->>Browser: childWindowRef.focus()
        Note over Browser: 既存の子ウィンドウに<br/>フォーカスを移す
    else 未起動 or 閉じられた
        PW->>W: window.open("/?mode=child",<br/>"child-window", specs)
        W->>Browser: ポップアップウィンドウを生成
        Browser->>Browser: main.tsx が ?mode=child を検出
        Browser->>Browser: ChildWindow を描画
        Browser->>Browser: useBroadcastSync() で<br/>BroadcastChannel に接続
    end
```

---

## 技術選定: ウィンドウ間通信 API 比較

| 特性 | BroadcastChannel | window.postMessage | SharedWorker |
|---|---|---|---|
| セットアップの容易さ | 簡単 | やや複雑 | 複雑 |
| 通信方向 | 多対多 | 1対1 | 多対多 |
| ウィンドウ参照の保持 | 不要 | 必要 | 不要 |
| 同一オリジン制約 | あり | なし (cross-origin 可) | あり |
| ブラウザサポート | モダンブラウザ全対応 | 全ブラウザ | モダンブラウザ全対応 |
| 中央集権的な状態管理 | なし（各タブが独自に保持） | なし | Worker が Single Source of Truth を持てる |
| 途中参加タブへの状態提供 | 不可（初期値から開始） | 送信側が参照を持てば可能 | 接続時に現在値を即座に提供 |
| 重い計算のオフロード | 不可（UI スレッドで実行） | 不可（UI スレッドで実行） | 別スレッドで実行可能 |
| 接続中クライアントの把握 | 不可 | 送信側が管理すれば可能 | port で接続・切断を把握可能 |
| 競合・整合性制御 | なし（各タブが自由に更新） | なし | Worker 側で排他制御・バリデーション可能 |

### 選定フローチャート

```mermaid
flowchart TD
    Start(["ウィンドウ間でデータを同期したい"]) --> Q1{"cross-origin 通信が必要？"}
    Q1 -->|"Yes"| R1["window.postMessage"]
    Q1 -->|"No"| Q2{"途中参加するタブ/ウィンドウに<br/>現在の状態を渡す必要がある？"}
    Q2 -->|"Yes"| R2["SharedWorker"]
    Q2 -->|"No"| Q3{"複数タブからの同時更新で<br/>競合制御が必要？"}
    Q3 -->|"Yes"| R2
    Q3 -->|"No"| Q4{"重い計算を<br/>UI スレッドから分離したい？"}
    Q4 -->|"Yes"| R2
    Q4 -->|"No"| R3["BroadcastChannel"]

    R1:::selected
    R2:::selected
    R3:::selected

    classDef selected fill:#2563eb,color:#fff,stroke:#1d4ed8,stroke-width:2px
```

### 本サンプルでの選定理由

本サンプルでは学習目的でのシンプルさを優先し **BroadcastChannel** を採用しています。

ただし、途中参加タブへの状態提供・競合制御・重い計算のオフロードが必要になった場合は **SharedWorker** への移行を検討してください。
