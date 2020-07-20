using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Threading;
using System.CodeDom.Compiler;
using System.Xml.Schema;

namespace test
{
    public partial class Form1 : Form
    {
        private delegate void FlushClient();//代理
        string timingGo;
        public Form1()
        {
            InitializeComponent();
            this.label3.Text = "0分0秒";
            System.Timers.Timer timer1 = new System.Timers.Timer();
            timer1.Start();
            timer1.Interval = 1000; //执行间隔时间,单位为毫秒; 这里实际间隔为1分钟  
            timer1.Elapsed += new System.Timers.ElapsedEventHandler(Timing);
            comboBox2.SelectedItem = "旋转色块";
            comboBox1.SelectedItem = "4×4";
        }
        int canStart = 0;//计时控件状态：0暂停；1复位；2开始
        #region 关闭窗口、最小化
        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            label1.ForeColor = Color.White;
            label1.BackColor = Color.FromArgb(30, 0, 0, 0);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            label1.ForeColor = Color.DimGray;
            label1.BackColor = Color.Transparent;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            label2.ForeColor = Color.DimGray;
            label2.BackColor = Color.Transparent;
        }

        private void label2_MouseMove(object sender, MouseEventArgs e)
        {
            label2.ForeColor = Color.White;
            label2.BackColor = Color.FromArgb(30, 0, 0, 0);
        }
        #endregion

        #region 计时控件
        private void button1_Click(object sender, EventArgs e)
        {
            if (canStart == 1) {
                canStart = 2;
                if (comboBox2.SelectedItem.ToString() == "黑白迭代") { 
                    B_WcanGo(signtarget);//黑白迭代保存随机生成盘面并恢复白色盘面
                    foreach (Control control in this.groupBox3.Controls)
                    {
                        control.BackColor = Color.White;
                    }
                }
            }
        }
        int timingNum = 0;
        void Timing(object source, ElapsedEventArgs e) {
            if (canStart == 1) {
                timingGo = "0分0秒";
                timingNum = 0;
            } else if (canStart == 2)
            {
                timingNum++;
                int a, b;
                a = timingNum / 60;
                b = timingNum % 60;
                timingGo = a + "分" + b + "秒";
                Thread thread = new Thread(CrossThreadFlush);
                thread.IsBackground = true;
                thread.Start();
            }
        }
        private void CrossThreadFlush()
        {
            while (true)
            {
                //将sleep和无限循环放在等待异步的外面
                Thread.Sleep(1000);
                ThreadFunction();
            }
        }
        private void ThreadFunction()
        {
            if (this.label3.InvokeRequired)//等待异步
            {
                FlushClient fc = new FlushClient(ThreadFunction);
                this.Invoke(fc);//通过代理调用刷新方法
            }
            else
            {
                this.label3.Text = timingGo;
            }
        }
        #endregion

        #region 游戏设置
        void gameSelection(object sender, EventArgs e) {
            switch (comboBox2.SelectedItem.ToString())
            {
                case "旋转色块":
                    groupBox1.Visible = true;
                    groupBox3.Visible = false;
                    comboBox3.Visible = false;
                    pictureBox17.Visible = true;
                    button37.Visible = false;
                    label7.Visible = false;
                    break;
                case "黑白迭代":
                    groupBox1.Visible = false;
                    groupBox3.Visible = true;
                    comboBox3.Visible = true;
                    comboBox3.SelectedItem = "简单";
                    pictureBox17.Visible = false;
                    button37.Visible = true;
                    label7.Visible = false;
                    break;
                case "黑白消除":
                    groupBox1.Visible = false;
                    groupBox3.Visible = true;
                    comboBox3.Visible = true;
                    comboBox3.SelectedItem = "简单";
                    pictureBox17.Visible = false;
                    button37.Visible = false;
                    label7.Visible = true;
                    break;
            }
        }
        void scaleSelection(object sender, EventArgs e) {
            switch (comboBox1.SelectedItem.ToString())
            {
                case "4×4": groupBox2.Visible = false; break;
                case "6×6": groupBox2.Visible = true; break;
                case "8×8": MessageBox.Show("自行脑补类推吧，懒得写了"); break;
            }

        }
        #endregion

        #region 随机生成题目

