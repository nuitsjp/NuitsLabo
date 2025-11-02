// See https://aka.ms/new-console-template for more information
using Xfinium.Pdf;

PdfFixedDocument document = new PdfFixedDocument();
PdfPage page = document.Pages.Add();
page.Rotation = 90;