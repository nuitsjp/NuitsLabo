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
    /// テキストボックスに入力されたテキストを、値の反映前に検証します。
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreviewTextInput(TextCompositionEventArgs e)
    {
        base.OnPreviewTextInput(e);

        // e.Textには、入力されたテキストが入っています。
        // 入力されたテキストから、反映後のテキストを予測します。
        var afterText = GetAfterText(e.Text);

        if (!IsInt(afterText))
        {
            // 反映後の値がint型の入力値ではない場合、イベントをキャンセルします。
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
    /// テキストがint型の値かどうかを判定します。
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private bool IsInt(string text)
    {
        // まずint形にパースして確認する。
        if (int.TryParse(text, out var intValue))
        {
            // パースに成功した場合、元の文字列とint型に変換した値が一致するかを確認する。
            // スペースが含まれている場合（など？）は、一致しないのでfalseを返す。
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