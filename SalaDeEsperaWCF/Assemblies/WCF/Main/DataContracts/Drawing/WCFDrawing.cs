using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Assemblies.DataContracts
{
    #region Espaço
    [DataContract()]
    public struct WCFSize
    {
        //http://msdn.microsoft.com/en-us/library/system.drawing.size.aspx

        private int height, width;

        [DataMember]
        public int Height
        {
            get { return height; }
            set { height = value; }
        }
        [DataMember]
        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public bool IsEmpty
        {
            get { return height == 0 && width == 0; }
        }
        public WCFSize(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        #region Operadores binários
        public static WCFSize operator +(WCFSize s1, WCFSize s2)
        {
            return new WCFSize() { Height = s1.Height + s2.Height, Width = s1.Width + s2.Width };
        }
        public static WCFSize operator -(WCFSize s1, WCFSize s2)
        {
            return new WCFSize() { Height = s1.Height - s2.Height, Width = s1.Width - s2.Width };
        }
        public static bool operator >(WCFSize s1, WCFSize s2)
        {
            return ((s1.Height * s1.Width) > (s2.Height * s2.Width));
        }
        public static bool operator >=(WCFSize s1, WCFSize s2)
        {
            return ((s1.Height * s1.Width) >= (s2.Height * s2.Width));
        }
        public static bool operator <(WCFSize s1, WCFSize s2)
        {
            return !(s1 >= s2);
        }
        public static bool operator <=(WCFSize s1, WCFSize s2)
        {
            return !(s1 > s2);
        }
        public static bool operator ==(WCFSize s1, WCFSize s2)
        {
            return (s1 >= s2) && (s1 <= s2);
        }
        public static bool operator !=(WCFSize s1, WCFSize s2)
        {
            return !(s1 == s2);
        }

        public override bool Equals(object obj)
        {
            if (obj is WCFSize)
                return (obj as WCFSize?) == this;
            else return false;
        }
        #endregion

        public override string ToString()
        {
            return string.Format("{0}x{1}", width, height);
        }

        //#region Casts

        //public static implicit operator WCFSize(Size s)
        //{
        //    return new WCFSize(s.Width, s.Height);
        //}

        //#endregion
    }

    [DataContract()]
    public struct WCFPoint
    {
        //http://msdn.microsoft.com/en-us/library/system.drawing.point.aspx

        private int x, y;

        [DataMember]
        public int X
        {
            get { return x; }
            set { x = value; }
        }
        [DataMember]
        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public bool IsEmpty
        {
            get { return x == 0 && y == 0; }
        }

        public WCFPoint(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public WCFPoint Offset(WCFPoint point)
        {
            return this + point;
        }
        public WCFPoint Offset(int x, int y)
        {
            return this.Offset(new WCFPoint() { X = x, Y = y });
        }

        #region Operadores binários
        public static WCFPoint operator +(WCFPoint s1, WCFPoint s2)
        {
            return new WCFPoint() { X = s1.X + s2.X, Y = s1.Y + s2.Y };
        }
        public static WCFPoint operator -(WCFPoint s1, WCFPoint s2)
        {
            return new WCFPoint() { X = s1.X - s2.X, Y = s1.Y - s2.Y };
        }
        public static bool operator ==(WCFPoint s1, WCFPoint s2)
        {
            return (s1.X == s2.X) && (s1.Y == s2.Y);
        }
        public static bool operator !=(WCFPoint s1, WCFPoint s2)
        {
            return !(s1 == s2);
        }

        public override bool Equals(object obj)
        {
            if (obj is WCFPoint)
                return (obj as WCFPoint?) == this;
            else return false;
        }
        #endregion

        public override string ToString()
        {
            return string.Format("({0}, {1})", x, y); ;
        }

        //#region Casts

        //public static implicit operator WCFPoint(Point p)
        //{
        //    return new WCFPoint(p.X, p.Y);
        //}

        //#endregion
    }

    [DataContract()]
    public struct WCFPointF
    {
        //http://msdn.microsoft.com/en-us/library/system.drawing.point.aspx

        private float x, y;

        [DataMember]
        public float X
        {
            get { return x; }
            set { x = value; }
        }
        [DataMember]
        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        public bool IsEmpty
        {
            get { return x == 0 && y == 0; }
        }

        public WCFPointF(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public WCFPointF Offset(WCFPointF point)
        {
            return this + point;
        }
        public WCFPointF Offset(float x, float y)
        {
            return this.Offset(new WCFPointF() { X = x, Y = y });
        }

        #region Operadores binários
        public static WCFPointF operator +(WCFPointF s1, WCFPointF s2)
        {
            return new WCFPointF() { X = s1.X + s2.X, Y = s1.Y + s2.Y };
        }
        public static WCFPointF operator -(WCFPointF s1, WCFPointF s2)
        {
            return new WCFPointF() { X = s1.X - s2.X, Y = s1.Y - s2.Y };
        }
        public static bool operator ==(WCFPointF s1, WCFPointF s2)
        {
            return (s1.X == s2.X) && (s1.Y == s2.Y);
        }
        public static bool operator !=(WCFPointF s1, WCFPointF s2)
        {
            return !(s1 == s2);
        }

        public override bool Equals(object obj)
        {
            if (obj is WCFPointF)
                return (obj as WCFPointF?) == this;
            else return false;
        }
        #endregion

        public override string ToString()
        {
            return string.Format("({0}, {1})", x, y); ;
        }

        //#region Casts

        //public static implicit operator WCFPoint(Point p)
        //{
        //    return new WCFPoint(p.X, p.Y);
        //}

        //#endregion
    }

    [DataContract()]
    public struct WCFRectangle
    {
        //http://msdn.microsoft.com/en-us/library/system.drawing.rectangle.aspx

        private WCFPoint location;
        private WCFSize size;

        [DataMember]
        public WCFPoint Location
        {
            get { return location; }
            set { location = value; }
        }
        [DataMember]
        public WCFSize Size
        {
            get { return size; }
            set { size = value; }
        }

        public int X
        {
            get { return location.X; }
        }
        public int Y
        {
            get { return location.Y; }
        }

        public int Width
        {
            get { return size.Width; }
        }
        public int Height
        {
            get { return size.Height; }
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
            get { return location.IsEmpty && size.IsEmpty; }
        }

        public WCFRectangle(WCFPoint location, WCFSize size)
        {
            this.location = location;
            this.size = size;
        }
        public WCFRectangle(int x, int y, int width, int height)
        {
            this.location = new WCFPoint(x, y);
            this.size = new WCFSize(width, height);
        }

        public override string ToString()
        {
            return string.Format("{X={0}, Y={1}, Width={2}, Height={3}}", location.X, location.Y, size.Width, size.Height);
        }

        #region Operadores binários

        public static bool operator == (WCFRectangle r1, WCFRectangle r2)
        {
            return r1.Location == r2.Location && r1.Size == r2.Size;
        }
        public static bool operator != (WCFRectangle r1, WCFRectangle r2)
        {
            return !(r1 == r2);
        }

        public override bool Equals(object obj)
        {
            if (obj is WCFRectangle)
                return (obj as WCFRectangle?) == this;
            else return false;
        }
        #endregion

        //#region Casts
        //public static implicit operator WCFRectangle(Rectangle r)
        //{
        //    return new WCFRectangle(r.X, r.Y, r.Width, r.Height);
        //}
        //#endregion
    }
    #endregion

    #region Fonts
    [DataContract()]
    public class WCFFont
    {
        private bool bold, italic, strikeOut, underline;
        public bool Bold { get { return (style & WCFFontStyle.Bold) == WCFFontStyle.Bold; } }
        public bool Italic { get { return (style & WCFFontStyle.Italic) == WCFFontStyle.Italic; } }
        public bool Strikeout { get { return (style & WCFFontStyle.Strikeout) == WCFFontStyle.Strikeout; } }
        public bool Underline { get { return (style & WCFFontStyle.Underline) == WCFFontStyle.Underline; } }
        public bool Regular { get { return (style & WCFFontStyle.Regular) == WCFFontStyle.Regular; } }

        private bool gdiVerticalFont;
        [DataMember]
        public bool GdiVerticalFont
        {
            get { return gdiVerticalFont; }
            set { gdiVerticalFont = value; }
        }

        private string name;
        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private float size;
        [DataMember]
        public float Size
        {
            get { return size; }
            set { size = value; }
        }

        private WCFFontStyle style;
        [DataMember]
        public WCFFontStyle Style
        {
            get { return style; }
            set { style = value; }
        }

        private WCFGraphicsUnit unit;
        [DataMember]
        public WCFGraphicsUnit Unit
        {
            get { return unit; }
            set { unit = value; }
        }

        private byte gdiCharSet;
        [DataMember]
        public byte GdiCharSet
        {
            get { return gdiCharSet; }
            set { gdiCharSet = value; }
        }

        public WCFFont(string name, float emSize, WCFFontStyle style, WCFGraphicsUnit unit, byte gdiCharSet, bool gdiVerticalFont)
        {
            this.name = name;
            this.size = emSize;
            this.style = style;
            this.unit = unit;
            this.gdiCharSet = gdiCharSet;
            this.gdiVerticalFont = gdiVerticalFont;
        }
    }

    [DataContract]
    [FlagsAttribute]
    public enum WCFFontStyle
    {
        [EnumMemberAttribute]
        Regular = 0,
        [EnumMemberAttribute]
        Bold = 1,
        [EnumMemberAttribute]
        Italic = 2,
        [EnumMemberAttribute]
        Underline = 4,
        [EnumMemberAttribute]
        Strikeout = 8
    }

    [DataContract]
    public enum WCFGraphicsUnit
    {
        World = 0,
        [EnumMemberAttribute]
        Display = 1,
        [EnumMemberAttribute]
        Pixel = 2,
        [EnumMemberAttribute]
        Point = 3,
        [EnumMemberAttribute]
        Inch = 4,
        [EnumMemberAttribute]
        Document = 5,
        [EnumMemberAttribute]
        Millimiter = 6
    }
    #endregion

    #region Cores
    [DataContract]
    public struct WCFColor
    {
        private int a, r, g, b;

        [DataMember]
        public int A
        {
            get { return a; }
            set { a = value; }
        }
        [DataMember]
        public int R
        {
            get { return r; }
            set { r = value; }
        }
        [DataMember]
        public int G
        {
            get { return g; }
            set { g = value; }
        }
        [DataMember]
        public int B
        {
            get { return b; }
            set { b = value; }
        }

        public WCFColor(int a, int r, int g, int b)
        {
            this.a = a;
            this.r = r;
            this.g = g;
            this.b = b;
        }
    }
    #endregion
}