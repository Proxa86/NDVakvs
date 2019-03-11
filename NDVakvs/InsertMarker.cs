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

            lParentFilters.Add(Directory.GetFiles(fbd.SelectedPath, "*.cpp", SearchOption.AllDirectories));
            lParentFilters.Add(Directory.GetFiles(fbd.SelectedPath, "*.cc", SearchOption.AllDirectories));
            lParentFilters.Add(Directory.GetFiles(fbd.SelectedPath, "*.c", SearchOption.AllDirectories));
            lParentFilters.Add(Directory.GetFiles(fbd.SelectedPath, "*.h", SearchOption.AllDirectories));
            lParentFilters.Add(Directory.GetFiles(fbd.SelectedPath, "*.hh", SearchOption.AllDirectories));
            lParentFilters.Add(Directory.GetFiles(fbd.SelectedPath, "*.hpp", SearchOption.AllDirectories));

            try
            {
                string paternOpen = "{";
                string paternClose = "}";
                string paternReturn = "return";
                string paternIf = "if";
                string paternSemicolon = ";";
                string parentComment = @"//";
                string parentCommentOpen = @"/\*";
                string parentCommentClose = @"\*/";
                string parentHeshTag = @"#";
                string parentStruct = "struct";
                string parentClass = "class";
                string parentNamespace = "namespace";
                bool flag = false;
                bool flagEndReturn = false;
                bool commentOpen = false;
                bool startAnalis = false;
                Regex regex;
                Match match;
                int n = 0;
                foreach (var filter in lParentFilters)
                {
                    
                    
                    foreach (var pathFile in filter)
                    {
                        int indexFile = 0;
                        int indexFunc = 0;
                        var allLines = File.ReadAllLines(pathFile).ToList();

                        for (int i = 0; i < allLines.Count; i++)
                        {
                            regex = new Regex(parentCommentOpen);
                            match = regex.Match(allLines[i]);
                            if(match.Success)
                            {
                                for(int j = i; j < allLines.Count; j++)
                                {
                                    regex = new Regex(parentCommentClose);
                                    match = regex.Match(allLines[j]);
                                    if (match.Success)
                                    {
                                        i = j + 1;
                                        break;
                                    }
                                    else continue;
                                }
                            }
                            //if (!commentOpen)
                            //{
                            //    regex = new Regex(parentCommentOpen);
                            //    match = regex.Match(allLines[i]);
                            //    if (match.Success)
                            //    {
                            //        commentOpen = true;
                            //        startAnalis = false;
                            //        continue;
                            //    }
                            //}
                            //if (commentOpen)
                            //{
                            //    regex = new Regex(parentCommentClose);
                            //    match = regex.Match(allLines[i]);
                            //    if (match.Success)
                            //    {
                            //        commentOpen = false;
                            //        startAnalis = true;

                            //    }
                            //    else if (!startAnalis)
                            //        continue;
                            //}
                            regex = new Regex(parentComment);
                            match = regex.Match(allLines[i]);
                            if (match.Success)
                            {
                                continue;
                            }
                            regex = new Regex(parentHeshTag);
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
                                    regex = new Regex(paternOpen);
                                    match = regex.Match(allLines[j]);
                                    if (match.Success)
                                    {
                                        i = j + 1;
                                        break;
                                    }
                                    else continue;
                                }
                            }

                            if (startAnalis & allLines[i] != "")
                            {
                                regex = new Regex(paternOpen);
                                match = regex.Match(allLines[i]);
                                if (match.Success)
                                {
                                    if (n == 0)
                                    {
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
                                    int indexEnd = allLines[i].IndexOf(")");
                                    regex = new Regex(paternReturn);
                                    match = regex.Match(allLines[i]);
                                    if (match.Success)
                                    {
                                        int indexStartReturn = match.Index;
                                        int indexEnd1 = allLines[i].IndexOf(";");
                                        string s = allLines[i].Substring(indexStartIf, indexEnd - indexStartIf + 1);
                                        string sa = allLines[i].Substring(indexStartReturn, indexEnd1 - indexStartReturn + 1);
                                        allLines.Remove(allLines[i]);
                                        allLines.Insert(i, "\t" + s);
                                        allLines.Insert(i + 1, "\t{");
                                        allLines.Insert(i + 2, String.Format("\t\t_akvs_probe(\"{0}:{1}\",'f','o')", indexFile, indexFunc));
                                        allLines.Insert(i + 3, "\t\t" + sa);
                                        allLines.Insert(i + 4, "\t}");
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
                                        allLines.Remove(allLines[i + 1]);
                                        allLines.Insert(i + 1, "\t{");
                                        allLines.Insert(i + 2, String.Format("\t\t_akvs_probe(\"{0}:{1}\",'f','o')", indexFile, indexFunc));
                                        allLines.Insert(i + 3, "\t\t" + sa);
                                        allLines.Insert(i + 4, "\t}");
                                        allLines.Insert(i + 5, "");
                                        i += 5;
                                    }
                                }
                                else
                                {
                                    regex = new Regex(paternReturn);
                                    match = regex.Match(allLines[i]);
                                    if (match.Success)
                                    {
                                        allLines.Insert(i, String.Format("\t_akvs_probe(\"{0}:{1}\",'f','o')", indexFile, indexFunc));
                                        ++i;
                                        if (n == 1)
                                        {
                                            flagEndReturn = true;
                                            ++indexFunc;
                                        }
                                            
                                    }
                                }

                                regex = new Regex(paternClose);
                                match = regex.Match(allLines[i]);
                                if (match.Success)
                                {
                                    --n;
                                    if (flagEndReturn)
                                    {
                                        flagEndReturn = false;
                                    }
                                    else if (n == 0)
                                    {
                                        allLines.Insert(i, String.Format("\t_akvs_probe(\"{0}:{1}\",'f','o')", indexFile, indexFunc));
                                        ++indexFunc;
                                        ++i;
                                    }
                                }
                            }
                        }
                        Console.WriteLine();
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
