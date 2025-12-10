# 概要

この文書では、GitHub Enterpriseの「Policies」タブで設定可能な各種ポリシーと、現状の設定について説明します。
これらのポリシーは、Enterprise内の全てのOrganizationに適用されるグローバルな設定です。

> [!NOTE]
> GitHubの機能追加に伴って設定項目が変わることが多いため、この文書ではユーザー側の利用に関わる主要な設定項目のみを記載しています。
> 全ての設定項目を網羅しているわけではないことにご留意ください。

## Member privileges

### Unaffiliated users

#### 概要

あるメンバーが全てのOrganizationから削除されると、Unaffiliated users(未所属ユーザー)になります。この設定では、そのようなユーザーをEnterprise内に残すか、完全に削除するかを選択できます。

#### 設定項目

1. Remain in the enterprise
    - Unaffiliated usersがEnterpriseに留まることを許可します。Organizationに所属していなくてもEnterprise内に存在できます。
2. Remove from the enterprise
    - ユーザーが全てのOrganizationから削除された場合、Enterpriseからも削除されます。

#### 現状の設定

1. Remain in the enterprise

### Base Permissions

#### 概要

Base Permissionsは、Organizationに所属する全てのメンバーに対して、Organization内の複数のリポジトリ、メンバー、コラボレーターへのアクセス権限をデフォルトで付与する設定です。Enterpriseレベルでポリシーを設定することで、全てのOrganizationに統一された基本権限を適用できます。

#### 設定項目

1. No policy
    - Enterpriseレベルでは設定せず、各Organizationでポリシーを設定できるようにします。
2. No permission
    - 全てのOrganizationに対してデフォルト権限を付与しないポリシーを強制します。
3. Read
    - 全てのOrganizationに対してデフォルトでRead権限を付与するポリシーを強制します。
4. Write
    - 全てのOrganizationに対してデフォルトでWrite権限を付与するポリシーを強制します。
5. Admin
    - 全てのOrganizationに対してデフォルトでAdmin権限を付与するポリシーを強制します。

#### 現状の設定

1. No policy

### Repository creation

#### 概要

Enterprise内で、誰がリポジトリを作成できるか、そしてどのような可視性(visibility)のリポジトリを作成できるかを制御する設定です。

#### 設定項目

1. No policy
    - Enterpriseレベルでは設定せず、各Organizationが独自にポリシーを設定します。
2. Disabled
    - Enterprise内のメンバーによるリポジトリ作成を禁止します。
3. Members can create repositories
    - Enterprise内の全メンバーが、指定された可視性(visibility)のリポジトリを作成できます。
    - 可視性は下記の項目を個別に許可することができます
        - [ ] Public: インターネット上の誰でも閲覧可能なリポジトリ
        - [ ] Private: Organization内の許可されたメンバーのみが閲覧可能なリポジトリ
        - [ ] Internal: Enterprise内のメンバーが閲覧可能なリポジトリ

#### 現状の設定

3. Members can create repositories
    - [ ] Public
    - [x] Private
    - [x] Internal

### Repository forking

#### 概要

有効にすると、Enterprise内の全てのプライベートおよびinternalリポジトリに対してフォーク(複製)が許可されます。無効にすると、Enterprise内の全てのプライベートおよびinternalリポジトリのフォークが禁止されます。

#### 設定項目

1. All organizations: Disabled
    - Enterprise内の全てのプライベートおよびinternalリポジトリでフォークを禁止します。
2. All organizations: Enabled
    - Enterprise内の全てのプライベートおよびinternalリポジトリでフォークを許可します。

有効にした場合、フォークを利用可能な範囲を以下のオプションから選択できます

1. Organizations within this enterprise
    - メンバーはEnterprise内のOrganizationにリポジトリをフォークできます。
2. Within the same organization
    - メンバーは同じOrganization内(intra-org)でのみリポジトリをフォークできます。
3. User accounts and within the same organization
    - メンバーは自分のユーザーアカウント、または同じOrganization内にリポジトリをフォークできます。
4. User accounts and organizations within this enterprise
    - メンバーは自分のユーザーアカウント、またはEnterprise内のOrganizationにリポジトリをフォークできます。
5. User accounts
    - メンバーは自分のユーザーアカウントにのみリポジトリをフォークできます。
6. Everywhere
    - メンバーは自分のユーザーアカウント、またはEnterprise内外のOrganizationにリポジトリをフォークできます。

#### 現状の設定

2. All organizations: Enabled

フォークを利用可能な範囲 : 

1. Organizations within this enterprise

### Outside collaborators

#### 概要

Outside collaboratorsをリポジトリに招待する権限を制御する設定です。

