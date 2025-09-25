# SendFtpTestStudy リファクタリング TODO

## 📋 進捗管理

**開始日**: 2025-01-04  
**最終更新**: 2025-01-04  
**全体進捗**: 4% (3/71項目完了)

---

## 🔴 **フェーズ1: 最優先 (P0) - 基盤の安定化**

### 1. TDD基盤の確立とテスト安定化
- [x] 現在のテスト実行状況を確認 (`dotnet test`) ✅ 1件のテスト成功
- [x] 失敗テストがある場合は原因を特定・記録 ✅ 失敗テストなし
- [x] 全テストが安定してPASSすることを確認 ✅ 成功率100%
- [ ] CI/CD用のテスト実行スクリプト作成
- **見積工数**: 1-2時間
- **実績工数**: 0.5時間
- **担当者**: GitHub Copilot CLI
- **完了日**: 2025-01-04 (75%完了)

### 2. 設定検証の強化
- [ ] `IValidateOptions<FtpClientOptions>` インターフェイス実装
- [ ] `FtpClientOptionsValidator` クラス作成
- [ ] Host必須チェックのテスト作成 (`FtpClientProvider_CreateAsync_WhenHostMissing_Throws`)
- [ ] Port範囲チェックのテスト作成 (`FtpClientProvider_CreateAsync_WhenPortInvalid_Throws`)
- [ ] User必須チェックのテスト作成 (`FtpClientProvider_CreateAsync_WhenUserMissing_Throws`)
- [ ] Password必須チェックのテスト作成 (`FtpClientProvider_CreateAsync_WhenPasswordMissing_Throws`)
- [ ] バリデーション実装（RED → GREEN → REFACTOR）
- [ ] `ServiceCollectionExtensions` にバリデータ登録
- [ ] 統合テストでバリデーション動作確認
- **見積工数**: 4-6時間
- **担当者**: 
- **完了日**: 

### 3. エラーハンドリングの改善
- [ ] カスタム例外クラス設計・作成 (`FtpClientException`, `FtpConnectionException`)
- [ ] `UploadAsync` 引数検証テスト作成 (`UploadAsync_RemotePathEmpty_ThrowsArgumentException`)
- [ ] `UploadAsync` null引数テスト作成 (`UploadAsync_ContentNull_ThrowsArgumentNullException`)
- [ ] FluentFTP例外ラップテスト作成 (`UploadAsync_FtpException_ThrowsCustomException`)
- [ ] `ILogger<FtpClient>` 注入対応
- [ ] 構造化ログ実装 (接続情報、エラー詳細)
- [ ] ログテスト作成 (`UploadAsync_Failure_LogsError`)
- [ ] エラーハンドリング実装（RED → GREEN → REFACTOR）
- **見積工数**: 6-8時間
- **担当者**: 
- **完了日**: 

**フェーズ1 進捗**: 0% (0/20項目完了)

---

## 🟡 **フェーズ2: 高優先 (P1) - アーキテクチャ改善**

### 4. SOLID原則の徹底（DIP）
- [ ] `IAsyncFtpClientFactory` インターフェイス設計・作成
- [ ] `AsyncFtpClientFactory` 実装クラス作成
- [ ] `FtpClientProvider` のFactory依存注入対応
- [ ] Mock用テスト作成 (`FtpClientProvider_CreateAsync_WhenConnectFails_ThrowsCustomException`)
- [ ] DI登録の更新 (`ServiceCollectionExtensions`)
- [ ] 既存テストの修正・実行確認
- **見積工数**: 8-10時間
- **担当者**: 
- **完了日**: 

### 5. 非同期処理の最適化
- [ ] `FtpClientOptions` に再試行設定追加 (`RetryCount`, `RetryInterval`)
- [ ] タイムアウト設定追加 (`ConnectionTimeout`, `DataTimeout`)
- [ ] Pollyライブラリ導入検討・実装
- [ ] 再試行テスト作成 (`CreateAsync_RetriesOnTransientFailure`)
- [ ] タイムアウトテスト作成 (`CreateAsync_TimesOutOnSlowConnection`)
- [ ] `ConfigureAwait(false)` の一貫性確認
- **見積工数**: 6-8時間
- **担当者**: 
- **完了日**: 

### 6. サービス登録の柔軟性向上
- [ ] `AddFtpClientProvider` オーバーロード設計
- [ ] `Action<FtpClientOptions>` オーバーロード実装
- [ ] デフォルト設定提供機能
- [ ] 設定エラー早期検出テスト (`AddFtpClientProvider_WhenSectionMissing_FailsFast`)
- [ ] 複数設定パターンのテスト作成
- [ ] ドキュメント更新
- **見積工数**: 4-6時間
- **担当者**: 
- **完了日**: 

**フェーズ2 進捗**: 0% (0/18項目完了)

---

## 🟢 **フェーズ3: 中期 (P2) - 機能拡張と最適化**

