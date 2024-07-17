using System;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Outlook.Application outlookApp = new Outlook.Application();
            Outlook.NameSpace outlookNamespace = outlookApp.GetNamespace("MAPI");
            Outlook.MAPIFolder inbox = outlookNamespace.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox);

            foreach (Outlook.MailItem mail in inbox.Items)
            {
                Console.WriteLine("Subject: " + mail.Subject);
            }
        }
    }
}