#### 設定項目

1. No policy
    - Organizationの管理者がOutside collaboratorの招待ポリシーを自由に設定できます。
2. Repository admins allowed
    - リポジトリ管理者がOutside collaboratorを招待できるようになります。
3. Organization owners only
    - OrganizationのオーナーのみがOutside collaboratorを招待できます。リポジトリ管理者は招待できません。
4. Enterprise owners only
    - EnterpriseのオーナーのみがOutside collaboratorを招待できます。Organizationオーナーやリポジトリ管理者は招待できません。

#### 現状の設定

3. Organization owners only

### Default branch name

#### 概要

Default branch name（デフォルトブランチ名）は、Enterprise内で新規に作成されるリポジトリに対して適用されるデフォルトのブランチ名を設定する機能です。

#### 設定項目

- デフォルトブランチ名の設定
    - Enterprise内の新規リポジトリで使用されるデフォルトのブランチ名を指定します（例：main, master, develop など）。
    - 個別のリポジトリでは、このデフォルト設定を上書きすることが可能です。
- [ ] Enforce across this enterprise (チェックボックス)
    - 有効にすると、Enterprise内の全てのOrganizationで指定したデフォルトブランチ名が強制され、Organizationレベルでの変更ができなくなります。
    - 無効の場合、Organizationごとに異なるデフォルトブランチ名を設定できます。

#### 現状の設定

- デフォルトブランチ名: main
- [ ] Enforce across this enterprise: チェックなし

### Deploy keys

#### 概要

Deploy keysは、単一のリポジトリに対して読み取りまたは書き込みアクセスを許可するSSH鍵です。この項目では、Deploy keysに関するポリシーを制御します。

#### 設定項目

1. No policy
    - Organizationは独自のデプロイキーポリシーを設定できます。新しいOrganizationではDeploy Keysのポリシーはデフォルトで無効です。
2. Disabled
    - Enterpriseが所有する全てのリポジトリでデプロイキーの作成と使用が禁止されます。パブリックリポジトリのクローン作成には影響しません。
3. Enabled
    - Enterprise内でデプロイキーの作成と使用が許可されます。

#### 現状の設定

3. Enabled

## Admin repository permissions

### Repository visibility change

#### 概要

Repository visibility changeは、リポジトリの管理者がリポジトリの可視性（public、private、internal）を変更できる権限を制御する設定です。

#### 設定項目

1. No policy
    - Enterpriseレベルでは設定せず、各Organizationが独自にリポジトリ可視性変更のポリシーを設定できます。
2. All organizations: Disabled
    - Organizationのオーナーのみがリポジトリの可視性を変更できます。リポジトリの管理者権限を持つメンバーでも変更できません。
3. All organizations: Enabled
    - リポジトリの管理者権限を持つメンバーがリポジトリの可視性を変更できます。

#### 現状の設定

2. All organizations: Disabled

### Repository deletion and transfer

#### 概要

Repository deletion and transferは、リポジトリの削除や他Organizationsへの転送の権限を制御する設定です。

#### 設定項目

1. No policy
    - Enterpriseレベルでは設定せず、各Organizationが独自にリポジトリ削除・転送のポリシーを設定できます。
2. All organizations: Disabled
    - Organizationのオーナーのみがpublicおよびprivateリポジトリを削除または転送できます。リポジトリの管理者権限を持つメンバーでも削除や転送はできません。
3. All organizations: Enabled
    - リポジトリの管理者権限を持つメンバーがpublicおよびprivateリポジトリを削除または転送できます。

#### 現状の設定

2. All organizations: Disabled

### Repository issue deletion

#### 概要

Repository issue deletionは、リポジトリ内のIssueを削除できる権限を制御する設定です。

#### 設定項目

1. No policy
    - Enterpriseレベルでは設定せず、各Organizationが独自にIssue削除のポリシーを設定できます。
2. Enabled
    - リポジトリの管理者権限を持つメンバーがリポジトリ内のIssueを削除できます。
3. Disabled
    - Organizationのオーナーのみが Issueを削除できます。リポジトリの管理者権限を持つメンバーでもIssueの削除はできません。

#### 現状の設定

1. No policy

## Actions

### Policies

#### 概要

Actionsは全てのOrganization、または特定のOrganizationでのみ有効にできます。無効にした場合、GitHub Actionsが利用できません。

#### 設定項目

1. Enable for all organizations
    - Enterprise内の全てのOrganizationでGitHub Actionsを有効にします。
2. Allow enterprise actions and reusable workflows
    - 選択した特定のOrganizationでのみGitHub Actionsを有効にします。
3. Disable
    - Enterprise内の全てのOrganizationでGitHub Actionsを無効にします。

