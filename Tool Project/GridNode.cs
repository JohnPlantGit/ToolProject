using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;


public class GridNode : PictureBox
{
    private int m_imageIndex = -1;
    private string m_className;
    private bool m_traversable;
    private int m_movementCost;
    private string m_path;

    public GridNode(int size)
    {
        BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        Size = new System.Drawing.Size(size, size);
        SizeMode = PictureBoxSizeMode.StretchImage;
    }
    public GridNode(Image image, string path, string className, bool traversable, int movementCost, int listSize)
    {
        m_className = className;
        m_traversable = traversable;
        m_movementCost = movementCost;
        m_path = path;

       Image = image;
       BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
       Name = m_className;
       Size = new System.Drawing.Size(listSize, listSize);
       SizeMode = PictureBoxSizeMode.StretchImage;
    }

    public int Index
    {
        get { return m_imageIndex; }
        set { m_imageIndex = value; }
    }
    public string ClassName
    {
        get { return m_className; }
        set { m_className = value; }
    }
    public bool Traversable
    {
        get { return m_traversable; }
        set { m_traversable = value; }
    }
    public int MovementCost
    {
        get { return m_movementCost; }
        set { m_movementCost = value; }
    }
    public string Path
    {
        get { return m_path; }
        set { m_path = value; }
    }



    public void Clear()
    {
        m_imageIndex = -1;
        Image = null;
    }

    public void Copy(GridNode other)
    {
        m_imageIndex = other.Index;
        m_className = other.ClassName;
        m_traversable = other.Traversable;
        m_movementCost = other.MovementCost;
        Image = other.Image;
    }
}

