# 概要

この文書では、GitHub Enterpriseの「AI Controls」タブで設定可能な各種ポリシーと、現状の設定について説明します。
これらのポリシーは、GitHub Copilotの利用に関する、Enterprise全体の制御設定です。

> [!NOTE]
> GitHubの機能追加に伴って設定項目が変わることが多いため、この文書ではユーザー側の利用に関わる主要な設定項目のみを記載しています。
> 全ての設定項目を網羅しているわけではないことにご留意ください。

## Copilot

### Configure allowed models

#### 概要

利用可能なモデルを設定します。

#### 設定項目

各モデルについて、以下のポリシーから選択できます：

1. Let organizations decide
    - Organizationが個別に有効/無効を設定できます。
2. Enabled everywhere
    - 全てのOrganizationおよびユーザーで有効になります。個別のOrganizationレベルで無効化できなくなります。
3. Disabled everywhere
    - 全てのOrganizationおよびユーザーで無効になります。個別のOrganizationレベルで有効化できなくなります。

#### 現状の設定

| Model | 現状の設定 |
|---|---|
| Anthropic Claude Sonnet 4 in Copilot | Let Organizations decide |
| Anthropic Claude Sonnet 4.5 in Copilot | Let Organizations decide |
| Anthropic Claude Haiku 4.5 in Copilot | Let Organizations decide |
| Anthropic Claude Opus 4.1 in Copilot | Let Organizations decide |
| Anthropic Claude Opus 4.5 in Copilot (Preview) | Let Organizations decide |
| Google Gemini 2.5 Pro in Copilot | Let Organizations decide |
| Google Gemini 3 Pro in Copilot (Preview) | Let Organizations decide |
| OpenAI GPT-5 in Copilot | Let Organizations decide |
| OpenAI GPT-5-Codex in Copilot (Preview) | Let Organizations decide |
| OpenAI GPT-5 mini in Copilot | Let Organizations decide |
| OpenAI GPT-5.1 in Copilot (Preview) | Let Organizations decide |
| OpenAI GPT-5.1-Codex in Copilot (Preview) | Let Organizations decide |
| OpenAI GPT-5.1-Codex-Mini in Copilot (Preview) | Let Organizations decide |
| xAI Grok Code Fast 1 in Copilot | Let Organizations decide |

## Privacy

### Suggestions matching public code

#### 概要

GitHub Copilotがパブリックコードに一致する提案を表示またはブロックするかどうかを制御します。

#### 設定項目

1. Allowed
    - パブリックコードに一致する提案を許可します。
2. Blocked
    - パブリックコードに一致する提案をブロックします。

#### 現状の設定

2. Blocked

## Features

### Spark (Preview)

#### 概要

[GitHub Spark](https://github.com/features/spark?locale=ja) の機能の利用を制御します。

#### 設定項目

1. Enabled everywhere
    - 全てのOrganizationでSparkを有効化します。
2. Disabled everywhere
    - 全てのOrganizationでSparkを無効化します。

また、以下のオプションがあります：

- [ ] Force Spark repos creation within the organization
    - チェックを入れると、ユーザーがSparkからリポジトリを作成する際、ユーザーアカウントではなく、Organization内に配置することを強制します。

#### 現状の設定

2. Disabled everywhere

- [ ] Force Spark repos creation within the organization: チェックなし

### Editor preview features

#### 概要

エディター用のプレビュー機能へのアクセスを制御します。

#### 設定項目

1. Let organizations decide
2. Enabled everywhere
3. Disabled everywhere

#### 現状の設定

- Select a policy（未選択）

### Copilot can search the web

#### 概要

CopilotがWebを検索できるようにします。

#### 設定項目

1. Let organizations decide
2. Enabled everywhere
3. Disabled everywhere

#### 現状の設定

1. Let organizations decide

### Copilot-generated commit messages

#### 概要

有効にすると、CopilotはGitHub.comでの変更に対してコミットメッセージを提案します。

#### 設定項目

1. Let organizations decide
2. Enabled everywhere
3. Disabled everywhere

#### 現状の設定

- Select a policy（未選択）

### Copilot Spaces

#### 概要

有効にすると、Organizationメンバーは[Copilot Spaces](https://docs.github.com/ja/copilot/how-tos/provide-context/use-copilot-spaces)を表示および作成できます。無効にすると、ユーザーはCopilot Spacesを表示または作成できません。

#### 設定項目

1. Let organizations decide
2. Enabled everywhere
3. Disabled everywhere

#### 現状の設定

- Select a policy（未選択）

## Billing

### Premium request paid usage

#### 概要

プレミアムリクエストの所定の利用枠を超えた分の利用(従量課金)の有効化を制御します。

#### 設定項目

1. Enabled
2. Enabled for selected products
3. Disabled

#### 現状の設定

1. Enabled

## Metrics

### Copilot metrics API

#### 概要

有効にすると、EnterpriseおよびOrganizationの管理者がCopilot metrics APIを通じてCopilot使用状況のメトリクスを照会できます。

#### 設定項目

1. Let organizations decide
2. Enabled everywhere
3. Disabled everywhere

#### 現状の設定

1. Let organizations decide

### Copilot usage metrics (Preview)

#### 概要

Enterpriseの管理者および請求マネージャーは、ダッシュボードで拡張されたCopilot使用状況メトリクスを表示できます。

#### 設定項目

1. Enabled
2. Disabled

#### 現状の設定

1. Enabled

## Copilot Clients

### Copilot in GitHub.com

#### 概要

GitHub.comでCopilot Chatおよびナレッジベース検索を使用可能にする設定です。

#### 設定項目

1. Let organizations decide
2. Enabled everywhere
3. Disabled everywhere

#### 現状の設定

1. Let organizations decide

### Copilot in the CLI (Preview)

#### 概要

[GitHub Copilot CLI](https://github.com/features/copilot/cli?locale=ja) を利用可能にする設定です。

#### 設定項目

1. Let organizations decide
2. Enabled everywhere
3. Disabled everywhere

#### 現状の設定

- Let organizations decide

### Copilot in GitHub Desktop

#### 概要

GitHub DesktopでGitHub Copilotを利用可能にする設定です。

#### 設定項目

1. Let organizations decide
2. Enabled everywhere
3. Disabled everywhere

#### 現状の設定

- Select a policy（未選択）

### Copilot Chat in the IDE

#### 概要

Copilot Chatをエディタで利用可能にする設定です。

#### 設定項目

1. Let organizations decide
2. Enabled everywhere
3. Disabled everywhere

#### 現状の設定

1. Let organizations decide

### Copilot Chat in GitHub Mobile

#### 概要

GitHub MobileアプリでCopilot Chatを利用可能にする設定です。

#### 設定項目

1. Let organizations decide
2. Enabled everywhere
3. Disabled everywhere

#### 現状の設定

1. Let organizations decide

### Copilot Agent Mode in IDE Chat

#### 概要

Copilot Agent Modeを利用可能にする設定です。

#### 設定項目

1. Let organizations decide
2. Enabled everywhere
3. Disabled everywhere

#### 現状の設定

2. Enabled everywhere
