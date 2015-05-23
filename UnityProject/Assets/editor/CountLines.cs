using UnityEngine;
using UnityEditor;
using System.Collections;

public class CountLines : EditorWindow
{
    System.Text.StringBuilder strStats;
    Vector2 scrollPosition = new Vector2(0, 0);

    struct File
    {
        public string name;
        public int nbLines;
        public int nbLinesWhitespace;
        public int nbLinesComments;
        public int nbLinesCode;

        public File(string name, int nbLines, int nbLinesWhitespace, int nbLinesComments)
        {
            this.name = name;
            this.nbLines = nbLines;
            this.nbLinesWhitespace = nbLinesWhitespace;
            this.nbLinesComments = nbLinesComments;
            this.nbLinesCode = nbLines - (nbLinesWhitespace + nbLinesComments);
        }
    }

    void OnGUI()
    {
        if (GUILayout.Button("Refresh"))
        {
            DoCountLines();
        }
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        EditorGUILayout.HelpBox(strStats.ToString(), MessageType.None);
        EditorGUILayout.EndScrollView();
    }


    [MenuItem("Custom/Count Lines")]
    public static void Init()
    {
        CountLines window = EditorWindow.GetWindow<CountLines>("Count Lines");
        window.Show();
        window.Focus();
        window.DoCountLines();
    }

    void DoCountLines()
    {
        Debug.Log("starting count");

        string strDir = System.IO.Directory.GetCurrentDirectory();
        strDir += @"/Assets";
        int iLengthOfRootPath = strDir.Length;
        ArrayList stats = new ArrayList();
        ProcessDirectory(stats, strDir);

        int iTotalNbLines = 0;
        int iTotalNbLinesWhitespace = 0;
        int iTotalNbLinesComments = 0;
        int iTotalNbLinesCode = 0;
        foreach (File f in stats)
        {
            iTotalNbLines += f.nbLines;
            iTotalNbLinesWhitespace += f.nbLinesWhitespace;
            iTotalNbLinesComments += f.nbLinesComments;
            iTotalNbLinesCode += f.nbLinesCode;
        }

        strStats = new System.Text.StringBuilder();
        strStats.Append("Number of Files: " + stats.Count + "\n");
        strStats.Append("Number of Lines: " + iTotalNbLines + "\n");
        strStats.Append("Number of Lines of Whitespace: " + iTotalNbLinesWhitespace + "\n");
        strStats.Append("Number of Lines of Comments: " + iTotalNbLinesComments + "\n");
        strStats.Append("Number of Lines of Code: " + iTotalNbLinesCode + "\n");
        strStats.Append("================\n");

        foreach (File f in stats)
        {
            strStats.Append(f.name.Substring(iLengthOfRootPath + 1, f.name.Length - iLengthOfRootPath - 1) + " --> Lines:" + f.nbLines + " White:" + f.nbLinesWhitespace + " Comments:"+f.nbLinesComments + " Code:" + f.nbLinesCode + "\n");
        }
    }

    static void ProcessDirectory(ArrayList stats, string dir)
    {
        // Get every c# file in the directory and process the file
        string[] strArrFiles = System.IO.Directory.GetFiles(dir, "*.cs");
        foreach (string strFileName in strArrFiles)
            ProcessFile(stats, strFileName);

        // Get every Java file in the directory and process the file
        strArrFiles = System.IO.Directory.GetFiles(dir, "*.js");
        foreach (string strFileName in strArrFiles)
            ProcessFile(stats, strFileName);

        // Get every sub directory and process the files and directories it contains
        string[] strArrSubDir = System.IO.Directory.GetDirectories(dir);
        foreach (string strSubDir in strArrSubDir)
            ProcessDirectory(stats, strSubDir);
    }

    static void ProcessFile(ArrayList stats, string filename)
    {
        // read in the files line by line
        System.IO.StreamReader reader = System.IO.File.OpenText(filename);
        int iLineCount = 0;
        int iLineCountWhitespace = 0;
        int iLineCountComments = 0;
        bool multiLineComment = false;

        string line;
        while (reader.Peek() >= 0)
        { 
            line = reader.ReadLine();
            ++iLineCount;

            //Pull out all the whitespace
            line = line.Replace("\t", "");
            line = line.Replace("\n", "");
            line = line.Replace(" ", "");
            if (line.Length == 0)
                ++iLineCountWhitespace;
            else if (line.Length >= 2)
            {
                if (line.Substring(0, 2) == "//")
                    ++iLineCountComments;
                if (line.Substring(0, 2) == "/*")
                    multiLineComment = true;
                if (multiLineComment)
                    ++iLineCountComments;
                if (line.Contains("*/"))
                    multiLineComment = false;
            }
            
        }
        stats.Add(new File(filename, iLineCount, iLineCountWhitespace, iLineCountComments));
        reader.Close();
    }
}