// outlookメールの立ち上げ

using Outlook=Microsoft.Office.Interop.Outlook;

var application = new Outlook.Application();
Outlook.MailItem mailItem = application.CreateItem(Outlook.OlItemType.olMailItem);
if (mailItem != null)
{
    // To
    Outlook.Recipient to = mailItem.Recipients.Add("XXX@XXX.co.jp");
    to.Type = (int)Outlook.OlMailRecipientType.olTo;

    // Cc
    Outlook.Recipient cc = mailItem.Recipients.Add("YYY@YYY.co.jp");
    cc.Type = (int)Outlook.OlMailRecipientType.olCC;

    // アドレス帳の表示名で表示できる
    mailItem.Recipients.ResolveAll();
    // 件名
    mailItem.Subject = "件名";
    // 本文
    mailItem.Body = "本文";
    // 表示(Displayメソッド引数のtrue/falseでモーダル/モードレスウィンドウを指定して表示できる)
    mailItem.Display(true);
}