有効にした場合、利用可能なActionsを以下のオプションから選択できます:

- Allow all actions and reusable workflows
    - リポジトリ内のアクションや再利用可能なワークフローに関係なく、全てのアクションが許可される場所で利用できます。
- Allow enterprise actions and reusable workflows
    - Enterprise内のリポジトリで定義されたアクションと再利用可能なワークフローのみが使用できます。
- Allow enterprise, and select non-enterprise, actions and reusable workflows
    - Enterprise内のアクションに加え、指定した外部アクションを使用できます。

また、以下のチェックボックスがあります:

- [ ] Require actions to be pinned to a full-length commit SHA
    - チェックを入れると、アクションの指定をコミットハッシュにピン留めすることが必須になります。

#### 現状の設定

1. Enable for all organizations
    - Allow all actions and reusable workflows
    - [ ] Require actions to be pinned to a full-length commit SHA: チェックなし

### Runners

#### 概要

リポジトリレベルのセルフホストランナーを管理する権限をOrganizationに許可するかどうかを選択します。

#### 設定項目

- [ ] Disable for all organizations (チェックボックス)
    - チェックを入れると、Enterprise内の全てのOrganizationでリポジトリレベルのセルフホストランナーが無効になります。

#### 現状の設定

- [ ] Disable for all organizations: チェックなし

### Custom images

#### 概要

Organizationがカスタムイメージを作成してランナーに割り当てられるかどうかを選択します。

#### 設定項目

1. Enable for all organizations
2. Enable for specific organizations
3. Disabled for all organizations

#### 現状の設定

3. Disabled for all organizations

### Artifact and log retention

#### 概要

アーティファクトとログの保持期間のデフォルト設定を選択します。Organizationはより短い保持期間を設定できますが、これより長くすることはできません。

#### 現状の設定

90日

### Approval for running fork pull request workflows from contributors

#### 概要

プルリクエストのワークフロー実行にあたり、承認を必須とするユーザーの範囲を設定します。
承認が必要かどうかの判断は、プルリクエストの作成者と、ワークフローをトリガーしたイベントの実行者の両方に基づいて行われます。
承認が（必須であると）判定された場合、そのワークフローを実行するには、リポジトリへの書き込み権限を持つユーザーによる承認が別途必要になります。

#### 設定項目

1. Require approval for first-time contributors who are new to GitHub:
    - GitHubの新規アカウントで、かつリポジトリへの初めてのコントリビューターであるユーザーに対して、プルリクエストをマージする前に承認が必要になります。
2. Require approval for first-time contributors:
    - リポジトリへの初めてのコントリビューターに対して、プルリクエストをマージする前に承認が必要になります。
3. Require approval for all external contributors:
    - Organization のメンバーまたはコラボレーターではないコントリビューターに対して、ワークフロー実行の承認が必要になります。

#### 現状の設定

2. Require approval for first-time contributors

### Fork pull request workflows in private and internal repositories

#### 概要

プライベートおよびinternalリポジトリでのフォークからのフォークプルリクエストに対して、ワークフローの実行を許可するかどうかを制御する設定です。

#### 設定項目

- [ ] Run workflows from fork pull requests (チェックボックス)
    - フォークプルリクエストに対してワークフローを実行することを許可します。この設定により、フォークが読み取り専用トークンを使用できるようになり、シークレットにアクセスできるようになります。

#### 現状の設定

- [ ] Run workflows from fork pull requests (チェックなし)

### Workflow permissions

#### 概要

ワークフロー実行時のGITHUB_TOKENに付与されるデフォルト権限を設定します。ワークフロー内でより詳細な権限を指定できます。

Organizationおよびリポジトリの管理者は、デフォルト権限をより厳しい設定に変更できます。

#### 設定項目

1. Read and write permissions:
    - ワークフローはリポジトリ内の全てのスコープに対する読み取りと書き込み権限を持ちます。
2. Read repository contents and packages permissions:
    - ワークフローはリポジトリのコンテンツとパッケージに対する読み取り権限のみを持ちます。

さらに、以下の設定でGitHub Actionsがプルリクエストを作成・承認できるかどうかを選択できます:

- [ ] Allow GitHub Actions to create and approve pull requests (チェックボックス)
    - チェックを入れると、GitHub Actionsがプルリクエストを作成したり、プルリクエストのレビューを承認したりできるようになります。

#### 現状の設定

1. Read and write permissions

- [x] Allow GitHub Actions to create and approve pull requests: 有効(チェックあり)

## Projects policies

### Organization projects

#### 概要

Organization projectsは、Enterprise内のメンバーがプロジェクトを作成できるかどうかを制御する設定です。

