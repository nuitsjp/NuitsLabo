using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfNumericTextBoxStudy;

/// <summary>
/// int型のみを入力可能とするテキストボックス
/// </summary>
public class IntTextBox : TextBox
{
    /// <summary>
    /// コンストラクター
    /// </summary>
    public IntTextBox()
    {
        // IMEを無効にする
        InputMethod.SetIsInputMethodEnabled(this, false);

        // ペーストイベントをハンドリング
        DataObject.AddPastingHandler(this, OnPaste);
    }

    /// <summary>
    /// 入力されたキーによって、テキストボックスの値を変更するかどうかを判定します。
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreviewKeyDown(KeyEventArgs e)
    {
        base.OnPreviewKeyDown(e);

        if (IsCursorMovementKey(e.Key) || IsEditingOperationKey(e.Key))
        {
            // カーソル移動系キー、編集操作系キーは処理せずにリターン
            return;
        }

        // 数値キー、マイナスキー以外のキーが押された場合、押されたキーに対応する文字を取得します。
        if (TryGetNumericChar(e.Key, out var keyChar))
        {
            // 数値キー、マイナスキーが押された場合
            // 入力されたキーから、反映後のテキストを予測します。
            var afterText = GetAfterText(keyChar.ToString());
            if (!IsInt(afterText))
            {
                // 反映後の値がint型の入力値ではない場合、イベントをキャンセルします。
                e.Handled = true;
            }
        }
        else
        {
            // 数値キー、マイナスキー以外のキーが押された場合、イベントをキャンセルします。
            e.Handled = true;
        }
    }

    /// <summary>
    /// ペーストされたテキストを、値の反映前に検証します。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnPaste(object sender, DataObjectPastingEventArgs e)
    {
        if (e.DataObject.GetDataPresent(typeof(string)))
        {
            // ペーストされたデータが文字列として取得できる場合
            // ペーストされたテキストを取得します。
            var text = (string?)e.DataObject.GetData(typeof(string)) ?? string.Empty;

            // ペーストされたテキストから、反映後のテキストを予測します。
            var afterText = GetAfterText(text);

            if (!IsInt(afterText))
            {
                // 反映後の値がint型の入力値ではない場合、イベントをキャンセルします。
                e.CancelCommand();
            }
        }
        else
        {
            // ペーストされたデータが文字列として取得できない場合、イベントをキャンセルします。
            e.CancelCommand();
        }
    }

    /// <summary>
    /// カーソル移動系キーかどうかを判定します。
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    private bool IsCursorMovementKey(Key key)
    {
        return key == Key.Left || key == Key.Right || key == Key.Up || key == Key.Down ||
               key == Key.Home || key == Key.End || key == Key.PageUp || key == Key.PageDown ||
               key == Key.Tab;
    }

    /// <summary>
    /// 編集操作系キーかどうかを判定します。
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    private bool IsEditingOperationKey(Key key)
    {
        return key == Key.Delete || key == Key.Back;
    }

    private bool TryGetNumericChar(Key key, out char keyChar)
    {
        switch (key)
        {
            // 数値キーの場合
            case >= Key.D0 and <= Key.D9:
                // キーコードから対応する数字を取得
                keyChar = (char)('0' + (int)key - (int)Key.D0);
                return true;
            case >= Key.NumPad0 and <= Key.NumPad9:
                // テンキーの場合、キーコードから対応する数字を取得
                keyChar = (char)('0' + (int)key - (int)Key.NumPad0);
                return true;
            default:
                // キーが特殊文字である場合
                switch (key)
                {
                    case Key.OemMinus:
                    case Key.Subtract:
                        keyChar = '-';
                        return true;
                    default:
                        // 数値またはマイナス以外の場合はkeyCharをデフォルトのchar.MinValueのままにしてfalseを返す
                        keyChar = char.MinValue;
                        return false;
                }
        }
    }

    /// <summary>
    /// テキストがint型の値かどうかを判定します。
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private bool IsInt(string text)
    {
        if ("-".Equals(text))
        {
            // マイナス記号のみの場合はtrueを返す
            return true;
        }

        // まずint形にパースして確認する。
        if (int.TryParse(text, out var intValue))
        {
            // 01などを除外するために、パースした値を文字列に変換して、入力値と一致するかどうかを確認する。
            return string.Equals(text, intValue.ToString());
        }
        return false;
    }

    /// <summary>
    /// ペースト後のテキストを予測します。
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private string GetAfterText(string text)
    {
        // ペースト後のテキストを予測（例えば、数値以外の文字が含まれる場合はペーストをキャンセル）
        return Text.Substring(0, SelectionStart)
               + text
               + Text.Substring(SelectionStart + SelectionLength);
    }
}