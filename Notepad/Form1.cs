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
using Microsoft.Win32;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Linq.Expressions;

namespace Notepad
{
    public partial class Form1 : Form
    {
        float fontSize = 0;
        public string filename;
        public bool isFileChanged;

        public Form1()
        {
            InitializeComponent();

            Init();
        }

        public void Init()
        {
            filename = "";
            isFileChanged = false;
        }

        public void CreatNewDocument(object sender, EventArgs e)
        {
            SaveUnsavedFile();
            richTextBox1.Text = "";
            filename = "";
            isFileChanged = false;
            UpdateTextWithTitle();
        }

        public void OpenFile(object sender, EventArgs e)
        {
            SaveUnsavedFile();
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamReader sr = new StreamReader(openFileDialog1.FileName);
                    richTextBox1.Text = sr.ReadToEnd();
                    sr.Close();
                    filename = openFileDialog1.FileName;
                    isFileChanged = false;
                }
                catch
                {
                    MessageBox.Show("Unable to open the file");
                }
            }
            UpdateTextWithTitle();
        }
        public void SaveFile(string _filename)
        {
            if (_filename == "")
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    _filename = saveFileDialog1.FileName;
                }
            }
            try
            {
                StreamWriter sw = new StreamWriter(_filename + ".txt");
                sw.WriteLine(richTextBox1.Text);
                sw.Close();
                filename = _filename;
                isFileChanged = false;
            }
            catch
            {
                MessageBox.Show("Unable to save file");
            }
            UpdateTextWithTitle();
        }

        public void Save(object sender, EventArgs e)
        {
            SaveFile(filename);
        }
        public void SaveAs(object sender, EventArgs e)
        {
            SaveFile("");
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (!isFileChanged)
            {
                this.Text = this.Text.Replace('*', ' ');
                isFileChanged = true;
                this.Text = "*" + this.Text;
            }
        }

        public void UpdateTextWithTitle()
        {
            if (filename != "")
                this.Text = filename + " - Notepad";
            else this.Text = "Unknown - Notepad";
        }

        public void SaveUnsavedFile()
        {
            if (isFileChanged)
            {
                DialogResult result = MessageBox.Show("Do you want to save changes to a file?", "Save file", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {
                    SaveFile(filename);
                }
            }
        }
        public void CopyText()
        {
            Clipboard.SetText(richTextBox1.SelectedText);
        }
        public void CutText()
        {
            Clipboard.SetText(richTextBox1.SelectedText);
            richTextBox1.Text = richTextBox1.Text.Remove(richTextBox1.SelectionStart, richTextBox1.SelectionLength);
        }
        public void PasteText()
        {
            richTextBox1.Text = richTextBox1.Text.Substring(0, richTextBox1.SelectionStart) + Clipboard.GetText()
                + richTextBox1.Text.Substring(richTextBox1.SelectionStart, richTextBox1.Text.Length - richTextBox1.SelectionStart);
        }

        private void OnCopyClick(object sender, EventArgs e)
        {
            CopyText();
        }

        private void OnCutClick(object sender, EventArgs e)
        {
            CutText();
        }

        private void OnPasteClick(object sender, EventArgs e)
        {
            PasteText();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (isFileChanged)
            {
                DialogResult result = MessageBox.Show("Do you want to save changes to a file?", "Save file", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {
                    SaveFile(filename);
                }
                if (result == DialogResult.Cancel)
                { 
                    e.Cancel = true;
                }
            }
        }
        public void IncreasText()
        {
            if (richTextBox1.Text != string.Empty)
            {
                fontSize = richTextBox1.Font.Size;

                richTextBox1.Font = new System.Drawing.Font(richTextBox1.Text, fontSize + 1);
            }
        }
        public void DownscaledText()
        {
            if (richTextBox1.Text != string.Empty)
            {
                fontSize = richTextBox1.Font.Size;

                richTextBox1.Font = new System.Drawing.Font(richTextBox1.Text, fontSize - 1);
            }
        }

        private void Increase(object sender, EventArgs e)
        {
            IncreasText();
        }

        private void Downscale(object sender, EventArgs e)
        {
            DownscaledText();
        }

        private void buttonSearch(object sender, EventArgs e)
        {
            int index = 0;
            richTextBox1.SelectAll();
            richTextBox1.SelectionBackColor = Color.White;
            while (index < richTextBox1.Text.LastIndexOf(textBox1.Text)) 
            {
                richTextBox1.Find(textBox1.Text, index, richTextBox1.TextLength, RichTextBoxFinds.MatchCase);
                richTextBox1.SelectionBackColor = Color.Yellow;
                index += richTextBox1.Text.IndexOf(textBox1.Text, index);
            }
            if (index == 0)
            {
                MessageBox.Show("Can't find word " + "'" + textBox1.Text + "'", "Search");
                textBox1.Text = "";
                textBox1.Clear();
            }
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSearch(sender, e);
                e.SuppressKeyPress = true;
            }
        }
    }
}
