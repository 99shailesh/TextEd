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
                    objreader.Close();

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

        

        // Auto completeation feature of brackets
        
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {   /*
            if(flag==0)
            {
                this.richTextBox2.Text += (count.ToString())+ Environment.NewLine;
                count++;
                flag = 1;
            }
            string currentText = this.richTextBox1.Text;
            char ch='!';
            second_char = ch;
            if(currentText.Length>3)
            {
               ch = currentText[currentText.Length - 1];//get last char
               second_char = currentText[currentText.Length - 3];
               last_char = ch;
            }
            char nxtchar = nextchar(ch);
            if (nxtchar!='!')
            {
                this.richTextBox1.Text += nxtchar;
                richTextBox1.SelectionStart = richTextBox1.Text.Length-1;
                richTextBox1.Focus();
            }
        */
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public char nextchar(char ch)
        {
            char chr='!';
            Dictionary<char, char> brackets = new Dictionary<char, char>();
            brackets.Add('(',')');
            brackets.Add('[', ']');
            brackets.Add('{', '}');
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
                //space_manager();
            }
            bracket(e.KeyChar);
            if (flag == 0)
            {
                this.richTextBox2.Text += (count.ToString()) + Environment.NewLine;
                count++;
                flag = 1;
            }
        }
        //space management
        public void space_manager()
        {
            int totalLines = this.richTextBox1.Lines.Length;
            string lastLine = "",space_str="";

            if (totalLines>0)
            {
                lastLine = this.richTextBox1.Lines[totalLines - 1];
            }
            for(int i=0;i<lastLine.Length;i++)
            {
                if(lastLine[i]==' ')
                {
                    break;
                }
                space_count++;
            }
            for(int i=0;i<space_count;i++)
            {
                space_str += " ";
            }
            if (second_char=='{')
            {
                space_str += "  ";
                richTextBox1.SelectionStart = richTextBox1.Text.Length - 1;
                richTextBox1.Focus();
                //this.richTextBox1.Text += "\n" + space_str;
                richTextBox1.Text=this.richTextBox1.Text.Substring(0,this.richTextBox1.Text.Length-1)+ space_str+"\n"+ this.richTextBox1.Text.Substring(this.richTextBox1.Text.Length - 1);
                richTextBox1.SelectionStart = richTextBox1.Text.Length - 2;
                richTextBox1.Focus();
            }
            else if(second_char==':')
            {

            }
            else
            {

            }

        }


    }
}
