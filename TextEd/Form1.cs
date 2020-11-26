using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Data.SqlTypes;

namespace TextEd
{
   
    public partial class Form1 : Form
    {
        string fileName, path;
        int count = 1,flag=0,space_count=0;
        char last_char='!',second_char='!';
        
        

        public Form1()
        {
            InitializeComponent();
        }
        private void nEWToolStripMenuItem_Click(object sender, EventArgs e)//new button
        {
            Form1 f = new Form1();
            f.Show();
        }

        private void eXITToolStripMenuItem_Click(object sender, EventArgs e)//exit button
        {
            this.Close();
        }

        private void sAVEToolStripMenuItem_Click(object sender, EventArgs e) // save file
        {

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "cpp files (*.cpp)|*.cpp|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {

                path = saveFileDialog1.FileName;
                byte[] plainDataArray = ASCIIEncoding.ASCII.GetBytes(richTextBox1.Text);
                using (var fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    fileStream.Write(plainDataArray, 0, plainDataArray.GetLength(0));
                    label1.Text = path;

                }
            }
        }//END

        private void sAVEToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string message = "Do you want to save changes to cuurent file";
            string title = "Save Changes";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons);
            if (result == DialogResult.Yes)
            {
                string str = richTextBox1.Text;
                System.IO.File.WriteAllText(label1.Text, str);
            }
            else
            {
                // Do something  
            }
            
        }
        private void oPENToolStripMenuItem_Click(object sender, EventArgs e)// HOW TO OPEN FILE
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                //string fileName;

                fileName = dlg.FileName;
                if (File.Exists(fileName) == true)
                {
                    StreamReader objreader = new StreamReader(fileName);
                    richTextBox1.Text = objreader.ReadToEnd();
                    label1.Text = fileName;
                    int n = richTextBox1.Lines.Count();
                    int count = 1;
                    this.richTextBox2.Text = "";
                    for (int i=0;i<n;i++)
                    {
                        this.richTextBox2.Text += (count.ToString()) + Environment.NewLine;
                        count++;
                    }
                    recolor(); 
                    objreader.Close();

                }
                else
                {
                    //msg file not exist
                }
            }

        }//END

        private void fILEToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        

        private void eXCUTEToolStripMenuItem_Click(object sender, EventArgs e)//excute  code
        {
            string command ="";
            string dir = label1.Text;

            if(dir== "untitled")
            {
                string message = "File is not saved";
                string title = "Error";
                MessageBox.Show(message, title);
            }
            else
            {
                string[] folder = dir.Split('.');
                if (folder[1] == "cpp")
                {
                    command = command + "g++ " + dir + " -o " + folder[0] + "&";
                    command = command + folder[0] + ".exe";
                }
                else if (folder[1] == "py")
                {
                    command = command + "python " + dir;
                }
                else
                {
                    command = command + "node " + dir;
                }
                System.Diagnostics.Process.Start("CMD.exe", "/K" + command);
            }
        }

        

        // color change
        
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            //do something 
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public char nextchar(char ch)
        {
            char chr='!';
            Dictionary<char, char> brackets = new Dictionary<char, char>();
            brackets.Add('(', ')');
            brackets.Add('[', ']');
            //brackets.Add('{', '}');
            //brackets.Add('<', '>'); removed due to inconvience in cout<< 
            brackets.Add('"', '"');

            foreach (KeyValuePair<char, char> kvp in brackets)
            {
                if(kvp.Key==ch)
                {
                    chr = kvp.Value;
                }
            }
            return chr; 
        }

        private void rUNToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        public void bracket(char ch)
        {
            char close = nextchar(ch);
            int lenth_befor_brack= richTextBox1.SelectionStart;
            if(close!='!')
            {
                richTextBox1.Text = this.richTextBox1.Text.Substring(0, lenth_befor_brack) + close + this.richTextBox1.Text.Substring(lenth_befor_brack);
                richTextBox1.SelectionStart = lenth_befor_brack;
                richTextBox1.Focus();
            }

        }
        //line number display and auto closing of brakets
        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar==(char)13)
            {
                this.richTextBox2.Text += (count.ToString()) + Environment.NewLine;
                count++;
                space_manager(); 
            }
            bracket(e.KeyChar);
            if (flag == 0)
            {
                this.richTextBox2.Text += (count.ToString()) + Environment.NewLine;
                count++;
                flag = 1;
                
            }
            if (label1.Text != "untitled")
            {
                recolor();
            }
            
        }
        //space management
        public void space_manager()
        {
            int prv_space_count = space_count;
            string prv_space_str = "";
            space_count = 0;
            int totalLines = this.richTextBox1.Lines.Length;
            string lastLine = "", space_str="",words="";
            int index = richTextBox1.SelectionStart;
            int line = richTextBox1.GetLineFromCharIndex(index);
            lastLine = richTextBox1.Lines[line - 1];
            words = lastLine;
            int lenth_befor_brack = richTextBox1.SelectionStart;
            for (int i=0;i<lastLine.Length;i++)
            {
                if (lastLine[i] != ' ')
                {
                    break;
                }
                space_count++;
            }
            for (int i = 0; i < space_count; i++)
            {
                space_str += " ";
            }
            for(int i=0;i<prv_space_count;i++)
            {
                prv_space_str += " ";
            }
            if(lastLine[lastLine.Length-1]=='{')
            {
                space_str += "     "+"\n"+prv_space_str+"}";
                space_count += 5;
            }
            richTextBox1.Text = this.richTextBox1.Text.Substring(0, lenth_befor_brack) + space_str + this.richTextBox1.Text.Substring(lenth_befor_brack);
            richTextBox1.SelectionStart = lenth_befor_brack+space_count;
            richTextBox1.Focus();
        }

        //color
        public void recolor()
        {

            Dictionary<string, int> keywords = new Dictionary<string, int>();
            keywords.Add("auto", 1);
            keywords.Add("bool", 1);
            keywords.Add("char", 1);
            keywords.Add("double", 1);
            keywords.Add("enum", 1);
            keywords.Add("float", 1);
            keywords.Add("int", 1);
            keywords.Add("long", 1);
            keywords.Add("short", 1);
            keywords.Add("template", 1);
            keywords.Add("union", 1);
            keywords.Add("wchar_t", 1);
            keywords.Add("vector", 1);
            keywords.Add("string", 1);
            keywords.Add("list", 1);

            //
            keywords.Add("asm", 2);
            keywords.Add("const", 2);
            keywords.Add("default", 2);
            keywords.Add("return", 2);
            keywords.Add("delete", 2);
            keywords.Add("namespace", 2);
            keywords.Add("register", 2);

            //
            keywords.Add("class", 3);
            keywords.Add("explicit", 3);
            keywords.Add("export", 3);
            keywords.Add("extern", 3);
            keywords.Add("freind", 3);
            keywords.Add("inline", 3);
            keywords.Add("public", 3);
            keywords.Add("static", 3);
            keywords.Add("using", 3);
            keywords.Add("volatile", 3);

            //
            keywords.Add("main", 4);
            keywords.Add("switch", 4);
            keywords.Add("case", 4);
            keywords.Add("break", 4);
            keywords.Add("try", 4);
            keywords.Add("catch", 4);
            keywords.Add("do", 4);
            keywords.Add("while", 4);
            keywords.Add("for", 4);
            keywords.Add("if", 4);
            keywords.Add("else", 4);
            keywords.Add("continue", 4);
            keywords.Add("goto", 4);

            //
            keywords.Add("true", 5);
            keywords.Add("false", 5);
            keywords.Add("void", 5);
            keywords.Add("signed", 5);
            keywords.Add("NULL", 5);
            keywords.Add("sizeof", 5);
            keywords.Add("unsigned", 5);
            keywords.Add("typedef", 5);
            keywords.Add("typeid", 5);
            keywords.Add("typename", 5);



            string text = richTextBox1.Text;
            String[] strlist = text.Split(' ', (char)13,'\n','{','}','#',';','"','(',')','<','>');
            string last_word = strlist[strlist.Length - 1];
            int color = 0;
            for (int i = 0; i < strlist.Length - 1; i++)
            {
                last_word = strlist[i];
                if (!keywords.ContainsKey(last_word))
                {
                    HighlightLastLine(richTextBox1, last_word, Color.White, false);
                }
                else
                {
                    foreach (KeyValuePair<string, int> kvp in keywords)
                    {
                        if (kvp.Key == last_word)
                        {
                            color = kvp.Value;
                        }
                    }
                    if (color == 1)
                    {
                        HighlightLastLine(richTextBox1, last_word, Color.LightGreen, false);
                    }
                    else if (color == 2)
                    {
                        HighlightLastLine(richTextBox1, last_word, Color.DeepPink, false);
                    }
                    else if (color == 3)
                    {
                        HighlightLastLine(richTextBox1, last_word, Color.YellowGreen, false);
                    }
                    else if (color == 4)
                    {
                        HighlightLastLine(richTextBox1, last_word, Color.BlueViolet, false);
                    }
                    else if (color == 5)
                    {
                        HighlightLastLine(richTextBox1, last_word, Color.OrangeRed, false);
                    }
                    else 
                    {
                        HighlightLastLine(richTextBox1, last_word, Color.White, false);
                    }
                }
            }


        }

        public void HighlightLastLine(RichTextBox TextControl, string TextToHighlight, Color HighlightColor, bool ClearColors = true)
        {
            //TextControl.Text = TextControl.Text.Trim();
            int lenth_befor_brack = richTextBox1.SelectionStart;
            
            if (ClearColors)
            {
                TextControl.SelectionStart = 0;
                TextControl.SelectionLength = 0;
                TextControl.SelectionColor = Color.Black;
            }
            
            int LastLineStartIndex = richTextBox1.Text.LastIndexOf(TextToHighlight);
            if (LastLineStartIndex >= 0)
            {
                TextControl.SelectionStart = LastLineStartIndex;
                TextControl.SelectionLength = TextControl.Text.Length - 1;
                TextControl.SelectionColor = HighlightColor;
                TextControl.SelectionStart = 0;
                TextControl.SelectionLength = 0;
            }
            richTextBox1.SelectionStart = lenth_befor_brack;
            richTextBox1.Focus();
        }

    }
}
