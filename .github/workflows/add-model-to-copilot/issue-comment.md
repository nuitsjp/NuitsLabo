## Add Model to Copilot

「Copilotへ新しいモデルの追加リクエスト」を下記にしたがって処理してください。

### 実行手順

1. **Issueからモデル名を抽出**
   - Issueのタイトルから新しいモデル名を取得します
   - タイトル形式: "Copilotへ新しいモデル <model_name in Copilot> の追加リクエスト"
   - **Issueタイトルからモデル名を抽出できない場合**: 2以降の手順を一切実行せず処理を中断し、Pull Requestを更新する:
      - タイトル: "モデルの抽出エラー: Issueタイトルの形式が不正です"
      - 本文:
        ```
        Issueタイトルからモデル名を抽出できませんでした。

        タイトルは以下の形式で記載してください:
        「Copilotへ新しいモデル <モデル名> in Copilot の追加リクエスト」

        例: 「Copilotへ新しいモデル OpenAI GPT-6 in Copilot の追加リクエスト」
        ```

2. **モデル情報の準備**
   - 抽出したモデル名から "(Preview)" を削除した形式を使用します
   - デフォルトの設定値: "Let Organizations decide"

3. **AI Controls.mdの更新**
   - ファイルパス: `/enterprise-settings/AI Controls.md`
   - 対象セクション: "Configure allowed models" → "現状の設定" のテーブル
   - **重複チェック**: すでに同じモデルがテーブルに存在するか確認する（Previewの有無だけが異なる場合は重複とみなす）
   - 新しいモデルを以下のルールにしたがって挿入:
     - ベンダー順（Anthropic → Google → OpenAI → xAI -> ほか）
     - 各ベンダー内ではモデル名のアルファベット順

       テーブル形式:
       ```markdown
       | <Model Name> in Copilot | Let Organizations decide |
       ```

   - **モデル重複時の処理**: 4以降の手順を一切実行せず処理を中断し、以下のコマンドを実行する:
      - タイトル: "モデル重複エラー: 指定されたモデルはすでに登録済みです"
      - 本文:
        ```
        指定されたモデル「<モデル名>」は既に登録済みです。

        新しいモデルを追加する場合は、未登録のモデル名でIssueを作成してください。

        Pull RequestおよびIssueをクローズしてください。
        ```
4. **Pull Requestの更新（Draft）**
   - PRテンプレート: `/.github/PULL_REQUEST_TEMPLATE/add-model-to-copilot.md` を使用
   - PRタイトル: Issueタイトルと完全に一致させる（例: "Copilotへ新しいモデルOpenAI GPT-6 in Copilotの追加リクエスト"）
   - PR本文で以下を記入:
     - モデル名
     - 設定値（デフォルト: "Let Organizations decide"）
     - 変更ファイルのチェック
     - 関連Issue番号

5. **バリデーション**
   - モデル名が正確に記載されているか確認
   - テーブルのMarkdown形式が正しいか確認
   - アルファベット順・製品順に適切な位置に挿入されているか確認

6. **PRをレビュー可能状態に変更**
   - Draft PRを「Ready for review」に変更
   - コマンド: `gh pr ready <PR番号>`
