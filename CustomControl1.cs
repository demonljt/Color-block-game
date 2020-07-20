using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    public partial class ShapeButton : Button
    {
        public ShapeButton()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        bool flag;
        [Description(" 获取或设置按钮椭圆效果。"), DefaultValue(false)]
        public bool Circle
        {
            set
            {
                flag = value;
                GraphicsPath gp = new GraphicsPath();
                gp.AddEllipse(this.ClientRectangle);//圆形
                this.Region = new Region(gp);
                FlatAppearance.BorderSize = 0;//去掉边框
                FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));//背景颜色
                this.Invalidate();
            }
            get { return flag; }
        }
    }
}
