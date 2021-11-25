﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fanuc_mirro
{
    public static class Methods
    {
        internal static CompleteMirror ReadLSFile(string text, bool mirrorWorkbook)
        {
            bool workbookFound = false;
            List<FileAndPath> resultPaths = new List<FileAndPath>();
            FileAndPath result = new FileAndPath();
            FileAndPath workbook = new FileAndPath();
            DirectoryInfo d = new DirectoryInfo(text);
            foreach (var file in d.GetFiles("*.ls"))
            {
                result = MirrorString(file,false);
                if (result == null)
                    return null;
                resultPaths.Add(result);
            }
            if (mirrorWorkbook)
            {
                foreach (var file in d.GetFiles("workbook.xvr"))
                {
                    workbookFound = true;
                    workbook = MirrorString(file,true);
                }
                if (!workbookFound)
                    MessageBox.Show("No Workbook.xvr found!");
            }

            CompleteMirror results;
            if (mirrorWorkbook)
                results = new CompleteMirror(resultPaths, workbook);
            else
                results = new CompleteMirror(resultPaths);
            return results ;
        }

        private static FileAndPath MirrorString(FileInfo file, bool isWorkbook)
        {
            string arg1, arg2, regex1, regex2;
            if (isWorkbook)
            {
                arg1 = "Y: ";
                arg2 = "P: ";
                regex1 = @"((?<=X.*)-[0-9]*\.[0-9]*)|((?<=X.*)[0-9]*\.[0-9]*)|((?<=X.*)-[0-9]*)|((?<=X.*)[0-9]*)|((?<=Y.*)-[0-9]*\.[0-9]*)|((?<=Y.*)[0-9]*\.[0-9]*)|((?<=Y.*)-[0-9]*)|((?<=Y.*)[0-9]*)|((?<=Z.*)-[0-9]*\.[0-9]*)|((?<=Z.*)[0-9]*\.[0-9]*)|((?<=Z.*)-[0-9]*)|((?<=Z.*)[0-9]*)";
                regex2 = @"((?<=W.*)-[0-9]*\.[0-9]*)|((?<=W.*)[0-9]*\.[0-9]*)|((?<=W.*)-[0-9]*)|((?<=W.*)[0-9]*)|((?<=P.*)-[0-9]*\.[0-9]*)|((?<=P.*)[0-9]*\.[0-9]*)|((?<=P.*)-[0-9]*)|((?<=P.*)[0-9]*)|((?<=R.*)-[0-9]*\.[0-9]*)|((?<=R.*)[0-9]*\.[0-9]*)|((?<=R.*)-[0-9]*)|((?<=R.*)[0-9]*)";
            }
            else
            {
                arg1 = "Y = ";
                arg2 = "W = ";
                regex1 = @"((?<=X.*\=.*)-[0-9]*\.[0-9]*)|((?<=X.*\=.*)[0-9]*\.[0-9]*)|((?<=X.*\=.*)-[0-9]*)|((?<=X.*\=.*)[0-9]*)|((?<=Y.*\=.*)-[0-9]*\.[0-9]*)|((?<=Y.*\=.*)[0-9]*\.[0-9]*)|((?<=Y.*\=.*)-[0-9]*)|((?<=Y.*\=.*)[0-9]*)|((?<=Z.*\=.*)-[0-9]*\.[0-9]*)|((?<=Z.*\=.*)[0-9]*\.[0-9]*)|((?<=Z.*\=.*)-[0-9]*)|((?<=Z.*\=.*)[0-9]*)";
                regex2 = @"((?<=W.*\=.*)-[0-9]*\.[0-9]*)|((?<=W.*\=.*)[0-9]*\.[0-9]*)|((?<=W.*\=.*)-[0-9]*)|((?<=W.*\=.*)[0-9]*)|((?<=P.*\=.*)-[0-9]*\.[0-9]*)|((?<=P.*\=.*)[0-9]*\.[0-9]*)|((?<=P.*\=.*)-[0-9]*)|((?<=P.*\=.*)[0-9]*)|((?<=R.*\=.*)-[0-9]*\.[0-9]*)|((?<=R.*\=.*)[0-9]*\.[0-9]*)|((?<=R.*\=.*)-[0-9]*)|((?<=R.*\=.*)[0-9]*)";
            }
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            string resultString ="";
            Regex regex;
            float X = 0, YInvert = 0, Z = 0, WInvert = 0, P =0,RInvert = 0;
            string currentLine = "";
            List<string> foundValues = new List<string>();
            var reader = new StreamReader(file.FullName);
            bool modifyLine = false;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (line.ToLower().Replace(" ","").Contains("config:"))
                {
                    regex = new Regex(@"^.*:\s*'", RegexOptions.IgnoreCase);
                    Match m = regex.Match(line);
                    string templine = m.ToString();
                    foundValues = new List<string>();
                    //regex = new Regex(@"((?<=CONFIG :..).(?=\ ))")|;
                    //regex = new Regex(@"((?<=CONFIG.*).(?=\ ))|((?<= CONFIG.*).(?=\,))");
                    string configString = (new Regex(@"(?<=')[\w\d-]*", RegexOptions.IgnoreCase)).Match(line.Replace(" ", "").Replace(",", "").Replace("\t", "")).ToString();
                    regex = new Regex(@"(\w|-\d|\d)", RegexOptions.IgnoreCase);
                    foreach (Match currentMatch in regex.Matches(configString))
                    {
                        foundValues.Add(currentMatch.ToString());
                    }
                    if (foundValues.Count != 6)
                    {
                        MessageBox.Show("Niewłaściwa liczba argumentów w konfiguracji osi", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return null;
                    }
                    RobotConf conf = new RobotConf(foundValues[0], foundValues[1], foundValues[2], int.Parse(foundValues[3]), int.Parse(foundValues[4]), int.Parse(foundValues[5]));
                    //if (conf.Arg1 == "N")
                    //    conf.Arg1 = "F";
                    //else if (conf.Arg1 == "F")
                    //    conf.Arg1 = "N";
                    if (conf.Arg4 != 0)
                        conf.Arg4 = -conf.Arg4;
                    if (conf.Arg5 != 0)
                        conf.Arg5 = -conf.Arg5;
                    if (conf.Arg6 != 0)
                        conf.Arg6 = -conf.Arg6;
                    currentLine = templine + conf.Arg1 + " " + conf.Arg2 + " " + conf.Arg3 + "," + conf.Arg4.ToString() + "," + conf.Arg5.ToString() + "," + conf.Arg6.ToString() + "',\n";
                    modifyLine = true;

                }
                if (line.Contains(arg1))
                {
                    foundValues = new List<string>();
                    regex = new Regex(regex1);
                    foreach (Match currentMatch in regex.Matches(line))
                    {
                        foundValues.Add(currentMatch.ToString());
                    }
                    foundValues = Methods.Filter(foundValues);
                    X = float.Parse(foundValues[0], CultureInfo.InvariantCulture);
                    YInvert = -float.Parse(foundValues[1], CultureInfo.InvariantCulture);
                    Z = float.Parse(foundValues[2], CultureInfo.InvariantCulture);
                    if (isWorkbook)
                        currentLine = "  X:   " + X.ToString(nfi) + "   Y:  " + YInvert.ToString(nfi) + "   Z:   " + Z.ToString(nfi)+"\n";
                    else
                        currentLine = "        X = " + X.ToString(nfi) + " mm,    Y = " +YInvert.ToString(nfi)+ " mm,    Z = " +Z.ToString(nfi)+ " mm,\n";
                    modifyLine = true;
                }
                if (line.Contains(arg2))
                {
                    foundValues = new List<string>();
                    regex = new Regex(regex2);
                    foreach (Match currentMatch in regex.Matches(line))
                    {
                        foundValues.Add(currentMatch.ToString());
                    }
                    foundValues = Methods.Filter(foundValues);
                    WInvert = -float.Parse(foundValues[0], CultureInfo.InvariantCulture);
                    P = float.Parse(foundValues[1], CultureInfo.InvariantCulture);
                    RInvert = -float.Parse(foundValues[2], CultureInfo.InvariantCulture);
                    if (isWorkbook)
                        currentLine = "  W:   " + WInvert.ToString(nfi) + "   P:  " + P.ToString(nfi) + "   R:   " + RInvert.ToString(nfi)+"</ARRAY>\n";
                    else
                        currentLine = "        W = " + (WInvert.ToString(nfi) + " deg,    P = " + P.ToString(nfi) + " deg,    R = " + RInvert.ToString(nfi) + " deg\n");
                    modifyLine = true;
                }
                if (!modifyLine)
                {
                    currentLine = line+"\n";
                }

                resultString = resultString + currentLine;
                modifyLine = false;
            }
            reader.Close();
            FileAndPath result = new FileAndPath(file,resultString);
            
            return result;
        }

        private static List<string> Filter(List<string> foundValues)
        {
            List<string> result = new List<string>();
            foreach (string element in foundValues.Where(item=>item != ""))
            {
                result.Add(element);
            }
            return result;
        }
    }
}
