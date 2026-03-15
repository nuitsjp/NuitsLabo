# SplitWindowStudy

React で親ウィンドウからポップアップ（子ウィンドウ）を開き、**SharedWorker** を用いて双方向にデータをリアルタイム同期するサンプルアプリケーションです。SharedWorker が Single Source of Truth として状態を一元管理し、途中参加タブにも現在の状態を即座に提供します。

## 起動方法

```bash
npm install
npm run dev
```

ブラウザで表示された URL（例: `http://localhost:5173/`）を開き、「子ウィンドウを開く」ボタンを押してください。

### 対応ブラウザ

本アプリは `new SharedWorker(..., { type: "module" })` (Module SharedWorker) を使用しています。以下のブラウザが必要です。

| ブラウザ | 最低バージョン |
|---|---|
| Chrome | 80+ |
| Edge | 80+ |
| macOS Safari | 16.0+ |

未対応ブラウザではウィンドウ間同期が無効になり、画面上にその旨が表示されます（クラッシュはしません）。

> **参考:** [MDN SharedWorker](https://developer.mozilla.org/en-US/docs/Web/API/SharedWorker) / [Can I use: SharedWorkers](https://caniuse.com/sharedworkers) / [Can I use: JS modules in shared workers](https://caniuse.com/wf-js-modules-shared-workers)

---

## アーキテクチャ概要

### コンポーネント構成図

```mermaid
graph TB
    subgraph Browser["ブラウザ (同一オリジン)"]
        SW["SharedWorker<br/>sync-worker.ts<br/>(Single Source of Truth)"]

        subgraph ParentTab["親ウィンドウ<br/>URL: /"]
            main_p["main.tsx<br/>(mode判定)"] --> PW["ParentWindow.tsx"]
            PW --> hook_p["useSharedSync()"]
        end

        subgraph ChildTab["子ウィンドウ (ポップアップ)<br/>URL: /?mode=child"]
            main_c["main.tsx<br/>(mode判定)"] --> CW["ChildWindow.tsx"]
            CW --> hook_c["useSharedSync()"]
        end

        hook_p <-->|"MessagePort"| SW
        hook_c <-->|"MessagePort"| SW
    end
```

### ファイル構成と責務

```mermaid
graph LR
    subgraph src
        M["main.tsx"] -->|"?mode=child なし"| P["ParentWindow.tsx"]
        M -->|"?mode=child あり"| C["ChildWindow.tsx"]
        P --> H["useSharedSync.ts"]
        C --> H
        H -->|"MessagePort"| W["sync-worker.ts"]
    end
```

| ファイル | 責務 |
|---|---|
| `main.tsx` | エントリポイント。URL の `?mode=child` パラメータで描画コンポーネントを切り替え |
| `ParentWindow.tsx` | 親画面 UI。`window.open()` で子ウィンドウをポップアップとして起動 |
| `ChildWindow.tsx` | 子画面 UI。親と同じ操作（カウンター・テキスト入力）を提供 |
| `useSharedSync.ts` | 双方向同期のカスタムフック。SharedWorker への接続・メッセージ送受信をカプセル化 |
| `sync-worker.ts` | SharedWorker スクリプト。状態の一元管理（Single Source of Truth）と全クライアントへのブロードキャストを担当 |

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
    participant Hook_P as useSharedSync<br/>(親側インスタンス)
    participant SW as SharedWorker<br/>(sync-worker.ts)
    participant Hook_C as useSharedSync<br/>(子側インスタンス)
    participant CW as 子ウィンドウ<br/>(ChildWindow)

    User->>PW: カウンター「+」ボタンをクリック
    PW->>Hook_P: updateState(s => ({...s, count: s.count + 1}))
    Hook_P->>Hook_P: setState で count を楽観的に更新
    Hook_P->>SW: port.postMessage({ type: "update", state })
    SW->>SW: currentState を更新
    SW->>Hook_P: port.postMessage({ type: "sync", state })
    SW->>Hook_C: port.postMessage({ type: "sync", state })
    Hook_C->>Hook_C: setState({ count: 1, text: "" })
    Hook_C-->>CW: 再レンダリング (count: 1)
```

### シーケンス図: 子 → 親の同期

```mermaid
sequenceDiagram
    actor User as ユーザー
    participant CW as 子ウィンドウ<br/>(ChildWindow)
    participant Hook_C as useSharedSync<br/>(子側インスタンス)
    participant SW as SharedWorker<br/>(sync-worker.ts)
    participant Hook_P as useSharedSync<br/>(親側インスタンス)
    participant PW as 親ウィンドウ<br/>(ParentWindow)

    User->>CW: テキスト入力欄に "Hello" と入力
    CW->>Hook_C: updateState(s => ({...s, text: "Hello"}))
    Hook_C->>Hook_C: setState で text を楽観的に更新
    Hook_C->>SW: port.postMessage({ type: "update", state })
    SW->>SW: currentState を更新
    SW->>Hook_C: port.postMessage({ type: "sync", state })
    SW->>Hook_P: port.postMessage({ type: "sync", state })
    Hook_P->>Hook_P: setState({ count: 1, text: "Hello" })
    Hook_P-->>PW: 再レンダリング (text: "Hello")
```

### シーケンス図: 途中参加タブの状態取得

SharedWorker の最大の利点 — 後から開いたタブが初期値ではなく現在の状態を即座に取得できます。

```mermaid
sequenceDiagram
    actor User as ユーザー
    participant PW as 親ウィンドウ
    participant SW as SharedWorker<br/>(currentState を保持)
    participant NewTab as 新しい子ウィンドウ

    Note over PW,SW: 親ウィンドウで操作済み<br/>currentState = { count: 5, text: "Hello" }

    User->>PW: 「子ウィンドウを開く」クリック
    PW->>NewTab: window.open("/?mode=child")
    NewTab->>SW: new SharedWorker() で接続
    SW->>NewTab: port.postMessage({ type: "init",<br/>state: { count: 5, text: "Hello" } })
    NewTab->>NewTab: setState({ count: 5, text: "Hello" })
    Note over NewTab: 初期値 (0, "") ではなく<br/>現在値 (5, "Hello") で表示開始
```

---

## 子ウィンドウの起動フロー

```mermaid
sequenceDiagram
    actor User as ユーザー
    participant PW as ParentWindow
    participant W as window.open()
    participant Browser as ブラウザ
    participant SW as SharedWorker

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
        Browser->>SW: useSharedSync() で<br/>SharedWorker に接続
        SW->>Browser: { type: "init", state } で<br/>現在の状態を即送信
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

本サンプルでは **SharedWorker** を採用しています。

- **途中参加タブへの状態提供**: 後から開いたタブが即座に現在の状態を取得でき、BroadcastChannel の「初期値から開始」問題を解決
- **Single Source of Truth**: SharedWorker が唯一の状態管理者となり、各タブ間の状態不整合を防止
- **再送防止ロジック不要**: プロトコル設計により、BroadcastChannel で必要だった `isReceiving` フラグが不要に
