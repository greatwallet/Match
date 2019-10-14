using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Match
{
    public partial class SSD_match : UserControl
    {
        // turn up one segment
        private void turnUp(int pos)
        {
            switch (pos)
            {
                // segment
                case 0:A.Visible = true;break;
                case 1:B.Visible = true;break;
                case 2:C.Visible = true;break;
                case 3: D.Visible = true; break;
                case 4: E.Visible = true; break;
                case 5: F.Visible = true; break;
                case 6: G.Visible = true; break;
                default: return;
            }
        }

        // turn down one segment
        private void turnDown(int pos)
        {
            switch (pos)
            {
                // segment
                case 0: A.Visible = false; break;
                case 1: B.Visible = false; break;
                case 2: C.Visible = false; break;
                case 3: D.Visible = false; break;
                case 4: E.Visible = false; break;
                case 5: F.Visible = false; break;
                case 6: G.Visible = false; break;
                default: return;
            }
        }

        // Clear all segment
        public void Clear()
        {
            A.Visible = false;
            B.Visible = false;
            C.Visible = false;
            D.Visible = false;
            E.Visible = false;
            F.Visible = false;
            G.Visible = false;
            eq1.Visible = false;
            eq2.Visible = false;
            add2.Visible = false;
            mul.Visible = false;
            divide.Visible = false;
        }
        
        // display digit 
        public void displayDigit(byte bcd)
        {
            int d = SSD.binary2digit(bcd);
            Debug.Assert(d >= 0 && d <= 9);
            for (int i = 0; i < 7; i++)
            {
                if (checkOnebit(bcd, i))
                    turnUp(i);
                else
                    turnDown(i);
            }
        }

        // display operator
        public void display(char c)
        {
            // opnd
            if (c - '0' >= 0 && c - '0' <= 9)
            {
                displayDigit(SSD.digit2binary(c - '0'));
                return;
            }
            // optr
            Clear();
            switch (c)
            {
                case '+':
                    {
                        add2.Visible = true;
                        G.Visible = true;
                        break;
                    }
                case '-':
                    {
                        G.Visible = true;
                        break;
                    }
                case '*':
                    {
                        mul.Visible = true;
                        break;
                    }
                case '/':
                    {
                        divide.Visible = true;
                        break;
                    }
                case '=':
                    {
                        eq1.Visible = true;
                        eq2.Visible = true;
                        break;
                    }
                default:return;

            }
        }
        
        public SSD_match()
        {
            InitializeComponent();
            Clear();
        }

        // check whether there is a one bit on postion n
        private static bool checkOnebit(byte bcd, int n)
        {
            return Convert.ToBoolean(bcd & (1 << n));
        }

        // init: input BCD,display digit
        public SSD_match(byte bcd)
        {
            InitializeComponent();
            displayDigit(bcd);
        }

        // init: input char, display 
        public SSD_match(char c)
        {
            InitializeComponent();
            display(c);
        }
    }
}
