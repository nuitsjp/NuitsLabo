# Hello Tauri

Tauri v2 の学習用プロジェクト。画面中央に「Hello, Tauri」と表示するデスクトップアプリケーション。

## 前提条件

- **Node.js** (v18 以上)
- **Rust** (`rustup` でインストール: https://rustup.rs/)
- **Visual Studio** の「C++ によるデスクトップ開発」ワークロード

## セットアップ

```bash
npm install
```

## 開発

ホットリロード付きで開発モードで起動する:

```bash
npm run tauri dev
```

## ビルド

リリース用にビルドする:

```bash
npm run tauri build
```

ビルド成果物は `src-tauri/target/release/` 以下に生成される:

| 成果物 | パス |
|--------|------|
| 実行ファイル | `src-tauri/target/release/hello-tauri.exe` |
| MSI インストーラー | `src-tauri/target/release/bundle/msi/hello-tauri_0.1.0_x64_en-US.msi` |
| NSIS インストーラー | `src-tauri/target/release/bundle/nsis/hello-tauri_0.1.0_x64-setup.exe` |

## プロジェクト構成

```
hello-tauri/
├── src/                        # フロントエンド
│   ├── index.html              # メイン HTML
│   └── styles.css              # スタイルシート
├── src-tauri/                  # バックエンド (Rust)
│   ├── src/
│   │   ├── main.rs             # エントリポイント
│   │   └── lib.rs              # アプリケーション定義
│   ├── tauri.conf.json         # Tauri 設定
│   └── Cargo.toml              # Rust 依存関係
└── package.json
```

## 環境構築で実施した内容

1. `rustup` で Rust ツールチェインをインストール
2. Visual Studio に「C++ によるデスクトップ開発」ワークロードを追加
3. `npm create tauri-app` でプロジェクトを生成 (vanilla テンプレート)
4. テンプレートを編集し「Hello, Tauri」を表示するシンプルな構成に変更
