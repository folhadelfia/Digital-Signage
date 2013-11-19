using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assemblies.XMLSerialization.Drawing
{
    #region Space
    public struct XMLSize
    {
        //http://msdn.microsoft.com/en-us/library/system.drawing.size.aspx

        private int height, width;

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public bool IsEmpty
        {
            get { return Height == 0 && Width == 0; }
        }
        public XMLSize(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        #region Operadores binários
        public static XMLSize operator +(XMLSize s1, XMLSize s2)
        {
            return new XMLSize() { Height = s1.Height + s2.Height, Width = s1.Width + s2.Width };
        }
        public static XMLSize operator -(XMLSize s1, XMLSize s2)
        {
            return new XMLSize() { Height = s1.Height - s2.Height, Width = s1.Width - s2.Width };
        }
        public static bool operator >(XMLSize s1, XMLSize s2)
        {
            return ((s1.Height * s1.Width) > (s2.Height * s2.Width));
        }
        public static bool operator >=(XMLSize s1, XMLSize s2)
        {
            return ((s1.Height * s1.Width) >= (s2.Height * s2.Width));
        }
        public static bool operator <(XMLSize s1, XMLSize s2)
        {
            return !(s1 >= s2);
        }
        public static bool operator <=(XMLSize s1, XMLSize s2)
        {
            return !(s1 > s2);
        }
        public static bool operator ==(XMLSize s1, XMLSize s2)
        {
            return (s1 >= s2) && (s1 <= s2);
        }
        public static bool operator !=(XMLSize s1, XMLSize s2)
        {
            return !(s1 == s2);
        }

        public override bool Equals(object obj)
        {
            if (obj is XMLSize)
                return (obj as XMLSize?) == this;
            else return false;
        }
        #endregion

        public override string ToString()
        {
            return string.Format("{0}; {1}", Width, Height);
        }
    }

    public struct XMLPoint
    {
        //http://msdn.microsoft.com/en-us/library/system.drawing.point.aspx

        private int x, y;

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public bool IsEmpty
        {
            get { return X == 0 && Y == 0; }
        }

        public XMLPoint(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public XMLPoint Offset(XMLPoint point)
        {
            return this + point;
        }
        public XMLPoint Offset(int x, int y)
        {
            return this.Offset(new XMLPoint() { X = x, Y = y });
        }

        #region Operadores binários
        public static XMLPoint operator +(XMLPoint s1, XMLPoint s2)
        {
            return new XMLPoint() { X = s1.X + s2.X, Y = s1.Y + s2.Y };
        }
        public static XMLPoint operator -(XMLPoint s1, XMLPoint s2)
        {
            return new XMLPoint() { X = s1.X - s2.X, Y = s1.Y - s2.Y };
        }
        public static bool operator ==(XMLPoint s1, XMLPoint s2)
        {
            return (s1.X == s2.X) && (s1.Y == s2.Y);
        }
        public static bool operator !=(XMLPoint s1, XMLPoint s2)
        {
            return !(s1 == s2);
        }

        public override bool Equals(object obj)
        {
            if (obj is XMLPoint)
                return (obj as XMLPoint?) == this;
            else return false;
        }
        #endregion

        public override string ToString()
        {
            return string.Format("({0}, {1})", X, Y); ;
        }
    }

    public struct XMLPointF
    {
        //http://msdn.microsoft.com/en-us/library/system.drawing.point.aspx

        float x, y;

        public float X
        {
            get { return x; }
            set { x = value; }
        }

        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        public bool IsEmpty
        {
            get { return X == 0 && Y == 0; }
        }

        public XMLPointF(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public XMLPointF Offset(XMLPointF point)
        {
            return this + point;
        }
        public XMLPointF Offset(float x, float y)
        {
            return this.Offset(new XMLPointF() { X = x, Y = y });
        }

        #region Operadores binários
        public static XMLPointF operator +(XMLPointF s1, XMLPointF s2)
        {
            return new XMLPointF() { X = s1.X + s2.X, Y = s1.Y + s2.Y };
        }
        public static XMLPointF operator -(XMLPointF s1, XMLPointF s2)
        {
            return new XMLPointF() { X = s1.X - s2.X, Y = s1.Y - s2.Y };
        }
        public static bool operator ==(XMLPointF s1, XMLPointF s2)
        {
            return (s1.X == s2.X) && (s1.Y == s2.Y);
        }
        public static bool operator !=(XMLPointF s1, XMLPointF s2)
        {
            return !(s1 == s2);
        }

        public override bool Equals(object obj)
        {
            if (obj is XMLPointF)
                return (obj as XMLPointF?) == this;
            else return false;
        }
        #endregion

        public override string ToString()
        {
            return string.Format("({0}, {1})", X, Y); ;
        }
    }

    public struct XMLRectangle
    {
        //http://msdn.microsoft.com/en-us/library/system.drawing.rectangle.aspx

        private XMLPoint location;
        private XMLSize size;

        public XMLPoint Location
        {
            get { return location; }
            set { location = value; }
        }
        public XMLSize Size
        {
            get { return size; }
            set { size = value; }
        }

        public int X
        {
            get { return Location.X; }
        }
        public int Y
        {
            get { return Location.Y; }
        }

        public int Width
        {
            get { return Size.Width; }
        }
        public int Height
        {
            get { return Size.Height; }
        }

        public int Top
        {
            get { return this.Y; }
        }
        public int Bottom
        {
            get { return this.Y + this.Height; }
        }
        public int Left
        {
            get { return this.X; }
        }
        public int Right
        {
            get { return this.X + this.Width; }
        }

        public bool IsEmpty
        {
            get { return Location.IsEmpty && Size.IsEmpty; }
        }

        public XMLRectangle(XMLPoint location, XMLSize size)
        {
            this.location = location;
            this.size = size;
        }
        public XMLRectangle(int x, int y, int width, int height)
        {
            this.location = new XMLPoint(x, y);
            this.size = new XMLSize(width, height);
        }

        public override string ToString()
        {
            return string.Format("{X={0}, Y={1}, Width={2}, Height={3}}", Location.X, Location.Y, Size.Width, Size.Height);
        }

        #region Operadores binários

        public static bool operator ==(XMLRectangle r1, XMLRectangle r2)
        {
            return r1.Location == r2.Location && r1.Size == r2.Size;
        }
        public static bool operator !=(XMLRectangle r1, XMLRectangle r2)
        {
            return !(r1 == r2);
        }

        public override bool Equals(object obj)
        {
            if (obj is XMLRectangle)
                return (obj as XMLRectangle?) == this;
            else return false;
        }
        #endregion
    }
    #endregion

    #region Fonts
    public class XMLFont
    {
        private bool bold, italic, strikeOut, underline;
        public bool Bold { get { return (style & XMLFontStyle.Bold) == XMLFontStyle.Bold; } }
        public bool Italic { get { return (style & XMLFontStyle.Italic) == XMLFontStyle.Italic; } }
        public bool Strikeout { get { return (style & XMLFontStyle.Strikeout) == XMLFontStyle.Strikeout; } }
        public bool Underline { get { return (style & XMLFontStyle.Underline) == XMLFontStyle.Underline; } }
        public bool Regular { get { return (style & XMLFontStyle.Regular) == XMLFontStyle.Regular; } }

        private bool gdiVerticalFont;
        public bool GdiVerticalFont
        {
            get { return gdiVerticalFont; }
            set { gdiVerticalFont = value; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private float size;
        public float Size
        {
            get { return size; }
            set { size = value; }
        }

        private XMLFontStyle style;
        public XMLFontStyle Style
        {
            get { return style; }
            set { style = value; }
        }

        private XMLGraphicsUnit unit;
        public XMLGraphicsUnit Unit
        {
            get { return unit; }
            set { unit = value; }
        }

        private byte gdiCharSet;
        public byte GdiCharSet
        {
            get { return gdiCharSet; }
            set { gdiCharSet = value; }
        }

        public XMLFont() { }
        public XMLFont(string name, float emSize, XMLFontStyle style, XMLGraphicsUnit unit, byte gdiCharSet, bool gdiVerticalFont)
        {
            this.name = name;
            this.size = emSize;
            this.style = style;
            this.unit = unit;
            this.gdiCharSet = gdiCharSet;
            this.gdiVerticalFont = gdiVerticalFont;
        }
    }

    [FlagsAttribute]
    public enum XMLFontStyle
    {
        Regular = 0,
        Bold = 1,
        Italic = 2,
        Underline = 4,
        Strikeout = 8
    }

    public enum XMLGraphicsUnit
    {
        World = 0,
        Display = 1,
        Pixel = 2,
        Point = 3,
        Inch = 4,
        Document = 5,
        Millimiter = 6
    }
    #endregion

    #region Cores
    public struct XMLColor
    {
        private int a, r, g, b;

        public int A
        {
            get { return a; }
            set { a = value; }
        }
        public int R
        {
            get { return r; }
            set { r = value; }
        }
        public int G
        {
            get { return g; }
            set { g = value; }
        }
        public int B
        {
            get { return b; }
            set { b = value; }
        }

        public XMLColor(int a, int r, int g, int b)
        {
            this.a = a;
            this.r = r;
            this.g = g;
            this.b = b;
        }
    }
    #endregion

    public enum XMLDirection
    {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4
    }
}
