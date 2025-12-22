---
name: Copilotへ新しいモデルの追加
description: Copilotへ新しいモデルを追加するリクエストを自動で支援するエージェント
---

## Add Model to Copilot

このエージェントは、GitHub Issueで建てられた「Copilotへ新しいモデルの追加リクエスト」に対応し、自動的にドキュメントを更新してPull Requestを作成します。

### 実行手順

1. **Issueからモデル名を抽出**
   - Issueのタイトルから新しいモデル名を取得します
   - タイトル形式: "Copilotへ新しいモデル <model_name in Copilot> の追加リクエスト"
   - モデル名は既に " in Copilot" を含んでいます（例: "OpenAI GPT-6 in Copilot"）
   - モデル名に "(Preview)" が含まれている場合は削除します
   - **エラー時の処理**: Issueタイトルからモデル名を抽出できない場合は、**以降の手順を一切実行せず処理を中断**し、以下のコマンドを実行する:
     ```bash
     # Issueにエラーメッセージを投稿
     gh issue comment <Issue番号> --body "@<Issue作成者> Issueタイトルからモデル名を抽出できませんでした。

     タイトルは以下の形式で記載してください:
     「Copilotへ新しいモデル <モデル名> in Copilot の追加リクエスト」

     例: 「Copilotへ新しいモデル OpenAI GPT-6 in Copilot の追加リクエスト」"

     # Pull RequestをClose
     gh pr close <PR番号>
     ```

2. **モデル情報の準備**
   - 抽出したモデル名から "(Preview)" を削除した形式を使用します
   - デフォルトの設定値: "Let Organizations decide"

3. **AI Controls.mdの更新**
   - ファイルパス: `enterprise-settings/AI Controls.md`
   - 対象セクション: "Configure allowed models" → "現状の設定" のテーブル（31-47行目付近）
   - **重複チェック**: 既に同じモデルがテーブルに存在するか確認する
   - **エラー時の処理**: 既に同じモデルが登録済みの場合は、**以降の手順を一切実行せず処理を中断**し、以下のコマンドを実行する:
     ```bash
     # Issueにエラーメッセージを投稿
     gh issue comment <Issue番号> --body "@<Issue作成者> 指定されたモデル「<モデル名>」は既に登録済みです。

     新しいモデルを追加する場合は、未登録のモデル名でIssueを作成してください。"

     # IssueをClose
     gh issue close <Issue番号>

     # Pull RequestをClose
     gh pr close <PR番号>
     ```
   - 新しいモデルを以下のルールに従って挿入:
     - ベンダー順（Anthropic → Google → OpenAI → xAI）
     - 各ベンダー内ではモデル名のアルファベット順
   - **注意**: テーブルの形式が壊れないよう、慎重に編集する
   
   テーブル形式:
   ```markdown
   | <Model Name> in Copilot | Let Organizations decide |
   ```

4. **Pull Requestの更新（Draft）**
   - PRテンプレート: `.github/PULL_REQUEST_TEMPLATE/add-model-to-copilot.md` を使用
   - PRタイトル: Issueタイトルと完全に一致させる（例: "Copilotへ新しいモデル OpenAI GPT-6 in Copilot の追加リクエスト"）
   - **Draft PRとして作成**: `gh pr create --draft` オプションを使用
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
