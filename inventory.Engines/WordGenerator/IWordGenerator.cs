using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace inventory.Engines.WordGenerator
{
    public interface IWordGenerator
    {
        MemoryStream PrintDocument(string TemplateName, ArrayList tabledata = null
            , Dictionary<string, string> contentReplacers = null
            , Dictionary<string, string> footerReplacers = null
            , Dictionary<string, string> headerReplacers = null
            , Dictionary<string, string> repetitiveReplacers = null
            , List<Dictionary<string, string>> repetitiveSectionReplacer = null
            , bool fullTable = true,
            int numberOfHeaderRows = 1);
   
        MemoryStream PrintMultiplePageDocument(string TemplateName, List<ArrayList> tabledata = null
           , List<Dictionary<string, string>> contentReplacers = null
           , List<Dictionary<string, string>> footerReplacers = null
           , List<Dictionary<string, string>> headerReplacers = null
            , List<List<ArrayList>> tableReplacers =  null
           , List<Dictionary<string, string>> repetitiveReplacers = null, bool fullTable = true, int numberOfHeaderRows = 1);
        MemoryStream PrintSectionRepeaterDocument(string TemplateName, List<ArrayList> sectionReplacer = null
            , List<Dictionary<string, string>> contentRepeaterReplacers = null
            , Dictionary<string, string> contentReplacers = null
            , Dictionary<string, string> footerReplacers = null
            , Dictionary<string, string> headerReplacers = null
            , Dictionary<string, string> repetitiveReplacers = null
            , List<Dictionary<string, string>> repetitiveSectionReplacer = null
            , int tableIndex = 0
            , int sectionIndex = 0
            , bool startNewPage = false);

    }
}
