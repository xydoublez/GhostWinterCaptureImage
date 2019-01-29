using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GhostWinterCaptureImage
{
    public partial class ColorBox : Control
    {
        private Color _selectedColor;
        /// <summary>
        /// 选中的颜色
        /// </summary>
        public Color SelectedColor
        {
            get { return _selectedColor; }
        }
        private Point _ptCurrent;
        private Rectangle _rectSelected;
        private Bitmap _imgColor = global::GhostWinterCaptureImage.Properties.Resources.color;
        private Color _lastColor;
        public ColorBox()
        {
            InitializeComponent();
            _selectedColor = Color.Red;
            _rectSelected = new Rectangle(-100, -100, 14, 14);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        
        /// <summary>
        /// 颜色改变委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public delegate void ColorChangeHandler(object sender,ColorChangedEventArgs e);
        /// <summary>
        /// 颜色改变事件
        /// </summary>
        public event ColorChangeHandler ColorChanged;
        /// <summary>
        /// 颜色改变事件处理方法
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnColorChanged(ColorChangedEventArgs e)
        {
            if (this.ColorChanged != null)
            {
                ColorChanged(this, e);
            }
        }
        protected override void OnClick(EventArgs e)
        {
            Color clr = _imgColor.GetPixel(_ptCurrent.X, _ptCurrent.Y);
            if ( clr.ToArgb() != Color.FromArgb(255, 254, 254, 254).ToArgb()
                &&clr.ToArgb() != Color.FromArgb(255,133,141,151).ToArgb()
                &&clr.ToArgb() != Color.FromArgb(255,110,126,149).ToArgb()
                )
            {
                if (this._selectedColor != clr)
                {
                    this._selectedColor = clr;
                }
                this.Invalidate();
                this.OnColorChanged(new ColorChangedEventArgs(clr));
            }
            base.OnClick(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            _ptCurrent = e.Location;
            try
            {
                Color clr = _imgColor.GetPixel(_ptCurrent.X, _ptCurrent.Y);
                if (clr != _lastColor)
                {
                    if (clr.ToArgb() != Color.FromArgb(255, 254, 254, 254).ToArgb()
                        && clr.ToArgb() != Color.FromArgb(255, 133, 141, 151).ToArgb()
                        && clr.ToArgb() != Color.FromArgb(255, 110, 126, 149).ToArgb()
                        && e.X > 39
                        )
                    {
                        _rectSelected.Y = e.Y > 17 ? 17 : 2;
                        _rectSelected.X = ((e.X - 39) / 15) * 15 + 38;
                        this.Invalidate();
                    }
                    else
                    {
                        _rectSelected.X = _rectSelected.Y = -100;
                        this.Invalidate();

                    }
                }
            }
            finally
            {
                base.OnMouseMove(e);
            }
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            _rectSelected.X = _rectSelected.Y - 100;
            this.Invalidate();
            base.OnMouseLeave(e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(global::GhostWinterCaptureImage.Properties.Resources.color,
                new Rectangle(0, 0, 165, 35));
            g.DrawRectangle(Pens.SteelBlue, 0, 0, 164, 34);
            SolidBrush brush = new SolidBrush(_selectedColor);
            g.FillRectangle(brush, 9, 5, 24, 24);
            g.DrawRectangle(Pens.DarkBlue, _rectSelected);
            base.OnPaint(e);
        }
        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, 165, 35, specified);
        }
        
    }

    /// <summary>
    /// 颜色改变事件参数
    /// </summary>
    public class ColorChangedEventArgs : EventArgs
    {
         private Color color;
         public Color Color
         {
             get { return color; }
         }
         public ColorChangedEventArgs(Color color)
         {
             this.color = color;
         }
    }
}