        private void button2_Click(object sender, EventArgs e)
        {

            canStart = 1;
            if (comboBox1.SelectedItem.ToString() == "4×4" && comboBox2.SelectedItem.ToString() == "旋转色块") {
                int[] a = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
                randomList(a);
                for (int i = 0; i < 4; i++)
                {
                    Color temp1 = Color.Red;
                    switch (i)
                    {
                        case 0:
                            temp1 = Color.Red;
                            break;
                        case 1:
                            temp1 = Color.Green;
                            break;
                        case 2:
                            temp1 = Color.Blue;
                            break;
                        case 3:
                            temp1 = Color.Yellow;
                            break;
                    }
                    for (int j = i * 4; j < i * 4 + 4; j++)
                    {
                        randomChangecolor4(a[j], temp1);
                    }
                }
            } else if (comboBox1.SelectedItem.ToString() == "6×6" && comboBox2.SelectedItem.ToString() == "旋转色块") {
                int[] a = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36 };
                randomList(a);
                for (int i = 0; i < 4; i++)
                {
                    Color temp1 = Color.Red;
                    switch (i)
                    {
                        case 0:
                            temp1 = Color.Red;
                            break;
                        case 1:
                            temp1 = Color.Green;
                            break;
                        case 2:
                            temp1 = Color.Blue;
                            break;
                        case 3:
                            temp1 = Color.Yellow;
                            break;
                    }
                    for (int j = i * 9; j < i * 9 + 9; j++)
                    {
                        randomChangecolor6(a[j], temp1);
                    }
                }
            } else if (comboBox2.SelectedItem.ToString() == "黑白迭代" && comboBox3.SelectedItem.ToString() == "简单") {
                randomSwitchcolor(6);
            } else if (comboBox2.SelectedItem.ToString() == "黑白迭代" && comboBox3.SelectedItem.ToString() == "一般") {
                randomSwitchcolor(10);
            } else if (comboBox2.SelectedItem.ToString() == "黑白迭代" && comboBox3.SelectedItem.ToString() == "困难") {
                randomSwitchcolor(16);
            } else if (comboBox2.SelectedItem.ToString() == "黑白消除" && comboBox3.SelectedItem.ToString() == "简单") {
                randomSwitchcolor(8);
            } else if (comboBox2.SelectedItem.ToString() == "黑白消除" && comboBox3.SelectedItem.ToString() == "一般") {
                randomSwitchcolor(16);
            } else if (comboBox2.SelectedItem.ToString() == "黑白消除" && comboBox3.SelectedItem.ToString() == "困难") {
                randomSwitchcolor(32);
            }
        }
        void randomList(int[] a)
        {
            //对数组进行随机排序的算法:随机选择两个位置，将两个位置上的值交换
            //交换的次数,这里使用数组的长度作为交换次数
            int count = a.Length;
            Random random = new Random();
            //开始交换
            for (int i = 0; i < count; i++)
            {
                //生成两个随机数位置
                int randomNum1 = random.Next(0, a.Length);
                int randomNum2 = random.Next(0, a.Length);
                //定义临时变量
                int temp;
                //交换两个随机数位置的值
                temp = a[randomNum1];
                a[randomNum1] = a[randomNum2];
                a[randomNum2] = temp;
            }
        }
        void randomChangecolor4(int i, Color temp1) {
            switch (i)
            {
                case 1:
                    pictureBox1.BackColor = temp1;
                    break;
                case 2:
                    pictureBox2.BackColor = temp1;
                    break;
                case 3:
                    pictureBox3.BackColor = temp1;
                    break;
                case 4:
                    pictureBox4.BackColor = temp1;
                    break;
                case 5:
                    pictureBox5.BackColor = temp1;
                    break;
                case 6:
                    pictureBox6.BackColor = temp1;
                    break;
                case 7:
                    pictureBox7.BackColor = temp1;
                    break;
                case 8:
                    pictureBox8.BackColor = temp1;
                    break;
                case 9:
                    pictureBox9.BackColor = temp1;
                    break;
                case 10:
                    pictureBox10.BackColor = temp1;
                    break;
                case 11:
                    pictureBox11.BackColor = temp1;
                    break;
                case 12:
                    pictureBox12.BackColor = temp1;
                    break;
                case 13:
                    pictureBox13.BackColor = temp1;
                    break;
                case 14:
                    pictureBox14.BackColor = temp1;
                    break;
                case 15:
                    pictureBox15.BackColor = temp1;
                    break;
                case 16:
                    pictureBox16.BackColor = temp1;
                    break;
            }
        }
        void randomChangecolor6(int i, Color temp1) {
            switch (i)
            {
                case 1:
                    pictureBox33.BackColor = temp1;
                    break;
                case 2:
                    pictureBox32.BackColor = temp1;
                    break;
                case 3:
                    pictureBox35.BackColor = temp1;
                    break;
                case 4:
                    pictureBox30.BackColor = temp1;
                    break;
                case 5:
                    pictureBox21.BackColor = temp1;
                    break;
                case 6:
                    pictureBox37.BackColor = temp1;
                    break;
                case 7:
                    pictureBox31.BackColor = temp1;
                    break;
                case 8:
                    pictureBox28.BackColor = temp1;
                    break;
                case 9:
                    pictureBox34.BackColor = temp1;
                    break;
                case 10:
                    pictureBox26.BackColor = temp1;
                    break;
                case 11:
                    pictureBox20.BackColor = temp1;
                    break;
                case 12:
                    pictureBox36.BackColor = temp1;
                    break;
                case 13:
                    pictureBox38.BackColor = temp1;
                    break;
                case 14:
                    pictureBox39.BackColor = temp1;
                    break;
                case 15:
                    pictureBox42.BackColor = temp1;
                    break;
                case 16:
                    pictureBox40.BackColor = temp1;
                    break;
                case 17:
                    pictureBox41.BackColor = temp1;
                    break;
                case 18:
                    pictureBox43.BackColor = temp1;
                    break;
                case 19:
                    pictureBox29.BackColor = temp1;
                    break;
                case 20:
                    pictureBox25.BackColor = temp1;
                    break;
                case 21:
                    pictureBox46.BackColor = temp1;
                    break;
                case 22:
                    pictureBox24.BackColor = temp1;
                    break;
                case 23:
                    pictureBox19.BackColor = temp1;
                    break;
                case 24:
                    pictureBox44.BackColor = temp1;
                    break;
                case 25:
                    pictureBox27.BackColor = temp1;
                    break;
                case 26:
                    pictureBox23.BackColor = temp1;
                    break;
                case 27:
                    pictureBox47.BackColor = temp1;
                    break;
                case 28:
                    pictureBox22.BackColor = temp1;
                    break;
                case 29:
                    pictureBox18.BackColor = temp1;
                    break;
                case 30:
                    pictureBox45.BackColor = temp1;
                    break;
                case 31:
                    pictureBox48.BackColor = temp1;
                    break;
                case 32:
                    pictureBox49.BackColor = temp1;
                    break;
                case 33:
                    pictureBox50.BackColor = temp1;
                    break;
                case 34:
                    pictureBox51.BackColor = temp1;
                    break;
                case 35:
                    pictureBox52.BackColor = temp1;
                    break;
                case 36:
                    pictureBox53.BackColor = temp1;
                    break;
            }
        }
        void randomSwitchcolor(int i) {
            foreach (Control control in this.groupBox3.Controls)
            {
                control.BackColor = Color.White;
            }
            Random random = new Random();
            for (int j = 0; j < i; j++) {
                int k = random.Next(123, 200);
                if (k % 10 <= 9 && k % 10 >= 3 || k % 10 == 0) {
                    B_Wswitch("pictureBox" + (k).ToString());
                    B_Wswitch("pictureBox" + (k + 1).ToString());
                    B_Wswitch("pictureBox" + (k - 1).ToString());
                    B_Wswitch("pictureBox" + (k + 10).ToString());
                    B_Wswitch("pictureBox" + (k - 10).ToString());
                }
            }
        }
        #endregion

        #region 旋转色块4×4
        #region 顺时针旋转按钮，颜色变化
        private void button4_Click(object sender, EventArgs e)
        {
            changeColor(pictureBox1, pictureBox5, pictureBox6, pictureBox2);
            completedJudg();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            changeColor(pictureBox2, pictureBox6, pictureBox7, pictureBox3);
            completedJudg();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            changeColor(pictureBox3, pictureBox7, pictureBox8, pictureBox4);
            completedJudg();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            changeColor(pictureBox5, pictureBox9, pictureBox10, pictureBox6);
            completedJudg();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            changeColor(pictureBox6, pictureBox10, pictureBox11, pictureBox7);
            completedJudg();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            changeColor(pictureBox7, pictureBox11, pictureBox12, pictureBox8);
            completedJudg();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            changeColor(pictureBox9, pictureBox13, pictureBox14, pictureBox10);
            completedJudg();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            changeColor(pictureBox10, pictureBox14, pictureBox15, pictureBox11);
            completedJudg();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            changeColor(pictureBox11, pictureBox15, pictureBox16, pictureBox12);
            completedJudg();
        }
        void changeColor(PictureBox a, PictureBox b, PictureBox c, PictureBox d)
        {
            if (canStart == 2) {
                Color temp = a.BackColor;
                a.BackColor = b.BackColor;
                b.BackColor = c.BackColor;
                c.BackColor = d.BackColor;
                d.BackColor = temp;
            }
        }
        #endregion

        #region 完成判断
        void completedJudg()
        {
            if (pictureBox1.BackColor == Color.Red && pictureBox2.BackColor == Color.Red
                && pictureBox5.BackColor == Color.Red && pictureBox6.BackColor == Color.Red
                && pictureBox3.BackColor == Color.Green && pictureBox4.BackColor == Color.Green
                && pictureBox7.BackColor == Color.Green && pictureBox8.BackColor == Color.Green
                && pictureBox9.BackColor == Color.Blue && pictureBox10.BackColor == Color.Blue
                && pictureBox13.BackColor == Color.Blue && pictureBox14.BackColor == Color.Blue
                && pictureBox11.BackColor == Color.Yellow && pictureBox12.BackColor == Color.Yellow
                && pictureBox15.BackColor == Color.Yellow && pictureBox16.BackColor == Color.Yellow
                && canStart == 2) {
                canStart = 0;
                MessageBox.Show("恭喜，已完成复位");
            }
        }
        #endregion

        #endregion

        #region 旋转色块6×6
        #region 色块旋转
        private void button12_Click(object sender, EventArgs e)
        {
            changeColor(pictureBox33, pictureBox31, pictureBox28, pictureBox32);
            completedJudg66();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            changeColor(pictureBox32, pictureBox28, pictureBox34, pictureBox35);
            completedJudg66();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            changeColor(pictureBox35, pictureBox34, pictureBox26, pictureBox30);
            completedJudg66();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            changeColor(pictureBox30, pictureBox26, pictureBox20, pictureBox21);
            completedJudg66();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            changeColor(pictureBox21, pictureBox20, pictureBox36, pictureBox37);
            completedJudg66();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            changeColor(pictureBox31, pictureBox38, pictureBox39, pictureBox28);
            completedJudg66();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            changeColor(pictureBox28, pictureBox39, pictureBox42, pictureBox34);
            completedJudg66();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            changeColor(pictureBox34, pictureBox42, pictureBox40, pictureBox26);
            completedJudg66();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            changeColor(pictureBox26, pictureBox40, pictureBox41, pictureBox20);
            completedJudg66();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            changeColor(pictureBox20, pictureBox41, pictureBox43, pictureBox36);
            completedJudg66();
        }

        private void button22_Click(object sender, EventArgs e)
        {
            changeColor(pictureBox38, pictureBox29, pictureBox25, pictureBox39);
            completedJudg66();
        }

        private void button23_Click(object sender, EventArgs e)
        {
            changeColor(pictureBox39, pictureBox25, pictureBox46, pictureBox42);
            completedJudg66();
        }

        private void button24_Click(object sender, EventArgs e)
        {
            changeColor(pictureBox42, pictureBox46, pictureBox24, pictureBox40);
            completedJudg66();
        }

        private void button25_Click(object sender, EventArgs e)
        {
            changeColor(pictureBox40, pictureBox24, pictureBox19, pictureBox41);
            completedJudg66();
        }

        private void button26_Click(object sender, EventArgs e)
        {
            changeColor(pictureBox41, pictureBox19, pictureBox44, pictureBox43);
            completedJudg66();
        }

        private void button27_Click(object sender, EventArgs e)
        {
            changeColor(pictureBox29, pictureBox27, pictureBox23, pictureBox25);
            completedJudg66();
        }

        private void button28_Click(object sender, EventArgs e)
        {
            changeColor(pictureBox25, pictureBox23, pictureBox47, pictureBox46);
            completedJudg66();
        }

        private void button29_Click(object sender, EventArgs e)
        {
            changeColor(pictureBox46, pictureBox47, pictureBox22, pictureBox24);
            completedJudg66();
        }

        private void button30_Click(object sender, EventArgs e)
        {
            changeColor(pictureBox24, pictureBox22, pictureBox18, pictureBox19);
            completedJudg66();
        }

        private void button31_Click(object sender, EventArgs e)
        {
            changeColor(pictureBox19, pictureBox18, pictureBox45, pictureBox44);
            completedJudg66();
        }

        private void button32_Click(object sender, EventArgs e)
        {
            changeColor(pictureBox27, pictureBox48, pictureBox49, pictureBox23);
            completedJudg66();
        }

        private void button33_Click(object sender, EventArgs e)
        {
            changeColor(pictureBox23, pictureBox49, pictureBox50, pictureBox47);
            completedJudg66();
        }

        private void button34_Click(object sender, EventArgs e)
        {
            changeColor(pictureBox47, pictureBox50, pictureBox51, pictureBox22);
            completedJudg66();
        }

        private void button35_Click(object sender, EventArgs e)
        {
            changeColor(pictureBox22, pictureBox51, pictureBox52, pictureBox18);
            completedJudg66();
        }

        private void button36_Click(object sender, EventArgs e)
        {
            changeColor(pictureBox18, pictureBox52, pictureBox53, pictureBox45);
            completedJudg66();
        }
        #endregion

        #region 完成判断
        void completedJudg66()
        {
            if (pictureBox33.BackColor == Color.Red && pictureBox32.BackColor == Color.Red && pictureBox35.BackColor == Color.Red
                && pictureBox31.BackColor == Color.Red && pictureBox28.BackColor == Color.Red && pictureBox34.BackColor == Color.Red
                && pictureBox38.BackColor == Color.Red && pictureBox39.BackColor == Color.Red && pictureBox42.BackColor == Color.Red
                && pictureBox30.BackColor == Color.Green && pictureBox21.BackColor == Color.Green && pictureBox37.BackColor == Color.Green
                && pictureBox26.BackColor == Color.Green && pictureBox20.BackColor == Color.Green && pictureBox36.BackColor == Color.Green
                && pictureBox40.BackColor == Color.Green && pictureBox41.BackColor == Color.Green && pictureBox43.BackColor == Color.Green
                && pictureBox29.BackColor == Color.Blue && pictureBox25.BackColor == Color.Blue && pictureBox46.BackColor == Color.Blue
                && pictureBox27.BackColor == Color.Blue && pictureBox23.BackColor == Color.Blue && pictureBox47.BackColor == Color.Blue
                && pictureBox48.BackColor == Color.Blue && pictureBox49.BackColor == Color.Blue && pictureBox50.BackColor == Color.Blue
                && pictureBox24.BackColor == Color.Yellow && pictureBox19.BackColor == Color.Yellow && pictureBox44.BackColor == Color.Yellow
                && pictureBox22.BackColor == Color.Yellow && pictureBox18.BackColor == Color.Yellow && pictureBox45.BackColor == Color.Yellow
                && pictureBox51.BackColor == Color.Yellow && pictureBox52.BackColor == Color.Yellow && pictureBox53.BackColor == Color.Yellow
                && canStart == 2)
            {
                canStart = 0;
                MessageBox.Show("恭喜，已完成复位");
            }
        }
        #endregion
        #endregion

        #region 黑白迭代

        #region 黑白色块切换
        void B_Wconversion(object sender, EventArgs e)//判断点击色块并转换周围颜色
        {
            PictureBox button = (PictureBox)sender;
            if (canStart == 2) {
                if (button.BackColor == Color.Black) { button.BackColor = Color.White; }
                else if (button.BackColor == Color.White) { button.BackColor = Color.Black; }
                int result = int.Parse(System.Text.RegularExpressions.Regex.Replace(button.Name, @"[^0-9]+", ""));
                B_Wswitch("pictureBox" + (result + 1).ToString());
                B_Wswitch("pictureBox" + (result - 1).ToString());
                B_Wswitch("pictureBox" + (result + 10).ToString());
                B_Wswitch("pictureBox" + (result - 10).ToString());
                B_WcompletedJudg();
            }
        }
        void B_Wswitch(string str)//切换指定色块颜色
        {
            if (this.groupBox3.Controls[str] != null) {
                Control control = this.groupBox3.Controls[str];
                if (control.BackColor == Color.Black) { control.BackColor = Color.White; }
                else if (control.BackColor == Color.White) { control.BackColor = Color.Black; }
            }
        }
        int[] signCurrent= new int[64];
        int[] signtarget= new int[64];
        void B_WcanGo(int[] para)//将盘面色块信息存入数组
        {
            int i = 0;
            foreach (Control control in this.groupBox3.Controls) {
                if (control.BackColor == Color.Black) {
                    para[i] = 1;
                    i++;
                } else if (control.BackColor == Color.White) {
                    para[i] = 0;
                    i++;
                }
            }
        }
        private void button37_Click(object sender, EventArgs e)
        {
            if (button37.Text == "回看目标" && canStart == 2) {
                button37.Text = "继续作答";
                canStart = 0;
                //B_WcanGo(signCurrent);
                int i = 0;
                foreach (Control control in this.groupBox3.Controls)
                {
                    if (signtarget[i] == 0) { control.BackColor = Color.White;
                    }else if (signtarget[i] == 1) {control.BackColor = Color.Black; }
                    i++;
                }

            } else if (button37.Text == "继续作答") {
                button37.Text = "回看目标";
                canStart = 2;
                int i = 0;
                foreach (Control control in this.groupBox3.Controls)
                {
                    if (signCurrent[i] == 0) { control.BackColor = Color.White;
                    }else if (signCurrent[i] == 1) {control.BackColor = Color.Black; }
                    i++;
                }
            }
        }
        #endregion
        #region 完成判断
        void B_WcompletedJudg() {

            if (comboBox2.SelectedItem.ToString() == "黑白消除") {
                int go = 1;
                foreach (Control control in this.groupBox3.Controls) {
                    if (control.BackColor == Color.Black) { go = go * 0; } else if (control.BackColor == Color.White) { go = go * 1; }
                }
                if (go==1) { 
                    canStart = 0;
                    MessageBox.Show("恭喜，已完成黑白消除");
                }
            }else if (comboBox2.SelectedItem.ToString() == "黑白迭代") {
                B_WcanGo(signCurrent);
                if (CompareArray(signCurrent,signtarget)) { 
                    canStart = 0;
                    MessageBox.Show("恭喜，已完成黑白迭代");
                }
            }
        }
        public bool CompareArray(int[] bt1, int[] bt2)
        {
            var len1 = bt1.Length;
            var len2 = bt2.Length;
            if (len1 != len2)
            {
                return false;
            }
            for (var i = 0; i < len1; i++)
            {
                if (bt1[i] != bt2[i])
                    return false;
            }
            return true;
        }
        #endregion

        #endregion

        private void button38_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem.ToString() == "旋转色块") {
               MessageBox.Show("通过点击旋转图标，则与之相邻的四个色块顺时针旋转90度，多次点击后使盘面颜色符合右下角所示的目标图形。" +
                   "先点击生成题目，确认题目后点击开始即可开始游戏，完成后计时停止并弹窗提示");
            }else if (comboBox2.SelectedItem.ToString() == "黑白迭代") {
               MessageBox.Show("点击任一图块则其本身和上下左右色块均反色，多次点击使盘面图形和目标图形相同" +
                   "先点击生成题目，确认题目后点击开始即可开始游戏，完成后计时停止并弹窗提示，游戏过程中可以点击回看目标查看目标图形，再次点击继续作答则继续游戏。");
            }else if (comboBox2.SelectedItem.ToString() == "黑白消除") { 
               MessageBox.Show("点击任一图块则其本身和上下左右色块均反色，多次点击使盘面图块均为白色。" +
                   "先点击生成题目，确认题目后点击开始即可开始游戏，完成后计时停止并弹窗提示");
            }
        }
    }

}
