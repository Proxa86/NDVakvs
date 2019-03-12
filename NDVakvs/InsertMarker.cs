using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NDVakvs
{
    class InsertMarker
    {
        FolderBrowserDialog fbd;

        public InsertMarker(FolderBrowserDialog fbd)
        {
            this.fbd = fbd;
        }

        public void insertMarker()
        {
            List<string[]> lParentFilters = new List<string[]>();
            List<MarkerFunction> lMarkerFunctions = new List<MarkerFunction>();

            lParentFilters.Add(Directory.GetFiles(fbd.SelectedPath, "*.cpp", SearchOption.AllDirectories));
            lParentFilters.Add(Directory.GetFiles(fbd.SelectedPath, "*.cc", SearchOption.AllDirectories));
            lParentFilters.Add(Directory.GetFiles(fbd.SelectedPath, "*.c", SearchOption.AllDirectories));
            lParentFilters.Add(Directory.GetFiles(fbd.SelectedPath, "*.h", SearchOption.AllDirectories));
            lParentFilters.Add(Directory.GetFiles(fbd.SelectedPath, "*.hh", SearchOption.AllDirectories));
            lParentFilters.Add(Directory.GetFiles(fbd.SelectedPath, "*.hpp", SearchOption.AllDirectories));

            try
            {
                string paternBracesOpen = "{";
                string paternBracesClose = "}";
                string parenthesesOpen = @"\(";
                string parenthesesClose = @"\)";
                string paternReturn = "return";
                string paternIf = "if";
                string parentElse = "else";
                string parentCase = "case";
                string parentDefault = "default";
                string paternSemicolon = ";";
                string parentComment = @"//";
                string parentCommentOpen = @"/\*";
                string parentCommentClose = @"\*/";
                string parentHashTag = @"#";
                string parentStruct = "struct";
                string parentClass = "class";
                string parentNamespace = "namespace";
                bool flag = false;
                bool flagEndReturn = false;
                bool commentOpen = false;
                bool startAnalis = false;
                bool flagCheckFunction = true;
                
                Regex regex;
                Match match;
                int n = 0;
                foreach (var filter in lParentFilters)
                {

                    int indexFile = 0;
                    foreach (var pathFile in filter)
                    {
                        bool includeHeader = false;
                        int indexFunc = 0;
                        var allLines = File.ReadAllLines(pathFile).ToList();

                        for (int i = 0; i < allLines.Count; i++)
                        {
                            regex = new Regex(parentCommentOpen);
                            match = regex.Match(allLines[i]);
                            int indexCommentOpen = match.Index;
                            if(match.Success)
                            {
                                for(int j = i; j < allLines.Count; j++)
                                {
                                    regex = new Regex(parentCommentClose);
                                    match = regex.Match(allLines[j]);
                                    if (match.Success)
                                    {
                                        int indexCommentClose = match.Index;
                                        if (i != j)
                                        {
                                            i = j + 1;
                                            break;
                                        }
                                        else if (indexCommentOpen != indexCommentClose)
                                            continue;
                                        else break;
                                    }
                                    else continue;
                                }
                            }
                            regex = new Regex(parentComment);
                            match = regex.Match(allLines[i]);
                            if (match.Success)
                            {
                                continue;
                            }
                            regex = new Regex(parentHashTag);
                            match = regex.Match(allLines[i]);
                            if (match.Success)
                            {
                                continue;
                            }
                            regex = new Regex(parentStruct);
                            match = regex.Match(allLines[i]);
                            if (match.Success)
                            {
                                for(int j = i; j < allLines.Count; j++)
                                {
                                    regex = new Regex(paternBracesOpen);
                                    match = regex.Match(allLines[j]);
                                    if (match.Success)
                                    {
                                        i = j + 1;
                                        break;
                                    }
                                    else continue;
                                }
                            }
                            regex = new Regex(parentClass);
                            match = regex.Match(allLines[i]);
                            if (match.Success)
                            {
                                for (int j = i; j < allLines.Count; j++)
                                {
                                    regex = new Regex(paternBracesOpen);
                                    match = regex.Match(allLines[j]);
                                    if (match.Success)
                                    {
                                        i = j + 1;
                                        break;
                                    }
                                    else continue;
                                }
                            }
                            regex = new Regex(parentNamespace);
                            match = regex.Match(allLines[i]);
                            if (match.Success)
                            {
                                for (int j = i; j < allLines.Count; j++)
                                {
                                    regex = new Regex(paternBracesOpen);
                                    match = regex.Match(allLines[j]);
                                    if (match.Success)
                                    {
                                        i = j + 1;
                                        break;
                                    }
                                    else continue;
                                }
                            }
                            if (allLines[i] != "")
                            {
                                if(flagCheckFunction)
                                {
                                    if (allLines[i].Contains(";"))
                                        continue;
                                    regex = new Regex(parenthesesOpen);
                                    match = regex.Match(allLines[i]);
                                    if (match.Success)
                                    {
                                        int numberString = i;
                                        string str = "";
                                        for (int j = i; j < allLines.Count; j++)
                                        {
                                            regex = new Regex(parenthesesClose);
                                            match = regex.Match(allLines[j]);
                                            if (match.Success)
                                            {
                                                for (int r = j; r < allLines.Count; r++)
                                                {
                                                    regex = new Regex(paternBracesOpen);
                                                    match = regex.Match(allLines[r]);
                                                    if (!match.Success)
                                                        str += allLines[r].Replace(" ", string.Empty);
                                                    else
                                                    {
                                                        j = r - 1;
                                                        break;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                for (int r = j; r < allLines.Count; r++)
                                                {
                                                    regex = new Regex(paternBracesOpen);
                                                    match = regex.Match(allLines[r]);
                                                    if (!match.Success)
                                                        str += allLines[r].Replace(" ", string.Empty);
                                                    else
                                                    {
                                                        j = r - 1;
                                                        break;
                                                    }
                                                }
                                                
                                            }
                                            i = j;
                                            break;
                                        }
                                        lMarkerFunctions.Add(new MarkerFunction
                                        {
                                            NumberFile = indexFile,
                                            NumberFunction = indexFunc,
                                            NameFunction = str,
                                            NumberString = numberString,
                                            NameFile = pathFile
                                        });
                                    }
                                }
                                
                                regex = new Regex(paternBracesOpen);
                                match = regex.Match(allLines[i]);
                                if (match.Success)
                                {
                                    if (n == 0)
                                    {
                                        includeHeader = true;
                                        flagCheckFunction = false;
                                        allLines.Insert(i + 1, String.Format("\t_akvs_probe(\"{0}:{1}\",'f','i')",indexFile,indexFunc));
                                        ++n;
                                    }
                                    else ++n;
                                    
   
                                }

                                regex = new Regex(paternIf);
                                match = regex.Match(allLines[i]);
                                if (match.Success)
                                {
                                    int indexStartIf = match.Index;
                                    int indexEnd = allLines[i].LastIndexOf(")");
                                    regex = new Regex(paternReturn);
                                    match = regex.Match(allLines[i]);
                                    if (match.Success)
                                    {
                                        int indexStartReturn = match.Index;
                                        int indexEnd1 = allLines[i].IndexOf(";");
                                        string s = allLines[i].Substring(indexStartIf, indexEnd - indexStartIf + 1);
                                        string sa = allLines[i].Substring(indexStartReturn, indexEnd1 - indexStartReturn + 1);
                                        //allLines.Remove(allLines[i]);
                                        allLines.RemoveAt(i);
                                        allLines.Insert(i, new string('\t', n) + s);
                                        allLines.Insert(i + 1, new string('\t',n)+"{");
                                        allLines.Insert(i + 2, new string('\t', n + 1)+String.Format("_akvs_probe(\"{0}:{1}\",'f','o')", indexFile, indexFunc));
                                        allLines.Insert(i + 3, new string('\t', n + 1) + sa);
                                        allLines.Insert(i + 4, new string('\t', n) + "}");
                                        allLines.Insert(i + 5, "");
                                        i += 5;

                                    }

                                    regex = new Regex(paternReturn);
                                    match = regex.Match(allLines[i + 1]);
                                    if (match.Success)
                                    {
                                        int indexStartReturn = match.Index;
                                        int indexEnd1 = allLines[i + 1].IndexOf(";");
                                        string sa = allLines[i + 1].Substring(indexStartReturn, indexEnd1 - indexStartReturn + 1);
                                        //allLines.Remove(allLines[i + 1]);
                                        allLines.RemoveAt(i + 1);
                                        allLines.Insert(i + 1, new string('\t',n)+"{");
                                        allLines.Insert(i + 2, new string('\t', n + 1) + String.Format("_akvs_probe(\"{0}:{1}\",'f','o')", indexFile, indexFunc));
                                        allLines.Insert(i + 3, new string('\t', n + 1) + sa);
                                        allLines.Insert(i + 4, new string('\t', n) + "}");
                                        allLines.Insert(i + 5, "");
                                        //allLines.RemoveAt(i + 1);
                                        //allLines.Insert(i + 1, "\t{");
                                        //allLines.Insert(i + 2, String.Format("\t\t_akvs_probe(\"{0}:{1}\",'f','o')", indexFile, indexFunc));
                                        //allLines.Insert(i + 3, "\t\t" + sa);
                                        //allLines.Insert(i + 4, "\t}");
                                        //allLines.Insert(i + 5, "");
                                        i += 5;
                                    }
                                }
                                else
                                {
                                    regex = new Regex(paternReturn);
                                    match = regex.Match(allLines[i]);
                                    if (match.Success)
                                    {
                                        allLines.Insert(i, new string('\t', n) + String.Format("_akvs_probe(\"{0}:{1}\",'f','o')", indexFile, indexFunc));
                                        ++i;
                                        if (n == 1)
                                        {
                                            flagEndReturn = true;
                                            ++indexFunc;
                                        }
                                            
                                    }
                                }

                                regex = new Regex(parentElse);
                                match = regex.Match(allLines[i]);
                                if (match.Success)
                                {
                                    int indexStartElse = match.Index;
                                    int indexEndElse = match.Index + 3;
                                    regex = new Regex(paternReturn);
                                    match = regex.Match(allLines[i]);
                                    if (match.Success)
                                    {
                                        int indexStartReturn = match.Index;
                                        int indexEnd1 = allLines[i].IndexOf(";");
                                        string s = allLines[i].Substring(indexStartElse, indexEndElse - indexStartElse + 1);
                                        string sa = allLines[i].Substring(indexStartReturn, indexEnd1 - indexStartReturn + 1);
                                        //allLines.Remove(allLines[i]);
                                        allLines.RemoveAt(i);
                                        allLines.Insert(i, new string('\t', n) + s);
                                        allLines.Insert(i + 1, new string('\t', n)+"{");
                                        allLines.Insert(i + 2, new string('\t', n + 1) + String.Format("_akvs_probe(\"{0}:{1}\",'f','o')", indexFile, indexFunc));
                                        allLines.Insert(i + 3, new string('\t', n + 1) + sa);
                                        allLines.Insert(i + 4, new string('\t', n)+"}");
                                        allLines.Insert(i + 5, "");
                                        i += 5;

                                    }

                                    regex = new Regex(paternReturn);
                                    match = regex.Match(allLines[i + 1]);
                                    if (match.Success)
                                    {
                                        int indexStartReturn = match.Index;
                                        int indexEnd1 = allLines[i + 1].IndexOf(";");
                                        string sa = allLines[i + 1].Substring(indexStartReturn, indexEnd1 - indexStartReturn + 1);
                                        //allLines.Remove(allLines[i + 1]);
                                        allLines.RemoveAt(i + 1);
                                        allLines.Insert(i + 1, new string('\t', n) + "{");
                                        allLines.Insert(i + 2, new string ('\t', n + 1) + String.Format("_akvs_probe(\"{0}:{1}\",'f','o')", indexFile, indexFunc));
                                        allLines.Insert(i + 3, new string('\t', n + 1) + sa);
                                        allLines.Insert(i + 4, new string('\t', n)+"}");
                                        allLines.Insert(i + 5, "");
                                        i += 5;
                                    }
                                }
                                //else
                                //{
                                //    regex = new Regex(paternReturn);
                                //    match = regex.Match(allLines[i]);
                                //    if (match.Success)
                                //    {
                                //        allLines.Insert(i, String.Format("\t_akvs_probe(\"{0}:{1}\",'f','o')", indexFile, indexFunc));
                                //        ++i;
                                //        if (n == 1)
                                //        {
                                //            flagEndReturn = true;
                                //            ++indexFunc;
                                //        }

                                //    }
                                //}

                                regex = new Regex(parentCase);
                                match = regex.Match(allLines[i]);
                                if (match.Success)
                                {
                                    int indexStartCase = match.Index;
                                    int indexEndCase = allLines[i].LastIndexOf(":");
                                    regex = new Regex(paternReturn);
                                    match = regex.Match(allLines[i]);
                                    if (match.Success)
                                    {
                                        int indexStartReturn = match.Index;
                                        int indexEnd1 = allLines[i].IndexOf(";");
                                        string s = allLines[i].Substring(indexStartCase, indexEndCase - indexStartCase + 1);
                                        string sa = allLines[i].Substring(indexStartReturn, indexEnd1 - indexStartReturn + 1);
                                        //allLines.Remove(allLines[i]);
                                        allLines.RemoveAt(i);
                                        allLines.Insert(i, new string('\t', n) + s);
                                        allLines.Insert(i + 1, new string('\t', n) +"{");
                                        allLines.Insert(i + 2, new string('\t', n + 1) + String.Format("_akvs_probe(\"{0}:{1}\",'f','o')", indexFile, indexFunc));
                                        allLines.Insert(i + 3, new string('\t', n + 1) + sa);
                                        allLines.Insert(i + 4, new string('\t', n) + "}");
                                        allLines.Insert(i + 5, "");
                                        i += 5;

                                    }

                                    regex = new Regex(paternReturn);
                                    match = regex.Match(allLines[i + 1]);
                                    if (match.Success)
                                    {
                                        int indexStartReturn = match.Index;
                                        int indexEnd1 = allLines[i + 1].IndexOf(";");
                                        string sa = allLines[i + 1].Substring(indexStartReturn, indexEnd1 - indexStartReturn + 1);
                                        //allLines.Remove(allLines[i + 1]);
                                        allLines.RemoveAt(i + 1);
                                        allLines.Insert(i + 1, new string('\t', n) + "{");
                                        allLines.Insert(i + 2, new string('\t', n + 1) + String.Format("_akvs_probe(\"{0}:{1}\",'f','o')", indexFile, indexFunc));
                                        allLines.Insert(i + 3, new string('\t', n + 1) + sa);
                                        allLines.Insert(i + 4, new string('\t', n) + "}");
                                        allLines.Insert(i + 5, "");
                                        i += 5;
                                    }
                                }

                                regex = new Regex(parentDefault);
                                match = regex.Match(allLines[i]);
                                if (match.Success)
                                {
                                    int indexStartDefault = match.Index;
                                    int indexEndDefault = allLines[i].LastIndexOf(":");
                                    regex = new Regex(paternReturn);
                                    match = regex.Match(allLines[i]);
                                    if (match.Success)
                                    {
                                        int indexStartReturn = match.Index;
                                        int indexEnd1 = allLines[i].IndexOf(";");
                                        string s = allLines[i].Substring(indexStartDefault, indexEndDefault - indexStartDefault + 1);
                                        string sa = allLines[i].Substring(indexStartReturn, indexEnd1 - indexStartReturn + 1);
                                        //allLines.Remove(allLines[i]);
                                        allLines.RemoveAt(i);
                                        allLines.Insert(i, new string('\t', n) + s);
                                        allLines.Insert(i + 1, new string('\t', n) + "{");
                                        allLines.Insert(i + 2, new string('\t', n + 1) + String.Format("_akvs_probe(\"{0}:{1}\",'f','o')", indexFile, indexFunc));
                                        allLines.Insert(i + 3, new string('\t', n + 1) + sa);
                                        allLines.Insert(i + 4, new string('\t', n) + "}");
                                        allLines.Insert(i + 5, "");
                                        i += 5;

                                    }

                                    regex = new Regex(paternReturn);
                                    match = regex.Match(allLines[i + 1]);
                                    if (match.Success)
                                    {
                                        int indexStartReturn = match.Index;
                                        int indexEnd1 = allLines[i + 1].IndexOf(";");
                                        string sa = allLines[i + 1].Substring(indexStartReturn, indexEnd1 - indexStartReturn + 1);
                                        //allLines.Remove(allLines[i + 1]);
                                        allLines.RemoveAt(i + 1);
                                        allLines.Insert(i + 1, new string('\t', n) + "{");
                                        allLines.Insert(i + 2, new string('\t', n + 1) + String.Format("_akvs_probe(\"{0}:{1}\",'f','o')", indexFile, indexFunc));
                                        allLines.Insert(i + 3, new string('\t', n + 1) + sa);
                                        allLines.Insert(i + 4, new string('\t', n ) + "}");
                                        allLines.Insert(i + 5, "");
                                        i += 5;
                                    }
                                }

                                regex = new Regex(paternBracesClose);
                                match = regex.Match(allLines[i]);
                                if (match.Success)
                                {
                                    --n;
                                    if (flagEndReturn)
                                    {
                                        flagEndReturn = false;
                                        flagCheckFunction = true;
                                    }
                                    else if (n == 0)
                                    {
                                        flagCheckFunction = true;
                                        allLines.Insert(i, String.Format("\t_akvs_probe(\"{0}:{1}\",'f','o')", indexFile, indexFunc));
                                        ++indexFunc;
                                        ++i;
                                    }
                                }
                            }
                        }
                        if (includeHeader)
                            allLines.Insert(0, "#include \"akvs_probe.h\"");
                        File.WriteAllLines(pathFile, allLines.ToArray());
                        ++indexFile;
                        

                    }
                }
                MessageBox.Show("Insert  markers.");

            }
            catch (Exception e)
            {
                MessageBox.Show("Can't open file.\nOriginal error: " + e.Message);
            }
        }
    }
}
