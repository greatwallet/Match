using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin.Controls;
using MaterialSkin;

namespace Match
{
    public partial class MainWindow : MaterialForm
    {
        private readonly MaterialSkinManager materialSkinManager;
        private const int N_SSD = 28;
        // SSD consisted of matches
        private SSD_match[] match_ssds = new SSD_match[N_SSD];

        private string equ2solve;
        public MainWindow()
        {
            InitializeComponent();
            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);

            initSSD();
        }

        //  panel display: self-input mode
        private void RB_self_input_CheckedChanged(object sender, EventArgs e)
        {
            this.Text_equ.Visible = true;
            panel1.BorderStyle = BorderStyle.Fixed3D;
            this.tableLayoutPanel_level.Visible = false;
            this.label_level.Visible = false;
            this.comboBox_level.Visible = false;
        }

        // panel display: auto-generate mode
        private void RB_generate_CheckedChanged(object sender, EventArgs e)
        {
            this.Text_equ.Visible = false;
            panel1.BorderStyle = BorderStyle.None;
            this.tableLayoutPanel_level.Visible = true;
            this.label_level.Visible = true;
            this.comboBox_level.Visible = true;
        }

        // Check if the equation is valid
        private bool isValidEqu(string equ)
        {
            // check length <= N_SSD, otherwise upoverflow!
            if (equ.Length > N_SSD) return false;

            string left, right;
            Equation.splitEqu(ref equ, out left, out right);
            // no equal or no right expression
            if (right == "") return false;
            // right expression must be an integer
            int _;
            if (!int.TryParse(right, out _)) return false;
            // try solve the left
            try
            { Expr.evaluate(left); }
            catch (Exception)
            { return false; }
            return true;
        }

        // Convert string to ssd_match
        private void str2match(string equ)
        {
            // Assert: equ is valid!
            // match_ssds = new List<SSD_match>(equ.Length);
            for (int i = 0; i < equ.Length; i++)
            {
                match_ssds[i].display(equ[i]);
            }
        }

        // Generate match boxes
        //private void DrawMatch(string equ)
        //{
        //    MatchBox.
        //}
        // Start providing answers
        private void Button_ans_Click(object sender, EventArgs e)
        {
            List<string> ans_list;
            Equation.Search(ref equ2solve,out ans_list,1,1);
            if(ans_list.Count == 0)
            {
                MessageBox.Show("此题无解");
                return;
            }
            // TODO: modify it so that it could show more answer
            str2match(ans_list[0]);
            return;
        }

        // check if the char is number or operators
        private static bool isValidChar(char c)
        {
            // Enable Delete
            if (c == 46) return true;
            // Enable Digit
            if (c - '0' >= 0 && c - '0' <= 9) return true;
            switch(c)
            {
                case '+':
                case '-':
                case '*':
                case '/':
                case '=':
                    // backspace
                case '\b':
                    return true;
                default:return false;
            }
        }

        // Make sure that the you can only press in numbers and operators
        private void Text_equ_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !isValidChar(e.KeyChar);
        }

        // init SSD
        private void initSSD()
        {
            match_ssds[0] = this.ssD_match1;
            match_ssds[1] = this.ssD_match2;
            match_ssds[2] = this.ssD_match3;
            match_ssds[3] = this.ssD_match4;
            match_ssds[4] = this.ssD_match5;
            match_ssds[5] = this.ssD_match6;
            match_ssds[6] = this.ssD_match7;
            match_ssds[7] = this.ssD_match8;
            match_ssds[8] = this.ssD_match9;
            match_ssds[9] = this.ssD_match10;
            match_ssds[10] = this.ssD_match11;
            match_ssds[11] = this.ssD_match12;
            match_ssds[12] = this.ssD_match13;
            match_ssds[13] = this.ssD_match14;
            match_ssds[14] = this.ssD_match15;
            match_ssds[15] = this.ssD_match16;
            match_ssds[16] = this.ssD_match17;
            match_ssds[17] = this.ssD_match18;
            match_ssds[18] = this.ssD_match19;
            match_ssds[19] = this.ssD_match20;
            match_ssds[20] = this.ssD_match21;
            match_ssds[21] = this.ssD_match22;
            match_ssds[22] = this.ssD_match23;
            match_ssds[23] = this.ssD_match24;
            match_ssds[24] = this.ssD_match25;
            match_ssds[25] = this.ssD_match26;
            match_ssds[26] = this.ssD_match27;
            match_ssds[27] = this.ssD_match28;
        }

        // Start Playing!
        private void Button_user_Click(object sender, EventArgs e)
        {
            // Check if the equation is in right format
            if (!isValidEqu(this.Text_equ.Text))
            {
                MessageBox.Show("输入等式格式错误！");
                return;
            }

            equ2solve = this.Text_equ.Text;
            // Generate match
            str2match(equ2solve);
            this.Button_ans.Enabled = true;
        }

        // set idx'th SSD
        //private void initSSD(int idx, char c)
        //{
        //    switch(idx)
        //    {
        //        case 0: this.ssD_match1.display(c);break;
        //        case 1: this.ssD_match2.display(c); break;
        //        case 2: this.ssD_match3.display(c); break;
        //        case 3: this.ssD_match4.display(c); break;
        //        case 4: this.ssD_match5.display(c); break;
        //        case 5: this.ssD_match6.display(c); break;
        //        case 6: this.ssD_match7.display(c); break;
        //        case 7: this.ssD_match8.display(c); break;
        //        case 8: this.ssD_match9.display(c); break;
        //        case 9: this.ssD_match10.display(c); break;
        //        case 10: this.ssD_match11.display(c); break;
        //        case 11: this.ssD_match12.display(c); break;
        //        case 12: this.ssD_match13.display(c); break;
        //        case 13: this.ssD_match14.display(c); break;
        //        case 14: this.ssD_match15.display(c); break;
        //        case 15: this.ssD_match16.display(c); break;
        //        case 16: this.ssD_match17.display(c); break;
        //        case 17: this.ssD_match18.display(c); break;
        //        case 18: this.ssD_match19.display(c); break;
        //        case 19: this.ssD_match20.display(c); break;
        //        case 20: this.ssD_match21.display(c); break;
        //        case 21: this.ssD_match22.display(c); break;
        //        case 22: this.ssD_match23.display(c); break;
        //        case 23: this.ssD_match24.display(c); break;
        //        case 24: this.ssD_match25.display(c); break;
        //        case 25: this.ssD_match26.display(c); break;
        //        case 26: this.ssD_match27.display(c); break;
        //        case 27: this.ssD_match28.display(c); break;
        //        default:return;
        //    }
        //}
    }
}