#### 設定項目

1. No policy
    - Enterpriseレベルでは設定せず、各Organizationが独自にプロジェクト作成のポリシーを設定できます。
2. Enabled
    - Enterprise内の全てのOrganizationでプロジェクト作成を許可します。Organizationのメンバーは権限に基づいてプロジェクトを作成できます。
3. Disabled
    - Enterprise内の全てのOrganizationでプロジェクト作成を無効にします。Organizationのメンバーはプロジェクトを作成できません。

#### 現状の設定

1. No policy

### Project visibility change permission

#### 概要

Project visibility change permissionは、プロジェクトの可視性（public、private）を変更できる権限を制御する設定です。有効にすると、プロジェクト管理者権限を持つメンバーがプロジェクトの公開/非公開を変更できます。無効の場合はOrganizationオーナーのみがこの権限を持ちます。

#### 設定項目

1. No policy
    - Enterpriseレベルでは設定せず、各Organizationが独自にプロジェクト可視性変更のポリシーを設定できます。
2. Enabled
    - プロジェクト管理者権限を持つメンバーがプロジェクトの可視性（public、private）を変更できます。
3. Disabled
    - Organizationのオーナーのみがプロジェクトの可視性を変更でき、プロジェクト管理者権限を持つメンバーでも変更できません。

#### 現状の設定

1. No policy

## Personal access tokens

### Restrict access via fine-grained personal access tokens

#### 概要

fine-grained personal access tokensを使用したアクセスを制限するかどうかを選択します。

#### 設定項目

1. Allow organizations to configure access requirements:
    - Organizationの管理者が、fine-grained personal access tokensを使用したアクセスを制限または許可できるようにします。
2. Restrict access via fine-grained personal access tokens:
    - Organizationのメンバーがfine-grained personal access tokensを使用してOrganizationのリソースにアクセスすることを防止します。Organizationの管理者はこの制限を無効にできません。
3. Allow access via fine-grained personal access tokens:
    - Organizationのメンバーがfine-grained personal access tokensを使用してOrganizationのリソースにアクセスできるようにします。Organizationの管理者はこの設定を上書きできません。

#### 現状の設定

1. Allow organizations to configure access requirements

### Require approval of fine-grained personal access tokens

#### 概要

ユーザーがfine-grained personal access tokensを取得する際に、Organization管理者による承認が必要かどうかを制御する設定です。
デフォルトでは、Organizationの管理者が各personal access tokenの使用を承認する必要があります。

#### 設定項目

1. Allow organizations to configure approval requirements:
    - Organizationの管理者が、fine-grained personal access tokensに対する承認プロセスを有効化または無効化できるようにします。
2. Require organizations to use the approval flow:
    - Enterprise内の全てのOrganizationに対して、メンバーがOrganizationにアクセスするfine-grained personal access tokensを取得する際の承認を必須とします。Organizationの管理者はこの承認フローを無効化できません。
3. Disable the approval flow in all organizations:
    - Enterprise内の全てのOrganizationで承認要件を無効化します。Organizationのメンバーは、正当化や承認なしにOrganizationをターゲットとするfine-grained personal access tokensを作成できますが、Enterprise外のOrganizationをターゲットとすることはできません。

#### 現状の設定

1. Allow organizations to configure approval requirements

### Set maximum lifetimes for personal access tokens

#### 概要

fine-grained personal access tokensがOrganizationへのアクセスを許可される最大有効期限を選択します。

#### 設定項目

- [ ] Fine-grained personal access tokens must expire (チェックボックス)
    - チェックを入れると、fine-grained personal access tokensの最大有効期限を設定できます（日数を指定）。

#### 現状の設定

- [x] Fine-grained personal access tokens must expire: 有効
    - 最大有効期限 : 366日

### Restrict personal access tokens (classic) from accessing your organizations

#### 概要

personal access tokens (classic)のOrganizationsへのアクセスを制限するかどうかを選択します。

#### 設定項目

1. Allow organizations to configure personal access tokens (classic) access requirements:
2. Restrict access via personal access tokens (classic):
3. Allow access via personal access tokens (classic):

#### 現状の設定

1. Allow organizations to configure personal access tokens (classic) access requirements

### Set a maximum lifetime for personal access tokens (classic)

#### 概要

personal access tokens (classic)がOrganizationへのアクセスを許可される最大有効期限を選択します。

#### 設定項目

- [ ] Personal access tokens (classic) must expire (チェックボックス)
    - チェックを入れると、personal access tokens (classic)の最大有効期限を設定できます（日数を指定）。

#### 現状の設定

- [ ] Personal access tokens (classic) must expire: チェックなし
