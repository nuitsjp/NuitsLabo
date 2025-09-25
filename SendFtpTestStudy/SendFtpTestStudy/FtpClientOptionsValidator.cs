using Microsoft.Extensions.Options;

namespace SendFtpTestStudy;

/// <summary>
/// FtpClientOptions設定値のバリデーションを実装するクラス
/// IValidateOptions&lt;T&gt;インターフェイスを実装してOptions pattern準拠の検証を提供
/// </summary>
public sealed class FtpClientOptionsValidator : IValidateOptions<FtpClientOptions>
{
    /// <summary>
    /// FtpClientOptionsの設定値を検証する
    /// Microsoft.Extensions.Options.OptionsValidationExceptionが発生する条件を定義
    /// </summary>
    /// <param name="name">設定名（通常はnull）</param>
    /// <param name="options">検証対象のFtpClientOptionsインスタンス</param>
    /// <returns>検証結果。失敗時はエラーメッセージを含む</returns>
    public ValidateOptionsResult Validate(string? name, FtpClientOptions options)
    {
        var errors = new List<string>();

        // Host必須チェック
        if (string.IsNullOrWhiteSpace(options.Host))
        {
            errors.Add("Host is required and cannot be empty.");
        }

        // Port範囲チェック（1-65535）
        if (options.Port is <= 0 or > 65535)
        {
            errors.Add("Port must be between 1 and 65535.");
        }

        // User必須チェック
        if (string.IsNullOrWhiteSpace(options.User))
        {
            errors.Add("User is required and cannot be empty.");
        }

        // Password必須チェック
        if (string.IsNullOrWhiteSpace(options.Password))
        {
            errors.Add("Password is required and cannot be empty.");
        }

        // エラーがある場合は失敗結果を返す
        if (errors.Count > 0)
        {
            return ValidateOptionsResult.Fail(errors);
        }

        // エラーがない場合は成功結果を返す
        return ValidateOptionsResult.Success;
    }
}