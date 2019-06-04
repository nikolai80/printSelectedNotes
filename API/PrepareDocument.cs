using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;
using System.Reflection;
using System.Windows;
using PrintSelected.BLL;

namespace PrintSelected.API
{
    class PrepareDocument
    {
        
        public List<string> DocumentIds { get; set; }
        ParodontRecommendationRepository repo = new ParodontRecommendationRepository();
        public void CreateDocumentWord(string pacientName)
        {
            string filename;
            StringBuilder finalText = new StringBuilder();
            try
            {
                //Create an instance for word app  
                Word.Application winword = new Word.Application();

                //Set animation status for word application  
                winword.ShowAnimation = false;

                //Set status for word application is to be visible or not.  
                winword.Visible = false;

                //Create a missing variable for missing value  
                object missing = System.Reflection.Missing.Value;

                //Create a new document  
                Word.Document document = winword.Documents.Add(ref missing, ref missing, ref missing, ref missing);

                //Add header into the document  
                foreach (Word.Section section in document.Sections)
                {
                    //Get the header range and add the header details.  
                    Word.Range headerRange = section.Headers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                    headerRange.Fields.Add(headerRange, Word.WdFieldType.wdFieldPage);
                    headerRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    object styleHeading1 = Word.WdBuiltinStyle.wdStyleHeading1;
                    headerRange.set_Style(ref styleHeading1);
                    headerRange.Text = "Уважаемый(-ая) "+pacientName;
                }

                //Add the footers into the document  
                foreach (Word.Section wordSection in document.Sections)
                {
                    //Get the footer range and add the footer details.  
                    Word.Range footerRange = wordSection.Footers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                    footerRange.Font.ColorIndex = Word.WdColorIndex.wdDarkRed;
                    footerRange.Font.Size = 10;
                    footerRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    footerRange.Text = "Доктор Рыбченко Надежда Николаевна";
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

                object file = System.IO.Path.Combine(dir + "\\document", "temp1.docx");


                document.SaveAs2(ref file);
                document.Close(ref missing, ref missing, ref missing);
                document = null;
                winword.Quit(ref missing, ref missing, ref missing);
                winword = null;
                MessageBox.Show("Документ сохранен успешно!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string PrepareRecomendations() {
            string res = "";
            StringBuilder finalText = new StringBuilder();
            foreach (var id in this.DocumentIds)
            {
                var textId = Guid.Parse(id);
                finalText.Append(repo.GetById(textId).Text + Environment.NewLine);
            }
            res = finalText.ToString();
            return res;
        }

        static private string GetDocumentDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            path = System.IO.Path.GetDirectoryName(path);
            return path;
        }

    }
}
