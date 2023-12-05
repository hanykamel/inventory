
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace inventory.Engines.WordGenerator
{
    public class WordGenerator : IWordGenerator
    {
        #region properties
        public string templateURL { get; set; }

        public WordprocessingDocument Template { get; private set; }
        #endregion




        public MemoryStream PrintDocument(string TemplateName, ArrayList tabledata
            , Dictionary<string, string> contentReplacers
            , Dictionary<string, string> footerReplacers
            , Dictionary<string, string> headerReplacers
            , Dictionary<string, string> repetitiveReplacers
            , List<Dictionary<string, string>> repetitiveSectionReplacer = null
            , bool fullTable = true,
            int numberOfHeaderRows =1)
        {
            this.templateURL = Path.Combine(Directory.GetCurrentDirectory(), "WordDocuments", TemplateName);
            IEnumerable<IdPartPair> parts;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (WordprocessingDocument temopWordDocument = WordprocessingDocument.
                    Create(memoryStream, WordprocessingDocumentType.Document, true))
                {
                    Template = GetDocument();
                    if (tabledata != null)
                    {
                        FillTable(tabledata, fullTable, numberOfHeaderRows);
                    }
                    if (contentReplacers != null)
                    {
                        ReplaceContent(contentReplacers);
                    }
                    if (footerReplacers != null)
                    {
                        RecplaceFooter(footerReplacers);
                    }
                    if (headerReplacers != null)
                    {
                        RecplaceHeader(headerReplacers);
                    }
                    if (repetitiveReplacers != null)
                    {
                        ReplaceRepetitive(repetitiveReplacers);
                    }
                    if (repetitiveSectionReplacer != null)
                    {
                        ReplaceSectionContent(repetitiveSectionReplacer);
                    }
                    parts = Template.Parts.ToList();

                    Template.Save();

                    foreach (var part in parts)
                        temopWordDocument.AddPart(part.OpenXmlPart, part.RelationshipId);
                }
                return memoryStream;
            }
        }

        public MemoryStream PrintMultiplePageDocument(string TemplateName, List<ArrayList> tabledata
            , List<Dictionary<string, string>> contentReplacers
            , List<Dictionary<string, string>> footerReplacers
            , List<Dictionary<string, string>> headerReplacers
            , List<List<ArrayList>> tableReplacers
            , List<Dictionary<string, string>> repetitiveReplacers, bool fullTable,int numberOfHeaderRows)
        {
            this.templateURL = Path.Combine(Directory.GetCurrentDirectory(), "WordDocuments", TemplateName);
            IEnumerable<IdPartPair> parts;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                var sources = new List<OpenXmlPowerTools.Source>();
                for (int i = 0; i < contentReplacers.Count; i++)
                {
                    using (WordprocessingDocument temopWordDocument = WordprocessingDocument.
                        Create(memoryStream, WordprocessingDocumentType.Document, true))
                    {
                        Template = GetDocument();
                        if (tabledata != null)
                        {
                            FillTable(tabledata[i], fullTable, numberOfHeaderRows);
                        }
                        if (tableReplacers != null)
                        {
                            FillMultiTable(tableReplacers[i], fullTable, numberOfHeaderRows);
                        }
                        if (contentReplacers != null)
                        {
                            ReplaceContent(contentReplacers[i]);
                        }
                        if (footerReplacers != null)
                        {
                            RecplaceFooter(footerReplacers[i]);
                        }
                        if (headerReplacers != null)
                        {
                            RecplaceHeader(headerReplacers[headerReplacers.Count == 1?0:i]);
                        }
                        if (repetitiveReplacers != null)
                        {
                            ReplaceRepetitive(repetitiveReplacers[i]);
                        }

                        Template.Save();
                        //if (i==0)
                        //{
                        parts = Template.Parts.ToList();
                        foreach (var part in parts)
                        {
                            temopWordDocument.AddPart(part.OpenXmlPart, part.RelationshipId);
                        }
                    }
                    sources.Add(new OpenXmlPowerTools.Source(new OpenXmlPowerTools.WmlDocument("", memoryStream), true));
                }
                var mergedDoc = OpenXmlPowerTools.DocumentBuilder.BuildDocument(sources);
                MemoryStream memorystream2 = new MemoryStream(mergedDoc.DocumentByteArray);
                return memorystream2;
            }
        }
        public MemoryStream PrintSectionRepeaterDocument(string TemplateName, List<ArrayList> sectionReplacer = null
            , List<Dictionary<string, string>> contentRepeaterReplacers = null
            , Dictionary<string, string> contentReplacers = null
            , Dictionary<string, string> footerReplacers = null
            , Dictionary<string, string> headerReplacers = null
            , Dictionary<string, string> repetitiveReplacers = null
            , List<Dictionary<string, string>> repetitiveSectionReplacer = null
            , int tableIndex = 0
            , int sectionIndex = 0
            , bool startNewPage = false)
        {
            this.templateURL = Path.Combine(Directory.GetCurrentDirectory(), "WordDocuments", TemplateName);
            IEnumerable<IdPartPair> parts;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (WordprocessingDocument temopWordDocument = WordprocessingDocument.
                    Create(memoryStream, WordprocessingDocumentType.Document, true))
                {
                    Template = GetDocument();
                    if (contentReplacers != null)
                    {
                        ReplaceContent(contentReplacers);
                    }
                    if (sectionReplacer != null || contentRepeaterReplacers != null)
                    {
                        SectionRepeaterCreator(sectionReplacer, contentRepeaterReplacers, sectionIndex, tableIndex, startNewPage);
                    }
                    if (repetitiveSectionReplacer != null)
                    {
                        ReplaceSectionContent(repetitiveSectionReplacer);
                    }
                    if (footerReplacers != null)
                    {
                        RecplaceFooter(footerReplacers);
                    }
                    if (headerReplacers != null)
                    {
                        RecplaceHeader(headerReplacers);
                    }
                    if (repetitiveReplacers != null)
                    {
                        ReplaceRepetitive(repetitiveReplacers);
                    }
                    parts = Template.Parts.ToList();

                    Template.Save();

                    foreach (var part in parts)
                        temopWordDocument.AddPart(part.OpenXmlPart, part.RelationshipId);
                }
                return memoryStream;
            }
        }
        #region Functions
        private WordprocessingDocument GetDocument()
        {
            return WordprocessingDocument.CreateFromTemplate(templateURL, false);
        }

        //Function used to replace the table contents
        //the provided table in the template must have 3 rows beside the header row
        private void FillTable(ArrayList data, bool fullTable, int numberOfHeaderRows)
        {
            MainDocumentPart mainPart = Template.MainDocumentPart;
            Body bod = mainPart.Document.Body;
            //choose the first table
            Table table = bod.Descendants<Table>().FirstOrDefault();
            var tableLength = table.Descendants<TableRow>().ToList();

            for (int j = 0; j < data.Count; j++)
            {
                var item = data[j];
                TableRow tr = null;
                if (data.Count == tableLength.Count && data.Count == j + 1)
                {
                    //clone the last row to keep the bottom border of the table
                    tr = table.Descendants<TableRow>().ToList()[tableLength.Count - 1].CloneNode(true) as TableRow;
                    //table.Descendants<TableRow>().ToList()[table.Descendants<TableRow>().ToList().Count - 1].Remove();
                }
                else
                {
                    //clone the third row(seconed row if we dont count the header) to keep the table inner format
                    tr = table.Descendants<TableRow>().ToList()[2].CloneNode(true) as TableRow;
                }
                string[] props = item.GetType().GetProperties().Select(p => p.Name).ToArray();
                for (int i = 0; i < props.Length; i++)
                {
                    //removes that text of the copied cell
                    tr.Descendants<TableCell>().ElementAt(i).RemoveAllChildren<Paragraph>();
                    //append a new text tag to the current cell
                    if (item.GetType().GetProperty(props[i]).GetValue(item) != null)
                    {
                        ParagraphProperties User_heading_pPr = new ParagraphProperties();
                        User_heading_pPr.Append(new Justification() { Val = JustificationValues.Center },
                                new BiDi(),
                        new TextDirection()
                        {
                            Val = TextDirectionValues.TopToBottomRightToLeft
                        });
                        Paragraph newParagraph = new Paragraph(User_heading_pPr);

                        newParagraph.Append(new Run(new Text(
                                item.GetType().GetProperty(props[i]).GetValue(item).ToString()
                           )));
                        tr.Descendants<TableCell>().ElementAt(i).Append(newParagraph);
                    }
                    else
                        tr.Descendants<TableCell>().ElementAt(i).Append(new Paragraph(new Run(new Text(""))));
                }
                //insert the row at index starting from 4 
                //as the insert at takes all table tags into consideration including properties tag and grid tag
                table.InsertAt(tr, j + 3+ numberOfHeaderRows);
            }
            //remove the first row
            //if (fullTable)
            //{
                table.Descendants<TableRow>().ToList()[numberOfHeaderRows].Remove();
            //}
            var x = table.Descendants<TableRow>().ToList();
            var iterator = 0;
            if (fullTable)
            {
                iterator = data.Count % (tableLength.Count - 1);
            }
            else
            {
                iterator = tableLength.Count - 2;
            }
            for (int i = 0; i < iterator; i++)
            {
                if (data.Count % (tableLength.Count - 1) == 0 || !fullTable)
                {
                    table.Descendants<TableRow>().ToList()[table.Descendants<TableRow>().ToList().Count - 1].Remove();
                }
                else
                {
                    table.Descendants<TableRow>().ToList()[table.Descendants<TableRow>().ToList().Count - 2].Remove();
                }
            }
            //remove the seconed row from the end
            //table.Descendants<TableRow>().ToList()[tableLength.Count -1].Remove();
            //remove the last row row
            //table.Descendants<TableRow>().ToList()[data.Count + 1].Remove();
        }
        private void ReplaceContent(Dictionary<string, string> replacers)
        {
            MainDocumentPart mainPart = Template.MainDocumentPart;
            //get the arrays of keys provided by the dictionary
            string[] keys = replacers.Keys.ToArray();
            var xm = mainPart.Document.Body.Descendants<Text>().Where(a => a.Text.StartsWith("VR"));
            try
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    //gets the text tags which have a text value that matches the current key
                    var xs = mainPart.Document.Body.Descendants<Text>().Where(a => a.Text.StartsWith("VR")).Where(a => a.Text.Equals(keys[i]));
                    var xArray = xs.ToArray();
                    //gets the first tag that matches the condition as word tends to create a fallback copy of your text to be backward compatible so we ignore the value of that tag
                    for (int j = 0; j < xArray.Length; j++)
                    {
                        var oldtext = xArray[j].Text;
                        xArray[j].Text = oldtext.Replace(keys[i], replacers[keys[i]]);
                    }
                }
            }
            catch (Exception EX)
            {

            }
        }

        private void FillMultiTable(List<ArrayList> dataList, bool fullTable,int numberOfHeaderRows)
        {
            MainDocumentPart mainPart = Template.MainDocumentPart;
            Body bod = mainPart.Document.Body;
            
            //choose the first table
            Table table = bod.Descendants<Table>().FirstOrDefault();
            for (int k = 0; k < dataList.Count; k++)
            {
                var pagebreak = new Paragraph(new Run(new Break() { Type = BreakValues.Page }));
                Table LastTable = bod.Descendants<Table>().LastOrDefault();
                var data = dataList[k];
                Table table2 = (Table)table.Clone();
                var tableLength = table.Descendants<TableRow>().ToList();

                for (int j = 0; j < data.Count; j++)
                {
                    var item = data[j];
                    TableRow tr = null;
                    if (data.Count == tableLength.Count && data.Count == j + 1)
                    {
                        //clone the last row to keep the bottom border of the table
                        tr = table2.Descendants<TableRow>().ToList()[tableLength.Count - 1].CloneNode(true) as TableRow;
                        //table.Descendants<TableRow>().ToList()[table.Descendants<TableRow>().ToList().Count - 1].Remove();
                    }
                    else
                    {
                        //clone the third row(seconed row if we dont count the header) to keep the table inner format
                        tr = table2.Descendants<TableRow>().ToList()[2].CloneNode(true) as TableRow;
                    }
                    string[] props = item.GetType().GetProperties().Select(p => p.Name).ToArray();
                    for (int i = 0; i < props.Length; i++)
                    {
                        //removes that text of the copied cell
                        tr.Descendants<TableCell>().ElementAt(i).RemoveAllChildren<Paragraph>();
                        //append a new text tag to the current cell
                        if (item.GetType().GetProperty(props[i]).GetValue(item) != null)
                        {
                            ParagraphProperties User_heading_pPr = new ParagraphProperties();
                            User_heading_pPr.Append(new Justification() { Val = JustificationValues.Center },
                                    new BiDi(),
                            new TextDirection()
                            {
                                Val = TextDirectionValues.TopToBottomRightToLeft
                            });
                            Paragraph newParagraph = new Paragraph(User_heading_pPr);

                            newParagraph.Append(new Run(new Text(
                                    item.GetType().GetProperty(props[i]).GetValue(item).ToString()
                               )));
                            tr.Descendants<TableCell>().ElementAt(i).Append(newParagraph);
                        }
                        else
                            tr.Descendants<TableCell>().ElementAt(i).Append(new Paragraph(new Run(new Text(""))));
                    }
                    //insert the row at index starting from 4 
                    //as the insert at takes all table tags into consideration including properties tag and grid tag
                   table2.InsertAt(tr, j + 3+ numberOfHeaderRows);
                }
                //remove the first row
                table2.Descendants<TableRow>().ToList()[numberOfHeaderRows].Remove();
                var x = table2.Descendants<TableRow>().ToList();
                var iterator = 0;
                if (fullTable)
                {
                    iterator = data.Count % (tableLength.Count - 1);
                }
                else
                {
                    iterator = tableLength.Count - 3;
                }
                for (int i = 0; i < iterator; i++)
                {
                    if (data.Count % (tableLength.Count - 1) == 0 || !fullTable)
                    {
                        table2.Descendants<TableRow>().ToList()[table2.Descendants<TableRow>().ToList().Count - 2].Remove();
                    }
                    else
                    {
                        table2.Descendants<TableRow>().ToList()[table2.Descendants<TableRow>().ToList().Count - 2].Remove();
                    }
                }
                if (k!=dataList.Count-1)
                {
                    table2.Descendants<TableRow>().LastOrDefault().Remove();
                }
                bod.InsertAfter(table2, LastTable);
                if (k!=0)
                {
                    bod.InsertAfter(pagebreak, LastTable);
                }
                if (dataList.Count % 2==1&&k == dataList.Count - 1)
                {
                    LastTable = bod.Descendants<Table>().LastOrDefault();
                    Table blankTable = (Table)table.Clone();
                    bod.InsertAfter(blankTable, LastTable);
                    bod.InsertAfter(new Paragraph(new Run(new Break() { Type = BreakValues.Page })), LastTable);
                    var descendants = blankTable.Descendants<Text>().Where(a => a.Text.StartsWith("VRTotalPrice")).FirstOrDefault();
                    if (descendants!=null)
                    {
                        descendants.Text = descendants.Text.Replace("VRTotalPrice", "");
                    }
                    
                }
            }
             bod.Descendants<Table>().FirstOrDefault().Remove();
        }
        private void ReplaceSectionContent(List<Dictionary<string, string>> replacers, bool startNewPage = false)
        {
            MainDocumentPart mainPart = Template.MainDocumentPart;

            Body bod = mainPart.Document.Body;
            SdtElement mainSection = bod.Descendants<SdtElement>().FirstOrDefault()?.CloneNode(true) as SdtElement;
            OpenXmlElement iteratorRef = bod.Descendants<SdtElement>().FirstOrDefault();
            for (int i = 0; i < replacers.Count; i++)
            {
                SdtElement element = mainSection.CloneNode(true) as SdtElement;

                FillRepeatedContent(element, replacers[i]);
                bod.InsertAfter(element, iteratorRef);
                iteratorRef = element;
                if (startNewPage && i != replacers.Count - 1)
                {
                    Paragraph pageBreak = new Paragraph(new Run(new Break() { Type = BreakValues.Page }));
                    bod.InsertAfter(pageBreak, iteratorRef);
                    iteratorRef = pageBreak;
                }
            }

            mainPart.Document.Body.Descendants<SdtElement>().FirstOrDefault().RemoveAllChildren();
            mainPart.Document.Body.Descendants<SdtElement>().FirstOrDefault().Remove();

        }
        private void RecplaceFooter(Dictionary<string, string> replacers)
        {
            string[] keys = replacers.Keys.ToArray();
            MainDocumentPart mainPart = Template.MainDocumentPart;
            IEnumerable<FooterPart> footerParts = mainPart.FooterParts.Where(a => a.Footer.Descendants<Text>().ToArray().Length > 0);
            FooterPart footer = footerParts.FirstOrDefault();
            for (int i = 0; i < keys.Length; i++)
            {
                var descendants = footer.Footer.Descendants<Text>().Where(a => a.Text.StartsWith("VR")).Where(a => a.Text.Contains(keys[i]));
                var textMember = descendants.FirstOrDefault();
                if (textMember != null)
                {
                    var oldtext = textMember.Text;
                    textMember.Text = oldtext.Replace(keys[i], replacers[keys[i]]);
                }
            }

        }
        private void RecplaceHeader(Dictionary<string, string> replacers)
        {
            string[] keys = replacers.Keys.ToArray();
            MainDocumentPart mainPart = Template.MainDocumentPart;
            IEnumerable<HeaderPart> headerParts = mainPart.HeaderParts.Where(a => a.Header.Descendants<Text>().ToArray().Length > 0);
            HeaderPart header = headerParts.FirstOrDefault();
            try
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    var descendants = header.Header.Descendants<Text>().Where(a => a.Text.StartsWith("VR")).Where(a => a.Text.Contains(keys[i]));
                    if (descendants != null && descendants.Count() > 0)
                    {
                        var oldtext = descendants.FirstOrDefault().Text;
                        descendants.FirstOrDefault().Text = oldtext.Replace(keys[i], replacers[keys[i]]);
                    }
                }
            }
            catch (NullReferenceException EX)
            {
                //Console.WriteLine(new Exception("There are no headers to replace"));
            }

        }

        private void ReplaceRepetitive(Dictionary<string, string> replacers)
        {
            MainDocumentPart mainPart = Template.MainDocumentPart;
            List<String> keys = replacers.Keys.ToList();
            List<String> repetitiveValues = new List<string>();
            //Get the values which match our condition
            var xm = mainPart.Document.Body.Descendants<Text>().Where(a => a.Text.StartsWith("VR")).Where(a => a.Text.Contains("ComiteeMember")).ToList();
            for (int i = 0; i < xm.Count; i++)
            {
                repetitiveValues.Add(xm[i].Text);
            }
            //this step is done to get the unique repetitive values as word tend to create
            //a fallback value of each text to be compatible with older versions
            List<String> uniqueValues = repetitiveValues.Distinct().ToList();
            try
            {
                for (int i = 0; i < uniqueValues.Count; i++)
                {
                    var xs = mainPart.Document.Body.Descendants<Text>().Where(a => a.Text.Contains(uniqueValues[i]));
                    var xArray = xs.ToArray();
                    var oldtext = xArray[0].Text;
                    var x = keys.Contains(oldtext);
                    if (x)
                    {
                        xArray[0].Text = oldtext.Replace(oldtext, replacers[oldtext]);
                    }
                    else
                    {
                        xArray[0].Text = oldtext.Replace(oldtext, "---------------------");
                    }

                }
            }
            catch (Exception EX)
            {

            }
        }

        //private void SectionRepeaterCreator(List<ArrayList> data, List<Dictionary<string, string>> contentReplacers, int tableIndex = 0, bool startNewPage = false)
        //{
        //    MainDocumentPart mainPart = Template.MainDocumentPart;

        //    Body bod = mainPart.Document.Body;
        //    SdtElement mainSection = mainPart.Document.Body.Descendants<SdtElement>()
        //            .FirstOrDefault().CloneNode(true) as SdtElement;
        //    mainPart.Document.Body.Descendants<SdtElement>().FirstOrDefault().RemoveAllChildren();
        //    mainPart.Document.Body.Descendants<SdtElement>().FirstOrDefault().Remove();
        //    for (int i = 0; i < data.Count; i++)
        //    {
        //        SdtElement element = mainSection.CloneNode(true) as SdtElement;

        //        Table table = element.Descendants<Table>().ToList()[tableIndex];
        //        FillRepeatedTable(table, data[i]);
        //        FillRepeatedContent(element, contentReplacers[i]);
        //        bod.AppendChild(element);
        //        //To insert repeated section in new page
        //        if (startNewPage && i != data.Count - 1)
        //            bod.AppendChild(new Paragraph(new Run(new Break() { Type = BreakValues.Page })));
        //    }

        //}

        private void SectionRepeaterCreator(List<ArrayList> data, List<Dictionary<string, string>> contentReplacers, int sectionIndex = 0, int tableIndex = 0, bool startNewPage = false)
        {
            MainDocumentPart mainPart = Template.MainDocumentPart;

            Body bod = mainPart.Document.Body;
            //var test = bod.Descendants<SdtElement>().Where(b => (b.GetType() == typeof(DocumentFormat.OpenXml.Wordprocessing.Body)));
            SdtElement mainSection = bod.Descendants<SdtElement>().ElementAt(sectionIndex)?.CloneNode(true) as SdtElement;
            OpenXmlElement iteratorRef = bod.Descendants<SdtElement>().ElementAt(sectionIndex);


            for (int i = 0; i < data.Count; i++)
            {
                SdtElement element = mainSection.CloneNode(true) as SdtElement;

                Table table = element.Descendants<Table>().ToList()[tableIndex];
                FillRepeatedTable(table, data[i]);
                FillRepeatedContent(element, contentReplacers[i]);
                bod.InsertAfter(element, iteratorRef);
                iteratorRef = element;
                if (startNewPage && i != data.Count - 1)
                {
                    Paragraph pageBreak = new Paragraph(new Run(new Break() { Type = BreakValues.Page }));
                    bod.InsertAfter(pageBreak, iteratorRef);
                    iteratorRef = pageBreak;
                }
            }

            mainPart.Document.Body.Descendants<SdtElement>().ElementAt(sectionIndex).RemoveAllChildren();
            mainPart.Document.Body.Descendants<SdtElement>().ElementAt(sectionIndex).Remove();


        }
        private void FillRepeatedTable(Table table, ArrayList data)
        {
            for (int j = 0; j < data.Count; j++)
            {
                var item = data[j];
                TableRow tr = null;
                if (data.Count == j + 1)
                {
                    //clone the last row to keep the bottom border of the table
                    tr = table.Descendants<TableRow>().ToList()[data.Count + 2].CloneNode(true) as TableRow;
                }
                else
                {
                    //clone the third row(seconed row if we dont count the header) to keep the table inner format
                    tr = table.Descendants<TableRow>().ToList()[2].CloneNode(true) as TableRow;
                }
                string[] props = item.GetType().GetProperties().Select(p => p.Name).ToArray();
                for (int i = 0; i < props.Length; i++)
                {
                    //removes that text of the copied cell
                    tr.Descendants<TableCell>().ElementAt(i).RemoveAllChildren<Paragraph>();
                    //append a new text tag to the current cell
                    if (item.GetType().GetProperty(props[i]).GetValue(item) != null)
                    {
                        ParagraphProperties User_heading_pPr = new ParagraphProperties();
                        User_heading_pPr.Append(new Justification() { Val = JustificationValues.Center });
                        Paragraph newParagraph = new Paragraph(User_heading_pPr);

                        newParagraph.Append(new Run(new Text(
                                item.GetType().GetProperty(props[i]).GetValue(item).ToString()
                           )));
                        tr.Descendants<TableCell>().ElementAt(i).Append(newParagraph);
                    }
                    else
                        tr.Descendants<TableCell>().ElementAt(i).Append(new Paragraph(new Run(new Text(""))));
                }
                //insert the row at index starting from 4 
                //as the insert at takes all table tags into consideration including properties tag and grid tag
                table.InsertAt(tr, j + 4);
            }
            //remove the first row
            table.Descendants<TableRow>().ToList()[1].Remove();
            //remove the seconed row from the end
            table.Descendants<TableRow>().ToList()[data.Count + 1].Remove();
            //remove the last row row
            table.Descendants<TableRow>().ToList()[data.Count + 1].Remove();
        }
        private void FillRepeatedContent(SdtElement element, Dictionary<String, String> replacers)
        {
            //get the arrays of keys provided by the dictionary
            string[] keys = replacers.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                //gets the text tags which have a text value that matches the current key
                var xs = element.Descendants<Text>().Where(a => a.Text.StartsWith("VR")).Where(a => a.Text.Equals(keys[i]));
                var xArray = xs.ToArray();
                //gets the first tag that matches the condition as word tends to create a fallback copy of your text to be backward compatible so we ignore the value of that tag
                var oldtext = xArray[0].Text;
                xArray[0].Text = oldtext.Replace(keys[i], replacers[keys[i]]);
            }
        }
        private void TableRepeater(List<ArrayList> data)
        {
            MainDocumentPart mainPart = Template.MainDocumentPart;
            Body bod = mainPart.Document.Body;
            for (int i = 0; i < data.Count; i++)
            {
                Table table = bod.Descendants<Table>().FirstOrDefault().CloneNode(true) as Table;


                for (int j = 0; j < data[i].Count; j++)
                {
                    var item = data[i][j];
                    TableRow tr = null;
                    if (data[i].Count == j + 1)
                    {
                        //clone the last row to keep the bottom border of the table
                        tr = table.Descendants<TableRow>().ToList()[data[i].Count + 2].CloneNode(true) as TableRow;
                    }
                    else
                    {
                        //clone the third row(seconed row if we dont count the header) to keep the table inner format
                        tr = table.Descendants<TableRow>().ToList()[2].CloneNode(true) as TableRow;
                    }
                    string[] props = item.GetType().GetProperties().Select(p => p.Name).ToArray();
                    for (int r = 0; r < props.Length; r++)
                    {
                        //removes that text of the copied cell
                        tr.Descendants<TableCell>().ElementAt(r).RemoveAllChildren<Paragraph>();
                        //append a new text tag to the current cell
                        if (item.GetType().GetProperty(props[r]).GetValue(item) != null)
                        {
                            tr.Descendants<TableCell>().ElementAt(r).Append(new Paragraph(new Run(new Text(
                                    item.GetType().GetProperty(props[r]).GetValue(item).ToString()
                               ))));
                        }

                    }
                    //insert the row at index starting from 4 
                    //as the insert at takes all table tags into consideration including properties tag and grid tag
                    table.InsertAt(tr, j + 4);
                }
                //remove the first row
                table.Descendants<TableRow>().ToList()[1].Remove();
                //remove the seconed row from the end
                table.Descendants<TableRow>().ToList()[data[i].Count + 1].Remove();
                //remove the last row row
                table.Descendants<TableRow>().ToList()[data[i].Count + 1].Remove();

                bod.AppendChild(new Paragraph(new Run(new Break())));
                bod.AppendChild(table);
            }

            //RemoveFirstRow

            bod.Descendants<Table>().FirstOrDefault().Remove();

        }


        #endregion
    }
}