### 7. 統合テストの拡充
- [ ] アップロード失敗シナリオテスト (`FtpClient_UploadAsync_NetworkFailure`)
- [ ] 再接続テスト (`FtpClient_UploadAsync_ReconnectsOnDisconnect`)
- [ ] キャンセレーションテスト (`FtpClient_UploadAsync_CancellationToken`)
- [ ] 大容量ファイルテスト (`FtpClient_UploadAsync_LargeFile`)
- [ ] 並列アップロードテスト (`FtpClient_UploadAsync_Parallel`)
- [ ] `FtpServer` フィクスチャクリーンアップ強化
- [ ] テスト並列実行対応 (`FtpServer_DisposeAsync_RemovesTempDirectory`)
- **見積工数**: 8-12時間
- **担当者**: 
- **完了日**: 

### 8. 機能拡張のための設計改善
- [ ] `IFtpClient` インターフェイス拡張設計
- [ ] ディレクトリ作成メソッド (`CreateDirectoryAsync`)
- [ ] ディレクトリ削除メソッド (`DeleteDirectoryAsync`)
- [ ] ファイル削除メソッド (`DeleteFileAsync`)
- [ ] Commandパターン検討・設計
- [ ] 新機能テスト作成 (`CreateDirectoryAsync_WhenMissing_Creates`)
- [ ] インターフェイス実装・既存テスト確認
- **見積工数**: 12-16時間
- **担当者**: 
- **完了日**: 

### 9. 設定のホットリロード対応
- [ ] `IOptionsMonitor<FtpClientOptions>` 対応設計
- [ ] `FtpClientProvider` の動的設定変更対応
- [ ] 設定変更テスト (`FtpClientProvider_ReflectsUpdatedOptions`)
- [ ] 設定変更時の既存接続処理検討
- [ ] ホットリロード実装・テスト
- **見積工数**: 6-8時間
- **担当者**: 
- **完了日**: 

**フェーズ3 進捗**: 0% (0/19項目完了)

---

## 🔵 **フェーズ4: 長期 (P3) - パフォーマンスと運用改善**

### 10. 接続プールの実装
- [ ] `IFtpClientPool` インターフェイス設計
- [ ] `FtpClientPool` 実装クラス作成
- [ ] 接続プール設定 (`PoolSize`, `MaxLifetime`)
- [ ] プールテスト作成 (`FtpClientPool_BorrowsAndReturnsClients`)
- [ ] 並列アップロード対応テスト
- [ ] `System.Threading.Channels` 活用検討
- [ ] リソース管理・リーク防止実装
- [ ] メモリ使用量測定・最適化
- **見積工数**: 16-20時間
- **担当者**: 
- **完了日**: 

### 11. 監視・メトリクス機能
- [ ] `IMetrics` インターフェイス設計
- [ ] メトリクス収集実装（成功/失敗回数、レスポンス時間）
- [ ] ヘルスチェック機能 (`IHealthCheck`)
- [ ] メトリクステスト (`UploadAsync_EmitsMetricOnSuccess`)
- [ ] ヘルスチェックテスト
- [ ] 監視ダッシュボード用データ出力
- **見積工数**: 8-12時間
- **担当者**: 
- **完了日**: 

**フェーズ4 進捗**: 0% (0/14項目完了)

---

## 📊 **全体サマリー**

| フェーズ | 総項目数 | 完了項目数 | 進捗率 | 見積工数 |
|---------|---------|-----------|-------|---------|
| P0 (最優先) | 20 | 0 | 0% | 11-16時間 |
| P1 (高優先) | 18 | 0 | 0% | 18-24時間 |
| P2 (中期) | 19 | 0 | 0% | 26-36時間 |
| P3 (長期) | 14 | 0 | 0% | 24-32時間 |
| **合計** | **71** | **0** | **0%** | **79-108時間** |

---

## 📝 **作業ログ**

### 2025-01-04
- [x] TODO.md作成 ✅
- [x] 初期リファクタリングプラン策定完了 ✅  
- [x] P0-1 TDD基盤確立 (75%完了) ✅ テスト実行確認、全テストPASS
- [ ] 実際の工数: 1時間
- [ ] 発見した課題: 現在1件のテストのみ、テストカバレッジ拡大が必要
- [ ] 次回の予定: P0-2 設定検証の強化開始

### 作業開始時の記録テンプレート
```
### YYYY-MM-DD
- [ ] 作業項目名
- [ ] 実際の工数: X時間
- [ ] 発見した課題:
- [ ] 次回の予定:
```

---

## 🎯 **次のアクション**

1. **P0-1**: TDD基盤の確立 - `dotnet test` の実行確認
2. **P0-2**: 設定検証の強化 - `IValidateOptions<FtpClientOptions>` の実装
3. **P0-3**: エラーハンドリング改善 - カスタム例外クラスの作成

**重要**: t_wada式TDDに従い、必ず **RED → GREEN → REFACTOR** の順で進めること

---

## 📚 **参考資料**

- [プロジェクトAGENTS.md](./AGENTS.md)
- [.NET Testing Best Practices](https://docs.microsoft.com/en-us/dotnet/core/testing/best-practices)
- [FluentFTP Documentation](https://github.com/robinrodricks/FluentFTP)
- [Polly Resilience Framework](https://github.com/App-vNext/Polly)