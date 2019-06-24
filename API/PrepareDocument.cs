﻿using System;
using System.Collections.Generic;
using System.Text;
using Word = Microsoft.Office.Interop.Word;
using System.Reflection;
using System.Windows;
using PrintSelected.BLL;
using System.Threading;
using System.Runtime.InteropServices;
using NLog;

namespace PrintSelected.API
{
    class PrepareDocument
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();
        public List<string> DocumentIds { get; set; }

        readonly ParodontRecommendationRepository repo = new ParodontRecommendationRepository();
        public string CreateDocumentWord(string pacientName)
        {
            var res = "";
            try
            {
                //Create an instance for word app  
                Word.Application winword = new Word.Application {
                    ShowAnimation = false,
                    Visible = false
                };

                //Create a missing variable for missing value  
                object missing = Missing.Value;

                //Create a new document  
                Word.Document document = winword.Documents.Add(ref missing, ref missing, ref missing, ref missing);

                //Add header into the document  
                foreach (Word.Section section in document.Sections)
                {
                    //Get the header range and add the header details.  
                    Word.Range headerRange = section.Headers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                    headerRange.Fields.Add(headerRange, Word.WdFieldType.wdFieldPage);
                    headerRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                    object styleHeading1 = Word.WdBuiltinStyle.wdStyleHeading1;
                    headerRange.set_Style(ref styleHeading1);
                    headerRange.Bold = 5;
                    headerRange.Underline =Word.WdUnderline.wdUnderlineSingle;
                    headerRange.Text = "Уважаемый(-ая) " + pacientName;
                }

                //Add the footers into the document  
                foreach (Word.Section wordSection in document.Sections)
                {
                    //Get the footer range and add the footer details.  
                    Word.Range footerRange = wordSection.Footers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                    footerRange.Font.ColorIndex = Word.WdColorIndex.wdDarkRed;
                    footerRange.Font.Size = 12;
                    footerRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    footerRange.Text = "Рекоммендации выдал доктор: Рыбченко Надежда Николаевна";
                }

                //Add paragraph with Heading 1 style Закомментировано для примера использования стилей  
                //Word.Paragraph para1 = document.Content.Paragraphs.Add(ref missing);
                //object styleHeading1 = Word.WdBuiltinStyle.wdStyleHeading1;
                //para1.Range.set_Style(ref styleHeading1);
                //para1.Range.Text = "Para 1 text";
                //para1.Range.InsertParagraphAfter();


                //adding text to document  
                document.Content.SetRange(0, 0);
                document.Content.Text = PrepareRecomendations();

                //Save the document  

                var dir = GetDocumentDirectory();

                object file = System.IO.Path.Combine(dir + "\\document", pacientName.Trim() + ".docx");
                res = file.ToString();

                document.SaveAs2(ref file);
                document.Close(ref missing, ref missing, ref missing);
                document = null;
                winword.Quit(ref missing, ref missing, ref missing);
                winword = null;
                MessageBox.Show("Документ сохранен успешно!");
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                MessageBox.Show("Произошла ошибка при сохранении документа. Обратитесь в техническую поддержку.");
                
            }

            return res;
        }

        private string PrepareRecomendations()
        {
            var res = "";
            try
            {
                StringBuilder finalText = new StringBuilder();
                finalText.Append(Environment.NewLine);
                foreach (var id in this.DocumentIds)
                {
                    var textId = Guid.Parse(id);
                    finalText.Append(repo.GetById(textId).Text + Environment.NewLine);
                }
                res = finalText.ToString();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _log.Error("Ошибка подготовки текста. {0}", ex.Message);
            }
            catch (Exception ex) {
                _log.Error("Ошибка подготовки текста. {0}", ex.Message);
            }
            return res;
        }

        static private string GetDocumentDirectory()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            path = System.IO.Path.GetDirectoryName(path);
            return path;
        }

        public void PrintDocument(string file)
        {

            object nullobj = Missing.Value;

            var wordApp = new Word.Application
            {
                DisplayAlerts = Word.WdAlertLevel.wdAlertsNone,
                Visible = false
            };

            Word.Document doc = null;
            Word.Documents docs = null;
            Word.Dialog dialog = null;

            if (!String.IsNullOrEmpty(file))
            {
                try
                {
                    docs = wordApp.Documents;
                    doc = docs.Open(file);

                    doc.Activate();
                    dialog = wordApp.Dialogs[Word.WdWordDialog.wdDialogFilePrint];
                    var dialogResult = dialog.Show(ref nullobj);
                    if (dialogResult == 1)
                    {
                        doc.PrintOut(false);
                    }
                }
                catch (Exception ex)
                {
                    _log.Error("Ошибка печати {0}", ex.Message);
                    MessageBox.Show("Не могу напечатать документ");

                }
                finally
                {
                    Thread.Sleep(3000);
                    if (dialog != null) Marshal.FinalReleaseComObject(dialog);
                    if (doc != null) Marshal.FinalReleaseComObject(doc);
                    if (docs != null) Marshal.FinalReleaseComObject(docs);

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    doc = null;
                    wordApp.Quit(false, ref nullobj, ref nullobj);
                }
            }
            else { MessageBox.Show("Не найден файл для печати"); }

        }

    }
}
