using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace Tool_Project
{
    // The form that handles editting the map
    public partial class MapEditor : Form
    {
        const int m_gridAmount = 20;
        GridNode[,] m_grid = new GridNode[m_gridAmount, m_gridAmount];
        int m_gridSize = 30;
        int m_listSize = 45;
        string m_saveFile = null;
        bool m_unsaved = false;

        string path;
        GridNode creator;
        Point offset;

        ClassEditor m_classEditor;

        public MapEditor()
        {
            InitializeComponent();

            m_classEditor = new ClassEditor(this);

            for (int x = 0; x < m_gridAmount; x++)
            {
                for (int y = 0; y < m_gridAmount; y++)
                {
                    m_grid[x, y] = new GridNode(m_gridSize);
                    Grid.Controls.Add(m_grid[x, y]);
                }
            }

            for (int x = 0; x < m_gridAmount; x++)
            {
                for (int y = 0; y < m_gridAmount; y++)
                {
                    GridNode current = m_grid[x, y];
                    current.Location = new System.Drawing.Point(m_gridSize * x, m_gridSize * y);
                    current.Click += new EventHandler(GridNode_Click);
                }
            }

            creator = new GridNode(m_listSize);
            creator.Visible = false;
            this.Controls.Add(creator);
        }

        // -------------
        // Checks the files in DragEventArgs' paths for their extension
        // Params:
        //      out:Filename will return the path of the file
        //      out:ext will return the extension of the file
        //      e The event args from the drag and drop
        // Return if the file has a valid extension
        // -------------
        private bool ValidateFile(out string filename, out string ext, DragEventArgs e)
        {
            bool valid = false;
            Array data = e.Data.GetData(DataFormats.FileDrop) as Array;
            

            filename = string.Empty;
            ext = String.Empty;

            if (data != null)
            {
                filename = ((string[])data)[0];
                for (int i = 0; i < filename.Length; i++)
                {
                    if (filename[i] == '.')
                    {
                        for (int j = i; j < filename.Length; j++)
                        {
                            ext += filename[j];
                        }
                        break;
                    }
                }
                if (ext == ".jpg" || ext == ".png" || ext == ".xml")
                {
                    valid = true;
                }
            }
            return valid;
        }

        // -------------
        // Is run when a class in the list is clicked
        // Copies the values of the class onto the creator
        // and set's up the creator
        // -------------
        private void GridImageList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                creator.Copy((GridNode)sender);

                offset.X = m_listSize / 2;
                offset.Y = m_listSize / 2;

                Point mPos = PointToClient(MousePosition);
                mPos.X -= offset.X;
                mPos.Y -= offset.Y;
                creator.Location = mPos;

                creator.BringToFront();
                creator.Visible = true;
            }
        }

        // -------------
        // Occurs when the mouse button is let go after selecting a class in the list
        // Copies the values of the creator onto the current tile on the map that the mouse is over
        // -------------
        private void Create(object sender, MouseEventArgs e)
        {
            creator.Visible = false;

            for (int x = 0; x < m_gridAmount; x++)
            {
                for (int y = 0; y < m_gridAmount; y++)
                {
                    if (m_grid[x, y].ClientRectangle.Contains(m_grid[x, y].PointToClient(Cursor.Position)))
                    {
                        m_grid[x, y].Copy(creator);

                        m_unsaved = true;
                        return;
                    }
                }
            }

        }

        // -------------
        // Occurs when the save tool strip is clicked
        // Creates an xml of the map and the classes
        // Lets the user choose the save location and name with the Save file dialog only if the map has not previously been saved
        // -------------
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XmlDocument doc = CreateXml();

            if (m_saveFile != null)
            {
                doc.Save(m_saveFile);
            }
            else
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "XML File (*.xml)|*.xml";
                saveFileDialog.DefaultExt = "xml";
                saveFileDialog.AddExtension = true;
                saveFileDialog.ShowDialog();

                if (saveFileDialog.FileName != "")
                {
                    FileStream fs = (FileStream)saveFileDialog.OpenFile();
                    m_saveFile = fs.Name;

                    doc.Save(fs);
                    fs.Close();
                }
            }
            m_unsaved = false;
        }

        // -------------
        // Occurs when the open tool strip is clicked
        // Allows the user to choose an xml to load
        // Warns the user if there is unsaved work
        // -------------
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML File (*.xml)|*.xml";
            openFileDialog.ShowDialog();

            if (openFileDialog.FileName != "")
            {
                if (m_unsaved)
                {
                    DialogResult dialogResult = MessageBox.Show("There is unsaved work open. Do you want to save changes?", "Close Form",
                       MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                    if (dialogResult == DialogResult.Yes)
                    {
                        XmlDocument saveDoc = CreateXml();                        

                        if (m_saveFile != null)
                        {
                            saveDoc.Save(m_saveFile);
                        }
                        else
                        {
                            SaveFileDialog saveFileDialog = new SaveFileDialog();
                            saveFileDialog.Filter = "XML File (*.xml)|*.xml";
                            saveFileDialog.DefaultExt = "xml";
                            saveFileDialog.AddExtension = true;
                            saveFileDialog.ShowDialog();

                            if (saveFileDialog.FileName != "")
                            {
                                FileStream fsSave = (FileStream)saveFileDialog.OpenFile();
                                m_saveFile = fsSave.Name;

                                saveDoc.Save(fsSave);
                                fsSave.Close();
                            }
                        }
                    }
                    else if (dialogResult == DialogResult.Cancel)
                    {
                        return;
                    }
                }

                FileStream fs = (FileStream)openFileDialog.OpenFile();

                m_saveFile = fs.Name;

                XmlDocument doc = new XmlDocument();
                doc.Load(fs);

                Clear();

                LoadXml(doc);
                
                fs.Close();
                m_unsaved = false;
            }
        }

        // -------------
        // Occurs if a file has been dragged onto the form
        // Validates the file
        // -------------
        private void MapEditor_DragEnter(object sender, DragEventArgs e)
        {
            string filename;
            string ext;

            if (ValidateFile(out filename, out ext, e) && ext == ".xml")
            {
                path = filename;
                e.Effect = DragDropEffects.Copy;
            }
        }

        // -------------
        // Occurs when a file is dropped onto the form
        // Will load the xml file
        // -------------
        private void MapEditor_DragDrop(object sender, DragEventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(path);
            }
            catch (Exception)
            {
                return;
            }

            Clear();

            LoadXml(doc);

            m_saveFile = path;

            m_unsaved = false;
        }

        // -------------
        // Responsible for moving the creator after the mouse
        // -------------
        private void Update_Tick(object sender, EventArgs e)
        {

            if (creator.Visible)
            {
                Point mPos = PointToClient(MousePosition);
                mPos.X -= offset.X;
                mPos.Y -= offset.Y;

                creator.Location = mPos;
            }
        }

        // -------------
        // Clears all the values in the form
        // -------------
        private void Clear()
        {
            for (int x = 0; x < m_gridAmount; x++)
            {
                for (int y = 0; y < m_gridAmount; y++)
                {
                    m_grid[x, y].Clear();
                }
            }
            imageList1.Images.Clear();
            GridTypeList.Controls.Clear();
        }

        // -------------
        // Occurs when the new tool strip is clicked
        // Clears the form
        // Warns the user if there is unsaved work
        // -------------
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_unsaved)
            {
                DialogResult dialogResult = MessageBox.Show("There is unsaved work open. Do you want to save changes?", "Close Form",
                   MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                {
                    XmlDocument doc = new XmlDocument();
                    XmlElement project = doc.CreateElement("Project");
                    doc.AppendChild(project);

                    XmlElement list = doc.CreateElement("List");
                    project.AppendChild(list);

                    for (int i = 0; i < GridTypeList.Controls.Count; i++)
                    {
                        XmlElement item = doc.CreateElement("Item");

                        string path = GridTypeList.Controls[i].Tag as string;
                        XmlText filename = doc.CreateTextNode(path);

                        item.AppendChild(filename);
                        list.AppendChild(item);
                    }

                    XmlElement grid = doc.CreateElement("Grid");
                    project.AppendChild(grid);

                    for (int x = 0; x < m_gridAmount; x++)
                    {
                        for (int y = 0; y < m_gridAmount; y++)
                        {
                            if (m_grid[x, y].Image != null)
                            {
                                XmlElement gridNode = doc.CreateElement("GridNode"/* + x.ToString() + "/" + y.ToString()*/);

                                XmlElement imageIndex = doc.CreateElement("Index");
                                XmlText imageIndexText = doc.CreateTextNode(m_grid[x, y].Index.ToString());

                                XmlElement xIndex = doc.CreateElement("X");
                                XmlText xIndexText = doc.CreateTextNode(x.ToString());

                                XmlElement yIndex = doc.CreateElement("Y");
                                XmlText yIndexText = doc.CreateTextNode(y.ToString());

                                gridNode.AppendChild(imageIndex);
                                imageIndex.AppendChild(imageIndexText);

                                gridNode.AppendChild(xIndex);
                                xIndex.AppendChild(xIndexText);

                                gridNode.AppendChild(yIndex);
                                yIndex.AppendChild(yIndexText);

                                grid.AppendChild(gridNode);
                            }
                        }
                    }

                    if (m_saveFile != null)
                    {
                        doc.Save(m_saveFile);
                    }
                    else
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Filter = "XML File (*.xml)|*.xml";
                        saveFileDialog.DefaultExt = "xml";
                        saveFileDialog.AddExtension = true;
                        saveFileDialog.ShowDialog();

                        if (saveFileDialog.FileName != "")
                        {
                            FileStream fs = (FileStream)saveFileDialog.OpenFile();
                            m_saveFile = fs.Name;

                            doc.Save(fs);
                            fs.Close();
                        }
                    }
                    Clear();
                    m_saveFile = null;
                    m_unsaved = false;
                }
                else if (dialogResult == DialogResult.No)
                {
                    Clear();
                    m_saveFile = null;
                    m_unsaved = false;
                }
            }
            else
            {
                Clear();
                m_saveFile = null;
                m_unsaved = false;
            }
            
        }

        // -------------
        // Occurs when a map tile is clicked
        // Removes the data in the map tile
        // -------------
        private void GridNode_Click(object sender, EventArgs e)
        {
            ((GridNode)sender).Clear();
            m_unsaved = true;
        }

        // -------------
        // Occurs when the save tool strip is clicked
        // allows the user to pick a location to save the map
        // -------------
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML File (*.xml)|*.xml";
            saveFileDialog.DefaultExt = "xml";
            saveFileDialog.AddExtension = true;
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                FileStream fs = (FileStream)saveFileDialog.OpenFile();
                m_saveFile = fs.Name;

                XmlDocument doc = CreateXml();

                doc.Save(fs);
                fs.Close();
                m_unsaved = false;
                m_saveFile = saveFileDialog.FileName;
            }
        }

        // -------------
        // Allows the user to save unsaved work before closing
        // -------------
        private void MapEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_unsaved)
            {
                DialogResult dialogResult = MessageBox.Show("There is unsaved work open. Do you want to save changes?", "Close Form",
                   MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                {
                    XmlDocument doc = new XmlDocument();
                    XmlElement project = doc.CreateElement("Project");
                    doc.AppendChild(project);

                    XmlElement list = doc.CreateElement("List");
                    project.AppendChild(list);

                    for (int i = 0; i < GridTypeList.Controls.Count; i++)
                    {
                        XmlElement item = doc.CreateElement("Item");

                        string path = GridTypeList.Controls[i].Tag as string;
                        XmlText filename = doc.CreateTextNode(path);

                        item.AppendChild(filename);
                        list.AppendChild(item);
                    }

                    XmlElement grid = doc.CreateElement("Grid");
                    project.AppendChild(grid);

                    for (int x = 0; x < m_gridAmount; x++)
                    {
                        for (int y = 0; y < m_gridAmount; y++)
                        {
                            if (m_grid[x, y].Image != null)
                            {
                                XmlElement gridNode = doc.CreateElement("GridNode"/* + x.ToString() + "/" + y.ToString()*/);

                                XmlElement imageIndex = doc.CreateElement("Index");
                                XmlText imageIndexText = doc.CreateTextNode(m_grid[x, y].Index.ToString());

                                XmlElement xIndex = doc.CreateElement("X");
                                XmlText xIndexText = doc.CreateTextNode(x.ToString());

                                XmlElement yIndex = doc.CreateElement("Y");
                                XmlText yIndexText = doc.CreateTextNode(y.ToString());

                                gridNode.AppendChild(imageIndex);
                                imageIndex.AppendChild(imageIndexText);

                                gridNode.AppendChild(xIndex);
                                xIndex.AppendChild(xIndexText);

                                gridNode.AppendChild(yIndex);
                                yIndex.AppendChild(yIndexText);

                                grid.AppendChild(gridNode);
                            }
                        }
                    }

                    if (m_saveFile != null)
                    {
                        doc.Save(m_saveFile);
                    }
                    else
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Filter = "XML File (*.xml)|*.xml";
                        saveFileDialog.DefaultExt = "xml";
                        saveFileDialog.AddExtension = true;
                        saveFileDialog.ShowDialog();

                        if (saveFileDialog.FileName != "")
                        {
                            FileStream fs = (FileStream)saveFileDialog.OpenFile();
                            m_saveFile = fs.Name;

                            doc.Save(fs);
                            fs.Close();
                        }
                    }
                    m_unsaved = false;
                }
                else if (dialogResult == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        // -------------
        // Occurs when the new class button is clicked
        // opens the Class editor form
        // -------------
        private void newClass_Click(object sender, EventArgs e)
        {
            m_classEditor.Visible = true;
        }

        // -------------
        // Creates a new class type in the list
        // Params:
        //      Image the class's display picture
        //      Path: the path to the image
        //      Name: the name of the class
        //      Traversable: if the class can be traversed
        //      MovementCost: the cost of moving over the class
        // -------------
        public void CreateNewClass(Image image, string path, string name, bool traversable, int movementCost)
        {
            GridNode node = new GridNode(image, path, name, traversable, movementCost, m_listSize);
            node.MouseDown += new MouseEventHandler(GridImageList_MouseDown);
            node.MouseUp += new MouseEventHandler(Create);
            node.ContextMenuStrip = GridListMenu;

            GridTypeList.Controls.Add(node);
            node.Index = GridTypeList.Controls.IndexOf(node);

            m_unsaved = true;
        }

        // -------------
        // Edits an existing class
        // Params:
        //      Same as CreateNewClass
        //      Index: the index of the class being edited
        // -------------
        public void EditClass(Image image, string path, string name, bool traversable, int movementCost, int index)
        {
            GridNode current = ((GridNode)GridTypeList.Controls[index]);

            current.Image = image;
            current.Path = path;
            current.Name = name;
            current.Traversable = traversable;
            current.MovementCost = movementCost;

            for (int x = 0; x < m_gridAmount; x++)
            {
                for (int y = 0; y < m_gridAmount; y++)
                {
                    if (m_grid[x,y].Index == index)
                    {
                        m_grid[x, y].Copy(current);
                    }
                }
            }

            m_unsaved = true;
        }

        // -------------
        // Creates an xml using the data in the form
        // -------------
        private XmlDocument CreateXml()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement project = doc.CreateElement("Project");
            doc.AppendChild(project);

            XmlElement list = doc.CreateElement("List");
            project.AppendChild(list);

            for (int i = 0; i < GridTypeList.Controls.Count; i++)
            {
                XmlElement item = doc.CreateElement("Item");

                XmlElement path = doc.CreateElement("Path");
                XmlText pathText = doc.CreateTextNode(((GridNode)GridTypeList.Controls[i]).Path);
                path.AppendChild(pathText);
                item.AppendChild(path);

                XmlElement className = doc.CreateElement("ClassName");
                XmlText classNameText = doc.CreateTextNode(((GridNode)GridTypeList.Controls[i]).ClassName);
                className.AppendChild(classNameText);
                item.AppendChild(className);

                XmlElement traversable = doc.CreateElement("Traversable");
                XmlText traversableText = doc.CreateTextNode(((GridNode)GridTypeList.Controls[i]).Traversable.ToString());
                traversable.AppendChild(traversableText);
                item.AppendChild(traversable);

                XmlElement movementCost = doc.CreateElement("MovementCost");
                XmlText movementCostText = doc.CreateTextNode(((GridNode)GridTypeList.Controls[i]).MovementCost.ToString());
                movementCost.AppendChild(movementCostText);
                item.AppendChild(movementCost);

                list.AppendChild(item);
            }

            XmlElement grid = doc.CreateElement("Grid");
            project.AppendChild(grid);

            for (int x = 0; x < m_gridAmount; x++)
            {
                for (int y = 0; y < m_gridAmount; y++)
                {
                    if (m_grid[x, y].Image != null)
                    {
                        XmlElement gridNode = doc.CreateElement("GridNode"/* + x.ToString() + "/" + y.ToString()*/);

                        XmlElement imageIndex = doc.CreateElement("Index");
                        XmlText imageIndexText = doc.CreateTextNode(m_grid[x, y].Index.ToString());
                        gridNode.AppendChild(imageIndex);
                        imageIndex.AppendChild(imageIndexText);

                        XmlElement xIndex = doc.CreateElement("X");
                        XmlText xIndexText = doc.CreateTextNode(x.ToString());
                        gridNode.AppendChild(xIndex);
                        xIndex.AppendChild(xIndexText);

                        XmlElement yIndex = doc.CreateElement("Y");
                        XmlText yIndexText = doc.CreateTextNode(y.ToString());
                        gridNode.AppendChild(yIndex);
                        yIndex.AppendChild(yIndexText);

                        grid.AppendChild(gridNode);
                    }
                }
            }
            return doc;
        }

        // -------------
        // Load data from an xml into the form
        // -------------
        private void LoadXml(XmlDocument doc)
        {
            XmlNode list = doc.FirstChild.FirstChild;

            foreach (XmlElement item in list.ChildNodes)
            {
                GridNode newNode = new GridNode(m_listSize);

                newNode.Path = item.ChildNodes[0].InnerText;
                newNode.ClassName = item.ChildNodes[1].InnerText;
                newNode.Traversable = bool.Parse(item.ChildNodes[2].InnerText);
                newNode.MovementCost = int.Parse(item.ChildNodes[3].InnerText);


                imageList1.Images.Add(new Bitmap(newNode.Path));

                newNode.Image = imageList1.Images[imageList1.Images.Count - 1];
                newNode.MouseDown += new MouseEventHandler(GridImageList_MouseDown);
                newNode.MouseUp += new MouseEventHandler(Create);
                newNode.ContextMenuStrip = GridListMenu;

                GridTypeList.Controls.Add(newNode);
                newNode.Index = GridTypeList.Controls.Count - 1;
            }

            XmlNode grid = doc.FirstChild.ChildNodes[1];

            foreach (XmlElement gridNode in grid.ChildNodes)
            {
                XmlElement imageIndex = gridNode.ChildNodes[0] as XmlElement;
                int index = int.Parse(imageIndex.InnerText);

                XmlElement xIndex = gridNode.ChildNodes[1] as XmlElement;
                int x = int.Parse(xIndex.InnerText);

                XmlElement yIndex = gridNode.ChildNodes[2] as XmlElement;
                int y = int.Parse(yIndex.InnerText);

                m_grid[x, y].Copy(((GridNode)GridTypeList.Controls[index]));
            }
        }

        // -------------
        // Occurs when edit is picked in a list item's context menu
        // Opens the class editor and copies the values of the clicked item into it
        // -------------
        private void GridListEdit_Click(object sender, EventArgs e)
        {
            m_classEditor.Visible = true;
            GridNode owner = ((GridNode)((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl);
            m_classEditor.EditClass(owner);
        }

        // -------------
        // Occurs when delete is picked in a list item's context menu
        // Deletes the selected item from the list
        // Removes all references to the item from the map
        // Updates the index of the rest of the items
        // -------------
        private void GridListDelete_Click(object sender, EventArgs e)
        {
            GridNode owner = ((GridNode)((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl);
            int index = GridTypeList.Controls.IndexOf(owner);
            GridTypeList.Controls.Remove(owner);

            for (int x = 0; x < m_gridAmount; x++)
            {
                for (int y = 0; y < m_gridAmount; y++)
                {
                    if (m_grid[x,y].Index == index)
                    {
                        m_grid[x, y].Clear();
                    }
                    if (m_grid[x,y].Index > index)
                    {
                        m_grid[x, y].Index--;
                    }
                }
            }
            for (int i = index + 1; i < GridTypeList.Controls.Count; i++)
            {
                ((GridNode)GridTypeList.Controls[i]).Index--;
            }
        }
    }
}
