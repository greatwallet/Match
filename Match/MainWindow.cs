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
using System.Timers;

namespace Match
{
    enum Mode { SELF_INPUT = 0, AUTO_GEN = 1 };
    public partial class MainWindow : MaterialForm
    {
        private readonly MaterialSkinManager materialSkinManager;
        private const int N_SSD = 28;
        // SSD consisted of matches
        private SSD_match[] match_ssds = new SSD_match[N_SSD];
        // the number of matches ought to be moved
        private int movMatches = 1;
        // store the answer list
        List<string> ans_list4win = new List<string>();
        // the index of and_list4win
        int idx_ans = 0;
        // the maximum timespan of the algorithm (in seconds)
        int MAX_TIME = 10;
        // the maximum amount of answers it provides
        int MAX_ANS_NUM = 1;
        private string equ2solve;
        // current mode
        Mode mode = Mode.SELF_INPUT;
        // current level
        Level lv = Level.MEDIUM;
        public MainWindow()
        {
            InitializeComponent();
            // enable cross thread call
            CheckForIllegalCrossThreadCalls = false;

            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);

            // Set the level to medium
            this.comboBox_level.SelectedIndex = 1;
            
            // skinProgressIndicator1.
            initSSD();

            // set the mov match number
            movMatches = 1;

            // set the max time
            MAX_TIME = 10;

            // set the max ans num
            MAX_ANS_NUM = 1;

            // mode & level
            mode = Mode.SELF_INPUT;
            lv = Level.MEDIUM;
        }

        //  panel display: self-input mode
        private void RB_self_input_CheckedChanged(object sender, EventArgs e)
        {
            // Set up visibility
            this.Text_equ.Visible = true;
            panel1.BorderStyle = BorderStyle.Fixed3D;
            this.tableLayoutPanel_level.Visible = false;
            this.label_level.Visible = false;
            this.comboBox_level.Visible = false;
            // set up mode
            mode = Mode.SELF_INPUT;
        }

        // panel display: auto-generate mode
        private void RB_generate_CheckedChanged(object sender, EventArgs e)
        {
            this.Text_equ.Visible = false;
            panel1.BorderStyle = BorderStyle.None;
            this.tableLayoutPanel_level.Visible = true;
            this.label_level.Visible = true;
            this.comboBox_level.Visible = true;
            // set up mode
            mode = Mode.AUTO_GEN;
        }

        // Check if the equation is valid
        private bool isValidEqu(string equ)
        {
            if (!Equation.isValidEqu(equ)) return false;
            // check length <= N_SSD, otherwise upoverflow!
            if (equ.Length > N_SSD) return false;

            return true;
        }

        // Convert string to ssd_match
        private void str2match(string equ)
        {
            // Assert: equ is valid!
            // match_ssds = new List<SSD_match>(equ.Length);
            clearSSD();
            for (int i = 0; i < equ.Length; i++)
            {
                match_ssds[i].display(equ[i]);
            }
        }

        // Start providing answers
        private void Button_ans_Click(object sender, EventArgs e)
        {
            List<string> ans_list;
            // set up time limit for the search;
            Equation.Search(ref equ2solve, out ans_list, movMatches, MAX_ANS_NUM,Convert.ToDouble(MAX_TIME));
            if (ans_list.Count == 0)
            {
                MessageBox.Show("此题无解");
                return;
            }
            ans_list4win = new List<string>(ans_list);

            idx_ans = 0;
            str2match(ans_list4win[idx_ans]);
            this.Button_next.Enabled = true;
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
        
        // Clear SSD
        private void clearSSD()
        {
            for (int i = 0; i < N_SSD; i++)
            {
                match_ssds[i].Clear();
            }
        } 
        // Start Playing!
        private void Button_user_Click(object sender, EventArgs e)
        {
            // TODO: check the option: auto-generated or self-input
            if (mode == Mode.SELF_INPUT)
            {
                // Check if the equation is in right format
                if (!isValidEqu(this.Text_equ.Text))
                {
                    MessageBox.Show("输入等式格式错误！");
                    return;
                }

                equ2solve = this.Text_equ.Text;
            }
            else
            {
                Equation.Generate(lv, out equ2solve, movMatches);
            }
            // Generate match
            str2match(equ2solve);
            this.Button_ans.Enabled = true;
            this.Button_ans.Visible = true;
            this.Button_next.Enabled = false;
            this.Button_next.Visible = true;
        }

        // change theme 
        private void Button_Theme_Click(object sender, EventArgs e)
        {
            materialSkinManager.Theme = materialSkinManager.Theme == MaterialSkinManager.Themes.DARK ? MaterialSkinManager.Themes.LIGHT : MaterialSkinManager.Themes.DARK;
        }
        // change color
        private int colorSchemeIndex = 0;
        private void Button_Color_Click(object sender, EventArgs e)
        {
            colorSchemeIndex++;
            if (colorSchemeIndex > 2) colorSchemeIndex = 0;

            //These are just example color schemes
            switch (colorSchemeIndex)
            {
                case 0:
                    materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
                    break;
                case 1:
                    materialSkinManager.ColorScheme = new ColorScheme(Primary.Indigo500, Primary.Indigo700, Primary.Indigo100, Accent.Pink200, TextShade.WHITE);
                    break;
                case 2:
                    materialSkinManager.ColorScheme = new ColorScheme(Primary.Green600, Primary.Green700, Primary.Green200, Accent.Red100, TextShade.WHITE);
                    break;
            }
        }

        // change mov match
        private void RB_matchNum_1_CheckedChanged(object sender, EventArgs e)
        {
            movMatches = 1;
        }
        private void RB_matchNum_2_CheckedChanged(object sender, EventArgs e)
        {
            movMatches = 2;
        }

        // change brush time
        private void Button_decrease_Click(object sender, EventArgs e)
        {
            if (PGB_time.Value <= PGB_time.Minimum) return;
            PGB_time.Value -= 1;
            MAX_TIME = PGB_time.Value;
        }
        private void Button_increase_Click(object sender, EventArgs e)
        {
            if (PGB_time.Value >= PGB_time.Maximum) return;
            PGB_time.Value += 1;
            MAX_TIME = PGB_time.Value;
        }

        // display the next answer
        private void Button_next_Click(object sender, EventArgs e)
        {
            idx_ans++;
            idx_ans %= ans_list4win.Count;
            str2match(ans_list4win[idx_ans]);
        }

        // change the number of maximum answers
        private void RB_ans_num_1_CheckedChanged(object sender, EventArgs e)
        {
            MAX_ANS_NUM = 1;
        }
        private void RB_ans_num_2_CheckedChanged(object sender, EventArgs e)
        {
            MAX_ANS_NUM = 2;
        }
        private void RB_ans_num_5_CheckedChanged(object sender, EventArgs e)
        {
            MAX_ANS_NUM = 5;
        }
        private void RB_ans_num_infty_CheckedChanged(object sender, EventArgs e)
        {
            MAX_ANS_NUM = int.MaxValue;
        }

        // set level
        private void comboBox_level_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(comboBox_level.SelectedIndex)
            {
                case 0: lv = Level.EASY;break;
                case 1: lv = Level.MEDIUM; break;
                case 2: lv = Level.HARD;break;
            }
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
