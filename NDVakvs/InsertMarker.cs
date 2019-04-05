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
        int indexFile = 0;

        public InsertMarker(FolderBrowserDialog fbd)
        {
            this.fbd = fbd;
        }

        public void insertMarker()
        {
            List<string[]> lParentFilters = new List<string[]>();


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
                //string parenthesesOpen = @"\(";
                //string parenthesesClose = @"\)";
                string paternReturn = "return";
                //string paternIf = "if";
                //string parentElse = "else";
                //string parentCase = "case";
                //string parentDefault = "default";
                string paternSemicolon = ";";
                //string parentComment = @"//";
                //string parentCommentOpen = @"/\*";
                //string parentCommentClose = @"\*/";
                //string parentHashTag = @"#";
                //string parentStruct = "^struct";
                //string parentClass = "^class";
                //string parentNamespace = "^namespace";
                bool flag = false;
                bool flagEndReturn = false;
                bool commentOpen = false;
                bool startAnalis = false;
                bool flagCheckFunction = true;

                Regex regex;
                Match match;
                int countOpenCloseBrace = 0;

                foreach (var filter in lParentFilters)
                {


                    foreach (var pathFile in filter)
                    {
                        bool includeHeader = false;
                        int indexFunc = 0;
                        var allLines = File.ReadAllLines(pathFile).ToList();
                        Console.WriteLine(pathFile);

                        for (int i = 0; i < allLines.Count; i++)
                        {

                            if (checkComment(allLines, ref i))
                                continue;
                            if (checkClass(allLines, ref i))
                                continue;
                            if (checkQt(allLines, ref i))
                                continue;
                            if (checkSpace(allLines, ref i))
                                continue;
                            if (checkDifinishinsFunction(allLines, ref i))
                                continue;

                            if (flagCheckFunction)
                            {
                                if (checkDifinishinsFunction(allLines, ref i))
                                    return;

                                searchFunctions(allLines, ref i, ref indexFunc, pathFile);

                                insertMarkerBracesOpen(allLines, ref i, ref countOpenCloseBrace, ref indexFunc, ref flagCheckFunction, ref includeHeader);

                                insertMarkerIf(allLines, ref i, ref countOpenCloseBrace, ref indexFunc, ref flagEndReturn);
                                insertMarkerElse(allLines, ref i, ref countOpenCloseBrace, ref indexFunc);
                                insertMarkerCase(allLines, ref i, ref countOpenCloseBrace, ref indexFunc);
                                insertMarkerDefault(allLines, ref i, ref countOpenCloseBrace, ref indexFunc);

                                //// Ищем (
                                //regex = new Regex(parenthesesOpen);
                                //match = regex.Match(allLines[i]);
                                //if (match.Success)
                                //{
                                //    int numberString = i;
                                //    string str = "";
                                //    for (int j = i; j < allLines.Count; j++)
                                //    {
                                //        // Ищем )
                                //        regex = new Regex(parenthesesClose);
                                //        match = regex.Match(allLines[j]);
                                //        if (match.Success)
                                //        {
                                //            // Нашли )
                                //            for (int r = j; r < allLines.Count; r++)
                                //            {
                                //                //Ищем {
                                //                regex = new Regex(paternBracesOpen);
                                //                match = regex.Match(allLines[r]);
                                //                if (!match.Success)
                                //                {
                                //                    if (!allLines[r].Contains(';'))
                                //                        //str += allLines[r].Replace(" ", string.Empty);
                                //                        str += allLines[r].TrimStart(' ');
                                //                    else
                                //                    {
                                //                        str = "";
                                //                        continue;
                                //                    }
                                //                }
                                //                else
                                //                {
                                //                    j = r - 1;
                                //                    break;
                                //                }
                                //            }
                                //        }
                                //        else // Если нет )
                                //        {
                                //            for (int r = j; r < allLines.Count; r++)
                                //            {
                                //                regex = new Regex(paternBracesOpen);
                                //                match = regex.Match(allLines[r]);
                                //                if (!match.Success)
                                //                {
                                //                    if (!allLines[r].Contains(';'))
                                //                        //str += allLines[r].Replace(" ", string.Empty);
                                //                        str += allLines[r].TrimStart(' ');
                                //                    else
                                //                    {
                                //                        str = "";
                                //                        break;
                                //                    }
                                //                }
                                //                else
                                //                {
                                //                    j = r - 1;
                                //                    break;
                                //                }
                                //            }

                                //        }
                                //        i = j;
                                //        break;
                                //    }
                                //    if (!str.Equals(""))
                                //    {
                                //        lMarkerFunctions.Add(new MarkerFunction
                                //        {
                                //            NumberFile = indexFile,
                                //            NumberFunction = indexFunc,
                                //            NameFunction = str,
                                //            NumberString = numberString,
                                //            NameFile = pathFile
                                //        });
                                //    }

                                //    Console.WriteLine(lMarkerFunctions.Count + "\t" + str);
                                //}
                            }

                            //regex = new Regex(paternBracesOpen);
                            //match = regex.Match(allLines[i]);
                            //if (match.Success)
                            //{
                            //    if (n == 0)
                            //    {
                            //        includeHeader = true;
                            //        flagCheckFunction = false;
                            //        allLines.Insert(i + 1, String.Format("\t_akvs_probe(\"{0}:{1}\",'f','i');", indexFile, indexFunc));
                            //        ++n;
                            //    }
                            //    else ++n;


                            //}

                            //regex = new Regex(paternIf);
                            //match = regex.Match(allLines[i]);
                            //if (match.Success)
                            //{
                            //    int indexStartIf = match.Index;
                            //    int indexEnd = allLines[i].LastIndexOf(")");
                            //    regex = new Regex(paternReturn);
                            //    match = regex.Match(allLines[i]);
                            //    if (match.Success)
                            //    {
                            //        int indexStartReturn = match.Index;
                            //        int indexEnd1 = allLines[i].IndexOf(";");
                            //        string s = allLines[i].Substring(indexStartIf, indexEnd - indexStartIf + 1);
                            //        string sa = allLines[i].Substring(indexStartReturn, indexEnd1 - indexStartReturn + 1);
                            //        //allLines.Remove(allLines[i]);
                            //        allLines.RemoveAt(i);
                            //        allLines.Insert(i, new string('\t', countOpenCloseBrace) + s);
                            //        allLines.Insert(i + 1, new string('\t', countOpenCloseBrace) + "{");
                            //        allLines.Insert(i + 2, new string('\t', countOpenCloseBrace + 1) + String.Format("_akvs_probe(\"{0}:{1}\",'f','o');", indexFile, indexFunc));
                            //        allLines.Insert(i + 3, new string('\t', countOpenCloseBrace + 1) + sa);
                            //        allLines.Insert(i + 4, new string('\t', countOpenCloseBrace) + "}");
                            //        allLines.Insert(i + 5, "");
                            //        i += 5;

                            //    }

                            //    regex = new Regex(paternReturn);
                            //    match = regex.Match(allLines[i + 1]);
                            //    if (match.Success)
                            //    {
                            //        int indexStartReturn = match.Index;
                            //        int indexEnd1 = allLines[i + 1].IndexOf(";");
                            //        string sa = allLines[i + 1].Substring(indexStartReturn, indexEnd1 - indexStartReturn + 1);
                            //        //allLines.Remove(allLines[i + 1]);
                            //        allLines.RemoveAt(i + 1);
                            //        allLines.Insert(i + 1, new string('\t', countOpenCloseBrace) + "{");
                            //        allLines.Insert(i + 2, new string('\t', countOpenCloseBrace + 1) + String.Format("_akvs_probe(\"{0}:{1}\",'f','o');", indexFile, indexFunc));
                            //        allLines.Insert(i + 3, new string('\t', countOpenCloseBrace + 1) + sa);
                            //        allLines.Insert(i + 4, new string('\t', countOpenCloseBrace) + "}");
                            //        allLines.Insert(i + 5, "");
                            //        //allLines.RemoveAt(i + 1);
                            //        //allLines.Insert(i + 1, "\t{");
                            //        //allLines.Insert(i + 2, String.Format("\t\t_akvs_probe(\"{0}:{1}\",'f','o')", indexFile, indexFunc));
                            //        //allLines.Insert(i + 3, "\t\t" + sa);
                            //        //allLines.Insert(i + 4, "\t}");
                            //        //allLines.Insert(i + 5, "");
                            //        i += 5;
                            //    }
                            //}
                            //else
                            //{
                            //    regex = new Regex(paternReturn);
                            //    match = regex.Match(allLines[i]);
                            //    if (match.Success)
                            //    {
                            //        allLines.Insert(i, new string('\t', countOpenCloseBrace) + String.Format("_akvs_probe(\"{0}:{1}\",'f','o');", indexFile, indexFunc));
                            //        ++i;
                            //        if (countOpenCloseBrace == 1)
                            //        {
                            //            flagEndReturn = true;
                            //            ++indexFunc;
                            //        }

                            //    }
                            //}



                            //regex = new Regex(parentCase);
                            //match = regex.Match(allLines[i]);
                            //if (match.Success)
                            //{
                            //    int indexStartCase = match.Index;
                            //    int indexEndCase = allLines[i].LastIndexOf(":");
                            //    regex = new Regex(paternReturn);
                            //    match = regex.Match(allLines[i]);
                            //    if (match.Success)
                            //    {
                            //        int indexStartReturn = match.Index;
                            //        int indexEnd1 = allLines[i].IndexOf(";");
                            //        string s = allLines[i].Substring(indexStartCase, indexEndCase - indexStartCase + 1);
                            //        string sa = allLines[i].Substring(indexStartReturn, indexEnd1 - indexStartReturn + 1);
                            //        //allLines.Remove(allLines[i]);
                            //        allLines.RemoveAt(i);
                            //        allLines.Insert(i, new string('\t', countOpenCloseBrace) + s);
                            //        allLines.Insert(i + 1, new string('\t', countOpenCloseBrace) + "{");
                            //        allLines.Insert(i + 2, new string('\t', countOpenCloseBrace + 1) + String.Format("_akvs_probe(\"{0}:{1}\",'f','o');", indexFile, indexFunc));
                            //        allLines.Insert(i + 3, new string('\t', countOpenCloseBrace + 1) + sa);
                            //        allLines.Insert(i + 4, new string('\t', countOpenCloseBrace) + "}");
                            //        allLines.Insert(i + 5, "");
                            //        i += 5;

                            //    }

                            //    regex = new Regex(paternReturn);
                            //    match = regex.Match(allLines[i + 1]);
                            //    if (match.Success)
                            //    {
                            //        int indexStartReturn = match.Index;
                            //        int indexEnd1 = allLines[i + 1].IndexOf(";");
                            //        string sa = allLines[i + 1].Substring(indexStartReturn, indexEnd1 - indexStartReturn + 1);
                            //        //allLines.Remove(allLines[i + 1]);
                            //        allLines.RemoveAt(i + 1);
                            //        allLines.Insert(i + 1, new string('\t', countOpenCloseBrace) + "{");
                            //        allLines.Insert(i + 2, new string('\t', countOpenCloseBrace + 1) + String.Format("_akvs_probe(\"{0}:{1}\",'f','o');", indexFile, indexFunc));
                            //        allLines.Insert(i + 3, new string('\t', countOpenCloseBrace + 1) + sa);
                            //        allLines.Insert(i + 4, new string('\t', countOpenCloseBrace) + "}");
                            //        allLines.Insert(i + 5, "");
                            //        i += 5;
                            //    }
                            //}

                            //regex = new Regex(parentDefault);
                            //match = regex.Match(allLines[i]);
                            //if (match.Success)
                            //{
                            //    int indexStartDefault = match.Index;
                            //    int indexEndDefault = allLines[i].LastIndexOf(":");
                            //    regex = new Regex(paternReturn);
                            //    match = regex.Match(allLines[i]);
                            //    if (match.Success)
                            //    {
                            //        int indexStartReturn = match.Index;
                            //        int indexEnd1 = allLines[i].IndexOf(";");
                            //        string s = allLines[i].Substring(indexStartDefault, indexEndDefault - indexStartDefault + 1);
                            //        string sa = allLines[i].Substring(indexStartReturn, indexEnd1 - indexStartReturn + 1);
                            //        //allLines.Remove(allLines[i]);
                            //        allLines.RemoveAt(i);
                            //        allLines.Insert(i, new string('\t', countOpenCloseBrace) + s);
                            //        allLines.Insert(i + 1, new string('\t', countOpenCloseBrace) + "{");
                            //        allLines.Insert(i + 2, new string('\t', countOpenCloseBrace + 1) + String.Format("_akvs_probe(\"{0}:{1}\",'f','o');", indexFile, indexFunc));
                            //        allLines.Insert(i + 3, new string('\t', countOpenCloseBrace + 1) + sa);
                            //        allLines.Insert(i + 4, new string('\t', countOpenCloseBrace) + "}");
                            //        allLines.Insert(i + 5, "");
                            //        i += 5;

                            //    }

                            //    regex = new Regex(paternReturn);
                            //    match = regex.Match(allLines[i + 1]);
                            //    if (match.Success)
                            //    {
                            //        int indexStartReturn = match.Index;
                            //        int indexEnd1 = allLines[i + 1].IndexOf(";");
                            //        string sa = allLines[i + 1].Substring(indexStartReturn, indexEnd1 - indexStartReturn + 1);
                            //        //allLines.Remove(allLines[i + 1]);
                            //        allLines.RemoveAt(i + 1);
                            //        allLines.Insert(i + 1, new string('\t', countOpenCloseBrace) + "{");
                            //        allLines.Insert(i + 2, new string('\t', countOpenCloseBrace + 1) + String.Format("_akvs_probe(\"{0}:{1}\",'f','o');", indexFile, indexFunc));
                            //        allLines.Insert(i + 3, new string('\t', countOpenCloseBrace + 1) + sa);
                            //        allLines.Insert(i + 4, new string('\t', countOpenCloseBrace) + "}");
                            //        allLines.Insert(i + 5, "");
                            //        i += 5;
                            //    }
                            //}

                            regex = new Regex(paternBracesClose);
                            match = regex.Match(allLines[i]);
                            if (match.Success)
                            {
                                --countOpenCloseBrace;
                                if (flagEndReturn)
                                {
                                    flagEndReturn = false;
                                    flagCheckFunction = true;
                                }
                                else if (countOpenCloseBrace == 0)
                                {
                                    flagCheckFunction = true;
                                    allLines.Insert(i, String.Format("\t_akvs_probe(\"{0}:{1}\",'f','o');", indexFile, indexFunc));
                                    ++indexFunc;
                                    ++i;
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

        private bool checkComment(List<string> allLines, ref int i)
        {
            string parentCommentOpen = @"/\*";
            string parentCommentClose = @"\*/";
            string parentHashTag = @"#";
            string parentCommentDoubleSlash = @"//";
            bool flag = false;

            Regex regex = new Regex(parentCommentOpen);
            Match match = regex.Match(allLines[i]);
            int indexStarOpen = allLines[i].IndexOf('*');
            int indexMultiCommentOpen = match.Index;
            if (match.Success)
            {
                flag = true;
                for (int j = i; j < allLines.Count; j++)
                {
                    regex = new Regex(parentCommentClose);
                    match = regex.Match(allLines[j]);
                    if (match.Success)
                    {
                        int indexStarClose = allLines[i].LastIndexOf('*');
                        int indexMultiCommentClose = match.Index;
                        if (i != j)
                        {
                            i = j + 1;
                            break;
                        }
                        else if (i == j)
                        {
                            if (indexMultiCommentOpen != indexMultiCommentClose)
                            {
                                if (indexStarOpen == indexStarClose)
                                {
                                    continue;
                                }
                                else break;
                            }
                            else break;
                        }
                        else break;
                    }
                    else continue;
                }
                --i;
            }
            regex = new Regex(parentCommentDoubleSlash);
            match = regex.Match(allLines[i]);
            if (match.Success)
            {
                flag = true;
                //++i;
            }
            regex = new Regex(parentHashTag);
            match = regex.Match(allLines[i]);
            if (match.Success)
            {
                flag = true;
                for (int j = i + 1; j < allLines.Count; j++)
                {
                    regex = new Regex(parentHashTag);
                    match = regex.Match(allLines[j]);
                    if (match.Success)
                    {
                        continue;
                    }
                    else
                    {
                        i = j;
                        break;
                    }
                }
                --i;
            }

            return flag;
        }

        private bool checkClass(List<string> allLines, ref int i)
        {
            string paternBracesOpen = "{";
            string parentStruct = "^struct";
            string parentClass = "^class";
            string parentNamespace = "^namespace";
            bool flag = false;

            Regex regex = new Regex(parentStruct);
            Match match = regex.Match(allLines[i]);
            if (match.Success)
            {
                flag = true;
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
            regex = new Regex(parentClass);
            match = regex.Match(allLines[i]);
            if (match.Success)
            {
                flag = true;
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
                flag = true;
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
            return flag;
        }

        private bool checkQt(List<string> allLines, ref int i)
        {
            string parentQ = "^Q_";
            string parentQt = "^QT_";
            bool flag = false;

            Regex regex = new Regex(parentQ);
            Match match = regex.Match(allLines[i]);
            if (match.Success)
            {
                flag = true;
                //++i;
            }
            else
            {
                regex = new Regex(parentQt);
                match = regex.Match(allLines[i]);
                if (match.Success)
                {
                    flag = true;
                    //++i;
                }
            }

            return flag;
        }

        private bool checkSpace(List<string> allLines, ref int i)
        {
            if (allLines[i].Equals(""))
                return true;
            else return false;
        }

        private bool checkDifinishinsFunction(List<string> allLines, ref int i)
        {
            if (allLines[i].Contains(";"))
                return true;
            else return false;
        }

        private void searchFunctions(List<string> allLines, ref int i, ref int indexFunc, string pathFile)
        {
            string paternBracesOpen = "{";
            string parenthesesOpen = @"\(";
            //string parenthesesClose = @"\)";

            List<MarkerFunction> lMarkerFunctions = new List<MarkerFunction>();



            Regex regex = new Regex(parenthesesOpen);
            Match match = regex.Match(allLines[i]);
            if (match.Success)
            {
                int numberString = i;
                string str = "";
                for (int j = i; j < allLines.Count; j++)
                {
                    // Ищем )
                    regex = new Regex(paternBracesOpen);
                    match = regex.Match(allLines[j]);
                    if (!match.Success)
                    {
                        // Нашли )
                        //for (int r = j; r < allLines.Count; r++)
                        {
                            //Ищем {
                            //regex = new Regex(paternBracesOpen);
                            //match = regex.Match(allLines[r]);
                            //if (!match.Success)
                            //{
                            if (!allLines[j].Contains(';'))
                                //str += allLines[r].Replace(" ", string.Empty);
                                str += allLines[j].TrimStart(' ');
                            else
                            {
                                str = "";
                                continue;
                            }
                            //}

                        }
                    }
                    else // Если нет )
                    {
                        i = j;
                        break;

                        //for (int r = j; r < allLines.Count; r++)
                        //{
                        //    regex = new Regex(paternBracesOpen);
                        //    match = regex.Match(allLines[r]);
                        //    if (!match.Success)
                        //    {
                        //        if (!allLines[r].Contains(';'))
                        //            //str += allLines[r].Replace(" ", string.Empty);
                        //            str += allLines[r].TrimStart(' ');
                        //        else
                        //        {
                        //            str = "";
                        //            break;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        j = r - 1;
                        //        break;
                        //    }
                        //}

                    }
                    //i = j;
                    //break;
                }
                if (!str.Equals(""))
                {
                    lMarkerFunctions.Add(new MarkerFunction
                    {
                        NumberFile = indexFile,
                        NumberFunction = indexFunc,
                        NameFunction = str,
                        NumberString = numberString,
                        NameFile = pathFile
                    });
                }

                Console.WriteLine(lMarkerFunctions.Count + "\t" + str);
            }
        }

        private void insertMarkerBracesOpen(List<string> allLines, ref int i, ref int countOpenCloseBrace, ref int indexFunc, ref bool flagCheckFunction, ref bool includeHeader)
        {
            string paternBracesOpen = "{";

            Regex regex = new Regex(paternBracesOpen);
            Match match = regex.Match(allLines[i]);
            if (match.Success)
            {
                if (countOpenCloseBrace == 0)
                {
                    includeHeader = true;
                    flagCheckFunction = false;
                    allLines.Insert(i + 1, String.Format("\t_akvs_probe(\"{0}:{1}\",'f','i');", indexFile, indexFunc));
                    ++countOpenCloseBrace;
                }
                else ++countOpenCloseBrace;


            }
        }

        private void insertMarkerIf(List<string> allLines, ref int i, ref int countOpenCloseBrace, ref int indexFunc, ref bool flagEndReturn)
        {
            string paternIf = "if";
            string paternReturn = "return";

            Regex regex = new Regex(paternIf);
            Match match = regex.Match(allLines[i]);
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
                    allLines.Insert(i, new string('\t', countOpenCloseBrace) + s);
                    allLines.Insert(i + 1, new string('\t', countOpenCloseBrace) + "{");
                    allLines.Insert(i + 2, new string('\t', countOpenCloseBrace + 1) + String.Format("_akvs_probe(\"{0}:{1}\",'f','o');", indexFile, indexFunc));
                    allLines.Insert(i + 3, new string('\t', countOpenCloseBrace + 1) + sa);
                    allLines.Insert(i + 4, new string('\t', countOpenCloseBrace) + "}");
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
                    allLines.Insert(i + 1, new string('\t', countOpenCloseBrace) + "{");
                    allLines.Insert(i + 2, new string('\t', countOpenCloseBrace + 1) + String.Format("_akvs_probe(\"{0}:{1}\",'f','o');", indexFile, indexFunc));
                    allLines.Insert(i + 3, new string('\t', countOpenCloseBrace + 1) + sa);
                    allLines.Insert(i + 4, new string('\t', countOpenCloseBrace) + "}");
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
                    allLines.Insert(i, new string('\t', countOpenCloseBrace) + String.Format("_akvs_probe(\"{0}:{1}\",'f','o');", indexFile, indexFunc));
                    ++i;
                    if (countOpenCloseBrace == 1)
                    {
                        flagEndReturn = true;
                        ++indexFunc;
                    }

                }
            }
        }

        private void insertMarkerElse(List<string> allLines, ref int i, ref int countOpenCloseBrace, ref int indexFunc)
        {
            string parentElse = "else";
            string paternReturn = "return";

            Regex regex = new Regex(parentElse);
            Match match = regex.Match(allLines[i]);
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
                    allLines.Insert(i, new string('\t', countOpenCloseBrace) + s);
                    allLines.Insert(i + 1, new string('\t', countOpenCloseBrace) + "{");
                    allLines.Insert(i + 2, new string('\t', countOpenCloseBrace + 1) + String.Format("_akvs_probe(\"{0}:{1}\",'f','o');", indexFile, indexFunc));
                    allLines.Insert(i + 3, new string('\t', countOpenCloseBrace + 1) + sa);
                    allLines.Insert(i + 4, new string('\t', countOpenCloseBrace) + "}");
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
                    allLines.Insert(i + 1, new string('\t', countOpenCloseBrace) + "{");
                    allLines.Insert(i + 2, new string('\t', countOpenCloseBrace + 1) + String.Format("_akvs_probe(\"{0}:{1}\",'f','o');", indexFile, indexFunc));
                    allLines.Insert(i + 3, new string('\t', countOpenCloseBrace + 1) + sa);
                    allLines.Insert(i + 4, new string('\t', countOpenCloseBrace) + "}");
                    allLines.Insert(i + 5, "");
                    i += 5;
                }
            }
        }

        private void insertMarkerCase(List<string> allLines, ref int i, ref int countOpenCloseBrace, ref int indexFunc)
        {
            string parentCase = "case";
            string paternReturn = "return";

            Regex regex = new Regex(parentCase);
            Match match = regex.Match(allLines[i]);
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
                    allLines.Insert(i, new string('\t', countOpenCloseBrace) + s);
                    allLines.Insert(i + 1, new string('\t', countOpenCloseBrace) + "{");
                    allLines.Insert(i + 2, new string('\t', countOpenCloseBrace + 1) + String.Format("_akvs_probe(\"{0}:{1}\",'f','o');", indexFile, indexFunc));
                    allLines.Insert(i + 3, new string('\t', countOpenCloseBrace + 1) + sa);
                    allLines.Insert(i + 4, new string('\t', countOpenCloseBrace) + "}");
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
                    allLines.Insert(i + 1, new string('\t', countOpenCloseBrace) + "{");
                    allLines.Insert(i + 2, new string('\t', countOpenCloseBrace + 1) + String.Format("_akvs_probe(\"{0}:{1}\",'f','o');", indexFile, indexFunc));
                    allLines.Insert(i + 3, new string('\t', countOpenCloseBrace + 1) + sa);
                    allLines.Insert(i + 4, new string('\t', countOpenCloseBrace) + "}");
                    allLines.Insert(i + 5, "");
                    i += 5;
                }
            }
        }

        private void insertMarkerDefault(List<string> allLines, ref int i, ref int countOpenCloseBrace, ref int indexFunc)
        {
            string parentDefault = "default";
            string paternReturn = "return";

            Regex regex = new Regex(parentDefault);
            Match match = regex.Match(allLines[i]);
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
                    allLines.Insert(i, new string('\t', countOpenCloseBrace) + s);
                    allLines.Insert(i + 1, new string('\t', countOpenCloseBrace) + "{");
                    allLines.Insert(i + 2, new string('\t', countOpenCloseBrace + 1) + String.Format("_akvs_probe(\"{0}:{1}\",'f','o');", indexFile, indexFunc));
                    allLines.Insert(i + 3, new string('\t', countOpenCloseBrace + 1) + sa);
                    allLines.Insert(i + 4, new string('\t', countOpenCloseBrace) + "}");
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
                    allLines.Insert(i + 1, new string('\t', countOpenCloseBrace) + "{");
                    allLines.Insert(i + 2, new string('\t', countOpenCloseBrace + 1) + String.Format("_akvs_probe(\"{0}:{1}\",'f','o');", indexFile, indexFunc));
                    allLines.Insert(i + 3, new string('\t', countOpenCloseBrace + 1) + sa);
                    allLines.Insert(i + 4, new string('\t', countOpenCloseBrace) + "}");
                    allLines.Insert(i + 5, "");
                    i += 5;
                }
            }
        }
    }
}
