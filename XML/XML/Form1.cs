using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace XML
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        XDocument XDoc = XDocument.Load("1.xml");
        XElement ElemSelect;
        string attr;
        private void ClearList()
        {
            listBox1.Items.Clear();
            listBox1.Items.Add("..");
        }
        private void SearchT(string search = null)
        {
            ClearList();
            foreach (XNode t in XDoc.Descendants(search).Elements())
            {
                listBox1.Items.Add(t.ToString() + "\r\n");  // вывод всех элементов
            }
        }
        private void SelectAll()
        {
            ClearList();
            if (XDoc.FirstNode != null)
            {
                listBox1.Items.Add(XDoc.FirstNode.ToString() + "\r\n");
            } else
            {
                MessageBox.Show("No matches");
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            SelectAll();
            textBox2.Text = XDoc.Root.Name.ToString();
            //listBox1.Items.Add(XDoc.Elements().ToString() + "\r\n");
            //XDoc.SelectSingleNode("//root/node");
            /*foreach (XElement t in XDoc.Descendants("."))
            {
                listBox1.Items.Add(t.ToString() + "\r\n");  // вывод всех элементов
            }*/
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ClearList();
            if (textBox1.Text != "")
            {
                //XDoc.Descendants().Where(i => i.Name == textBox1.Text);
                var search = XDoc.Descendants(textBox1.Text);
                foreach (var t in search)
                {
                    listBox1.Items.Add(t.ToString() + "\r\n"); 
                }
            }
            
            
        }
        private void lst_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = (int)e.Graphics.MeasureString(listBox1.Items[e.Index].ToString(), listBox1.Font, listBox1.Width).Height;
        }

        private void lst_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (listBox1.Items.Count > 0)
            {
                e.DrawBackground();
                e.DrawFocusRectangle();
                if (e.Index == -1) e.Graphics.DrawString(listBox1.Items[0].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds);
                else
                e.Graphics.DrawString(listBox1.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Text = "";
            comboBox2.Items.Clear();
            textBox3.Text = "";
            if (listBox1.Items[listBox1.SelectedIndex].ToString() == "..")
            {
                textBox2.Text = XDoc.Root.Name.ToString();
                foreach (var attr in XDoc.Descendants(textBox2.Text).Attributes())
                {
                    comboBox2.Items.Add(attr.Name.ToString());
                }
            }
            else
            {
                ElemSelect = XElement.Parse(listBox1.Items[listBox1.SelectedIndex].ToString());
                textBox2.Text = ElemSelect.Name.ToString();
                foreach (var attr in ElemSelect.Attributes())
                {
                    comboBox2.Items.Add(attr.Name.ToString());
                }
            }
            
        }

        

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox1.Items[listBox1.SelectedIndex].ToString() == "..")
            {
                SelectAll();
            }
            else
            {
                SearchT(ElemSelect.Name.ToString());
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string node = textBox2.Text;
            attr = comboBox2.Items[comboBox2.SelectedIndex].ToString();
            textBox3.Text = ElemSelect.Attribute(attr).Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XDoc.Descendants().Where(i => i.ToString() == ElemSelect.ToString()).Remove();
            SelectAll();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var w = XDoc.Descendants(ElemSelect.Name).Where(i => i.ToString() == ElemSelect.ToString());
            w.Attributes().Where(i => i.Name == attr).Remove();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var attrlist = XDoc.Descendants(ElemSelect.Name).Where(i => i.ToString() == ElemSelect.ToString()).Attributes().Where(i => i.Name == attr);
            foreach (var w in attrlist)
            {
                w.Value = textBox3.Text;
            }
            XDoc.Save("1.xml");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                //отмечен
                textBox4.Visible = true;
                textBox5.Visible = true;
                comboBox3.Visible = true;
                button5.Visible = true;
                button4.Visible = true;

            }
            else
            {
                textBox4.Visible = false;
                textBox5.Visible = false;
                comboBox3.Visible = false;
                button5.Visible = false;
                button4.Visible = false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            
            openFileDialog1.ShowDialog();
            string fullname = openFileDialog1.FileName;
            XDoc = XDocument.Load(fullname);
            SelectAll();
        }
    }
}
