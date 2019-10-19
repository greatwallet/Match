using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

// Seven-Segment-Display Digit
namespace Match
{
    // Remove or Place: Attribute of Action and State
    enum Action
    {
        Remove = 0, Place = 1
    };
    class SSD
    {
        // Binary Coded decimal
        public byte BCD;
        
        /* init function */
        // No input, the BCD is set to Null.
        public SSD()
        {
            BCD = 0x00;
        }

        // input = integer
        public SSD(int d)
        {
            BCD = digit2binary(d);
        }

        // input = character of digit
        public SSD(char c)
        {
            BCD = digit2binary(Convert.ToByte(c - '0'));
        }
        // input = BCD of the SSD
        public SSD(byte bcd)
        {
            BCD = bcd;
        }
        // Convert digit to BCD
        public static byte digit2binary(int d)
        {
            switch(d)
            {
                case 0: return 0x3f;
                case 1: return 0x06;
                case 2: return 0x5b;
                case 3: return 0x4f;
                case 4: return 0x66;
                case 5: return 0x6d;
                case 6: return 0x7d;
                case 7: return 0x07;
                case 8: return 0x7f;
                case 9: return 0x6f;
                default: throw new Exception("Error in digit2Binary");
            }
        }

        // Convert BCD to digit,if b is not a valid form for a digit, return -1
        public static int binary2digit(byte b)
        {
            switch (b)
            {
                case 0x3f: return 0;
                case 0x06: return 1;
                case 0x5b: return 2;
                case 0x4f: return 3;
                case 0x66: return 4;
                case 0x6d: return 5;
                case 0x7d: return 6;
                case 0x07: return 7;
                case 0x7f: return 8;
                case 0x6f: return 9;
                default: return -1;//throw new Exception("Error in binary2digit");
            }
        }

        // Get the digit value of current BCD
        public int getVal()
        {
            return binary2digit(BCD);
        }

        /* re-define operator equal & non-equal */
        public static bool operator ==(SSD a, SSD b)
        {
            return a.BCD == b.BCD;
        }
        public static bool operator !=(SSD a, SSD b)
        {
            return a.BCD != b.BCD;
        }
        public bool Equals(SSD rt)
        {
            return BCD == rt.BCD;
        }

        //Set a bit to zero of the BCD.
        // 0<=n<=7; means the position of the bit 
        private byte _setZeroBit(int n)
        {
            Debug.Assert(n >= 0 && n <= 7);
            return Convert.ToByte(BCD & (~(0x01 << n)));
        }

        //Set a bit to one of the BCD.
        // 0<=n<=7; means the position of the bit 
        private byte _setOneBit(int n)
        {
            Debug.Assert(n >= 0 && n <= 7);
            return Convert.ToByte(BCD | (0x01 << n));
        }

        // expand the SSD by removing or placing ONE match
        public List<SSD> expand(Action act)
        {
            var children = new List<SSD>();
            // Remove one match
            if (act == Action.Remove)
            {
                for (int idx = 0; idx <= 6; idx++)
                {
                    // generate new ssd after removing 
                    byte bcd_rm = _setZeroBit(idx);
                    // Note: should not completely remove the match
                    if (bcd_rm == BCD || bcd_rm == 0) continue;
                    children.Add(new SSD(bcd_rm));
                }
            }
            // Place one match
            else
            {
                for (int idx = 0; idx <= 6; idx++)
                {
                    // generate new ssd after placing 
                    byte bcd_pl = _setOneBit(idx);
                    if (bcd_pl == BCD) continue;
                    children.Add(new SSD(bcd_pl));
                }
            }
            return children;
        }

        // count how many '1's there is in the byte
        public static int countOnes(byte bcd)
        {
            int count = 0;
            for (int i = 0; i < 7; i++) 
                count += (bcd >> i) & 1;
            return count;
        }
        // difference between 2 SSDs
        public static int diff(byte left, byte right)
        {
            return countOnes(Convert.ToByte(left ^ right));
        }
        public static int diff(SSD left, SSD right)
        {
            return countOnes(Convert.ToByte(left.BCD ^ right.BCD));
        }
        /* Functions aids to cut branches */

        // get the "Match Loss" of the digit
        // public int match_loss()
    }
}
