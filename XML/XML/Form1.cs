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
        XDocument xDoc1;
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
                xDoc1 = XDocument.Parse(listBox1.Items[listBox1.SelectedIndex].ToString());
                textBox2.Text =  xDoc1.Root.Name.ToString();
                foreach (var attr in xDoc1.Descendants(textBox2.Text).Attributes())
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
                xDoc1 = XDocument.Parse(listBox1.Items[listBox1.SelectedIndex].ToString());
                //listBox1.Items.Add(xDoc1.Root.Name.ToString());
                SearchT(xDoc1.Root.Name.ToString());
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            xDoc1 = XDocument.Parse(listBox1.Items[listBox1.SelectedIndex].ToString());
            string node = textBox2.Text;
            string attr = comboBox2.Items[comboBox2.SelectedIndex].ToString();
            textBox3.Text = xDoc1.Element(node).Attribute(attr).Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XElement Xe = XElement.Parse(listBox1.Items[listBox1.SelectedIndex].ToString());
            XDoc.Descendants().Where(i => i.ToString() == Xe.ToString()).Remove();
            SelectAll();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            XElement Xe = XElement.Parse(listBox1.Items[listBox1.SelectedIndex].ToString());
            string attr = comboBox2.Items[comboBox2.SelectedIndex].ToString();
            var w = XDoc.Descendants().Where(i => i.ToString() == Xe.ToString());
            foreach ( var r in w)
            {
                r.Attribute(attr).Remove();
            }
            
        }
    }
}
