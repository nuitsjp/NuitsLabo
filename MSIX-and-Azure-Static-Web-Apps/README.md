# MSIX and Azure Static Web Apps

MSIXで作成したインストーラーをAzure Static Web Appsに公開するサンプル

- HelloMsix\HelloMsix\HelloMsix.csproj <- リリースするアプリケーション
- HelloMsix\HelloMsix.Package\HelloMsix.Package.csproj <- MISXプロジェクト
- MSIX-and-Azure-Static-Web-Apps\releaseフォルダにインストーラーを配置
- インストーラーもGithubに配備
- Azure Static Web AppsへのDeployはRelease/MsixAndAzureStaticWebAppsブランチへのプッシュをトリガーに実行

MSIXプロジェクトの作成などは[こちら](https://docs.microsoft.com/ja-jp/windows/apps/desktop/modernize/modernize-wpf-tutorial-5)を参照。

Azure Static Web Appsの初期構築手順は[こちら](https://docs.microsoft.com/ja-jp/azure/static-web-apps/getting-started?tabs=angular)。

少しはまったところ。

- Azure Static Web Appsで利用するフォルダパスに空白は避ける
- スペースが入っているとCIが正しく動作しない
- 解決策はあるかもしれないけど、追跡調査は未実施