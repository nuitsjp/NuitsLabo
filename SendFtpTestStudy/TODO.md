# SendFtpTestStudy リファクタリング TODO

## 📋 進捗管理

**開始日**: 2025-01-04  
**最終更新**: 2025-01-04  
**全体進捗**: 52% (14/27項目完了)

---

## 🔴 **フェーズ1: 最優先 (P0) - 基盤の安定化**

### 1. TDD基盤の確立とテスト安定化
- [x] 現在のテスト実行状況を確認 (`dotnet test`) ✅ 1件のテスト成功
- [x] 失敗テストがある場合は原因を特定・記録 ✅ 失敗テストなし
- [x] 全テストが安定してPASSすることを確認 ✅ 成功率100%
- **見積工数**: 1-2時間
- **実績工数**: 0.5時間
- **担当者**: GitHub Copilot CLI
- **完了日**: 2025-01-04 (100%完了)

### 2. 設定検証の強化
- [x] `IValidateOptions<FtpClientOptions>` インターフェイス実装 ✅
- [x] `FtpClientOptionsValidator` クラス作成 ✅
- [x] Host必須チェックのテスト作成 (`FtpClientProvider_CreateAsync_WhenHostMissing_Throws`) ✅
- [x] Port範囲チェックのテスト作成 (`FtpClientProvider_CreateAsync_WhenPortInvalid_Throws`) ✅
- [x] User必須チェックのテスト作成 (`FtpClientProvider_CreateAsync_WhenUserMissing_Throws`) ✅
- [x] Password必須チェックのテスト作成 (`FtpClientProvider_CreateAsync_WhenPasswordMissing_Throws`) ✅
- [x] バリデーション実装（RED → GREEN → REFACTOR） ✅
- [x] `ServiceCollectionExtensions` にバリデータ登録 ✅
- [x] 統合テストでバリデーション動作確認 ✅
- **見積工数**: 4-6時間
- **実績工数**: 2時間
- **担当者**: GitHub Copilot CLI
- **完了日**: 2025-01-04 (100%完了)

**フェーズ1 進捗**: 100% (14/14項目完了)

---

## 🟡 **フェーズ2: 高優先 (P1) - 非同期処理最適化**

### 3. 非同期処理の最適化
- [ ] `FtpClientOptions` に再試行設定追加 (`RetryCount`, `RetryInterval`)
- [ ] タイムアウト設定追加 (`ConnectionTimeout`, `DataTimeout`)
- [ ] Pollyライブラリ導入検討・実装
- [ ] 再試行テスト作成 (`CreateAsync_RetriesOnTransientFailure`)
- [ ] タイムアウトテスト作成 (`CreateAsync_TimesOutOnSlowConnection`)
- [ ] `ConfigureAwait(false)` の一貫性確認
- **見積工数**: 6-8時間
- **担当者**: 
- **完了日**: 

**フェーズ2 進捗**: 0% (0/6項目完了)

---

## 🟢 **フェーズ3: 中期 (P2) - テスト拡充**

### 4. 統合テストの拡充
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

**フェーズ3 進捗**: 0% (0/7項目完了)

---

## 📊 **全体サマリー**

| フェーズ | 総項目数 | 完了項目数 | 進捗率 | 見積工数 |
|---------|---------|-----------|-------|---------|
| P0 (最優先) | 14 | 14 | 100% | 5-8時間 |
| P1 (高優先) | 6 | 0 | 0% | 6-8時間 |
| P2 (中期) | 7 | 0 | 0% | 8-12時間 |
| **合計** | **27** | **14** | **52%** | **19-28時間** |

---

## ❌ **除外された項目 (要件に基づく)**

### 除外理由と項目

#### 1. エラーハンドリング改善 (除外)
**理由**: ローレベル通信ライブラリとして例外は上位にスローする方針
- カスタム例外クラス作成
- 例外ラップ処理  
- ロギング機能追加

#### 2. SOLID原則の徹底(DIP) (除外)
**理由**: `FtpClientProvider`が既に適切に`AsyncFtpClient`への依存を隠蔽
- `IAsyncFtpClientFactory` 導入
- Factoryパターン実装

#### 3. サービス登録の柔軟性向上 (除外)
**理由**: 破棄指示により除外
- `AddFtpClientProvider` オーバーロード
- 設定エラー早期検出

#### 4. 機能拡張のための設計改善 (除外)
**理由**: 現在の必要機能 (ファイルアップロード) は満たされている
- ディレクトリ操作メソッド追加
- Commandパターン導入

#### 5. 設定のホットリロード対応 (除外)
**理由**: 利用しない方針
- `IOptionsMonitor` 対応
- 動的設定変更

#### 6. フェーズ4全体 (除外)
**理由**: パフォーマンス最適化・監視機能は不要
- 接続プール実装
- メトリクス・監視機能

---

## 📝 **作業ログ**

### 2025-01-04
- [x] TODO.md作成 ✅
- [x] 初期リファクタリングプラン策定完了 ✅  
- [x] P0-1 TDD基盤確立 (75%完了) ✅ テスト実行確認、全テストPASS
- [x] 要件に基づくTODO見直し完了 ✅ 71項目→27項目に絞り込み
- [x] P0-2 設定検証の強化完了 ✅ TDD（RED→GREEN→REFACTOR）サイクルで実装
- **実際の工数**: 3.5時間（P0-1: 1.5時間 + P0-2: 2時間）
- **発見した課題**: P0-1で現在1件のテストのみ→P0-2で6テストに拡大、バリデーション強化完了
- **次回の予定**: P1-3 非同期処理の最適化開始

---

## 🎯 **次のアクション**

1. **P1-3**: 非同期処理の最適化 - 再試行・タイムアウト設定  
2. **P2-4**: 統合テストの拡充 - エラーケース・パフォーマンステスト

**重要**: t_wada式TDDに従い、必ず **RED → GREEN → REFACTOR** の順で進めること

---

## 📚 **参考資料**

- [プロジェクトAGENTS.md](./AGENTS.md)
- [.NET Testing Best Practices](https://docs.microsoft.com/en-us/dotnet/core/testing/best-practices)
- [FluentFTP Documentation](https://github.com/robinrodricks/FluentFTP)  
- [IValidateOptions Documentation](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.options.ivalidateoptions-1)