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

namespace Tool_Project
{
    public partial class ClassEditor : Form
    {
        DragDropData dragData = new DragDropData();
        MapEditor m_mapEditor;
        int m_editIndex;

        public ClassEditor(MapEditor mapEditor)
        {
            InitializeComponent();

            m_mapEditor = mapEditor;
        }

        private void ClassEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Visible = false;
                Clear();
                e.Cancel = true;
                m_mapEditor.Focus();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!((CheckBox)sender).Checked)
            {
                movementCost.Enabled = false;
            }
            else
            {
                movementCost.Enabled = true;
            }
        }

        private bool ValidateFile(out string filename, out string ext, DragEventArgs e)
        {
            bool valid = false;
            string[] data = e.Data.GetData(DataFormats.FileDrop) as string[];

            filename = string.Empty;
            ext = string.Empty;

            if (data != null)
            {
                filename = data[0];
                ext = Path.GetExtension(filename);
                if (ext == ".jpg" || ext == ".png")
                {
                    valid = true;
                }
            }
            return valid;
        }

        private void ClassEditor_DragEnter(object sender, DragEventArgs e)
        {
            string filename;
            string ext;

            dragData.valid = ValidateFile(out filename, out ext, e);

            if (dragData.valid)
            {
                e.Effect = DragDropEffects.Copy;

                string temp = filename;
                string temp2 = System.IO.Directory.GetCurrentDirectory() + "\\";
                filename = temp.Replace(temp2, "");

                //if (Path.GetDirectoryName(filename) == System.IO.Directory.GetCurrentDirectory())
                //{
                //    filename = Path.GetFileName(filename);                        
                //}

                dragData.path = filename;
                dragData.ext = ext;
            }
                
        }

        private void ClassEditor_DragDrop(object sender, DragEventArgs e)
        {
            if (dragData.valid)
            {
                pictureBox1.Image = new Bitmap(dragData.path);
            }
        }

        private void Clear()
        {
            pictureBox1.Image = null;
            name.Clear();
            traversable.Checked = true;
            movementCost.Value = 0;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Clear();
            m_mapEditor.Focus();
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null && !string.IsNullOrWhiteSpace(name.Text))
            {
                m_mapEditor.CreateNewClass(pictureBox1.Image, dragData.path, name.Text, traversable.Checked, traversable.Checked ? (int)movementCost.Value : -1);

                this.Visible = false;
                Clear();

                m_mapEditor.Focus();
            }
            else
            {
                MessageBox.Show("Please enter a name and an Image", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void EditClass(GridNode node)
        {
            pictureBox1.Image = node.Image;
            name.Text = node.ClassName;
            traversable.Checked = node.Traversable;
            movementCost.Value = node.MovementCost;
            if (!traversable.Checked)
            {
                movementCost.Enabled = false;
            }

            m_editIndex = node.Index;
            dragData.path = node.Path;

            editButton.Visible = true;
            createButton.Visible = false;
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            m_mapEditor.EditClass(pictureBox1.Image, dragData.path, name.Text, traversable.Checked, (int)movementCost.Value, m_editIndex);

            editButton.Visible = false;
            createButton.Visible = true;

            this.Visible = false;
            Clear();

            m_mapEditor.Focus();
        }
    }
}
