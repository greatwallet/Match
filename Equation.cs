using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Match
{
    // The state of the search algorithm
    class Equation
    {
        // ssd of digits
        private List<SSD> ssds;

        // Positions and char of operators in the original expression
        private Dictionary<int, char> optr;

        // The next action of the State: place or remove?
        private Action Attribute;

        // anncestors
        private List<Equation> anncestors;

        // get depth of the node in the graph
        private int get_depth()
        {
            return anncestors.Count;
        }

        // private static Random rnd;
        private static bool isdigit(char c)
        {
            return (c - '0') >= 0 && (c - '0') <= 9;
        }

        /* Transformation between string & ssd */
        // from string to ssd 
        public static void str2ssd(ref string src, out List<SSD> dest_ssd, out Dictionary<int, char> dest_optr)
        {
            dest_ssd = new List<SSD>(); dest_optr = new Dictionary<int, char>();
            dest_ssd.Clear(); dest_optr.Clear();
            for (int i = 0; i < src.Length; i++)
            {
                if (isdigit(src[i])) dest_ssd.Add(new SSD(src[i]));
                else dest_optr.Add(i, src[i]);
            }
        }

        public static void str2ssd(ref string src, out Equation dest)
        {
            dest = new Equation();
            str2ssd(ref src, out dest.ssds, out dest.optr);
        }

        private static char digit2char(int d)
        {
            Debug.Assert(d >= 0 && d <= 9);
            return Convert.ToChar(d + '0');
        }

        // from ssd to string, if transform successfully return True, else False
        // TO TEST!
        public static bool ssd2str(ref List<SSD> src_ssd, ref Dictionary<int, char> src_optr, out string dest)
        {
            // string length
            int length = src_ssd.Count() + src_optr.Count();
            char[] dest_chars = new char[length];
            // Place operators
            foreach (var optr in src_optr)
                dest_chars[optr.Key] = optr.Value;

            var optr_idx_list = new List<int>(src_optr.Keys);
            // Traverse string, place OPTR or OPND according to src.
            for (int str_idx = 0, ssd_j = 0, optr_keys_k = 0; str_idx < length; str_idx++)
            {
                // if optr_keys not out of index and optr_keys goes right with current idx
                if (optr_keys_k < optr_idx_list.Count && optr_idx_list[optr_keys_k] == str_idx)
                {
                    int optr_idx = optr_idx_list[optr_keys_k];
                    // place operator
                    if (str_idx == optr_idx)
                    {
                        dest_chars[str_idx] = src_optr[optr_idx];
                        optr_keys_k++;
                    }
                    // place operand
                }
                else
                {
                    int d = src_ssd[ssd_j].getVal();
                    // check if ssd operand is valid 
                    if (d < 0 || d > 9)
                    {
                        dest = "";
                        return false;
                    }
                    dest_chars[str_idx] = digit2char(d);
                    ssd_j++;
                }
            }
            dest = new string(dest_chars);
            return true;
        }
        public static bool ssd2str(ref Equation src, out string dest)
        {
            return ssd2str(ref src.ssds, ref src.optr, out dest);
        }
        // init: transform a string equation to ssd form.
        public Equation()
        {
            // rnd = new Random();
            ssds = new List<SSD>();
            optr = new Dictionary<int, char>();
            anncestors = new List<Equation>();
        }
        public Equation(string equ, Action attr)
        {
            ssds = new List<SSD>();
            optr = new Dictionary<int, char>();
            //rnd = new Random();

            Attribute = attr;
            anncestors = new List<Equation>();

            str2ssd(ref equ, out ssds, out optr);
        }

        // split the equation into expression, "=" and result
        public static void splitEqu(ref string src, out string left, out string right)
        {
            left = ""; right = "";
            for (int i = src.Length - 1; i >= 0; i--)
            {
                if (src[i] == '=')
                {
                    left = src.Substring(0, i);
                    right = src.Substring(i + 1);
                    return;
                }
            }
        }

        // check if an equation is right,input equ
        public static bool isCorrect(ref string equ)
        {
            string lf, rt;
            splitEqu(ref equ, out lf, out rt);
            int true_ans = Expr.evaluate(lf);
            int search_ans;
            bool parse_flag = int.TryParse(rt, out search_ans);
            if (parse_flag == false) return false;
            else return search_ans == true_ans;
        }

        // check if an equation is right,input ssd
        // TO TEST!
        public static bool isCorrect(ref List<SSD> ssd, ref Dictionary<int, char> optr)
        {
            string equ;
            // Try transform
            bool trans_flag = ssd2str(ref ssd, ref optr, out equ);
            if (trans_flag == false) return false;
            else return isCorrect(ref equ);
        }

        // check if an equation is right,input ssd
        // TO TEST!
        public static bool isCorrect(Equation equ)
        {
            string equ_str;
            // Try transform
            bool trans_flag = ssd2str(ref equ, out equ_str);
            if (trans_flag == false) return false;
            else return isCorrect(ref equ_str);
        }

        // inverse the action
        public static Action inverse(Action act)
        {
            switch (act)
            {
                case Action.Place: return Action.Remove;
                case Action.Remove: return Action.Place;
                default: throw new Exception("Error in inverse!");
            }
        }

        /*       //// randomly choose an item from a list
               //// TO TEST!
               //public static SSD choice (List<SSD> ssd_list)
               //{
               //    Debug.Assert(ssd_list.Count > 0);
               //    Random rnd = new Random();
               //    int idx = rnd.Next(ssd_list.Count);
               //    return ssd_list[idx];
               //}*/


        // check if the grandson has the same list as its grandpa! 
        private static bool sameAsGrandpa(Equation p,int ssd_idx)
        {
            // if p is in the first 2 layers, then he does not have grandpa
            if (p.get_depth() < 2) return false;
            Equation grandpa = p.anncestors[1];
            return p.ssds[ssd_idx] == grandpa.ssds[ssd_idx];
        }

        // expand One Possible random equation List, define its ancestors, attribute, ssds and optr
        // TO TEST!
        private static void expand(Equation father,out List<Equation> children)
        {
            children = new List<Equation>();
            
            for (int i = 0 ; i < father.ssds.Count; i++)
            {
                SSD cur_ssd = father.ssds[i];
                
                // expand curent ssd 
                var cur_ssd_list = cur_ssd.expand(father.Attribute);

                for (int j = 0; j < cur_ssd_list.Count; j++)
                {
                    Equation son = new Equation();
                    // ssds 
                    son.ssds = new List<SSD>(father.ssds);
                    // change the i'th ssd to a new one.
                    son.ssds[i] = cur_ssd_list[j];

                    //* Cut branch Operation*/
                    // if same as grandpa: (means it goes back! ) then should be cut!
                    if (sameAsGrandpa(son, i)) continue;

                    // optr
                    son.optr = new Dictionary<int, char>(father.optr);
                    // attribute
                    son.Attribute = inverse(father.Attribute);
                    // ancestors
                    son.anncestors = new List<Equation>(father.anncestors);
                    son.anncestors.Insert(0, father);
                    children.Add(son);
                }
            }

        }

        // Exist Function
        private static bool exist(ref List<Equation> equ_list,ref Equation obj)
        {
            bool flag = true;
            foreach (var equ in equ_list)
            {
                flag = true;
                for (int i = 0; i < equ.ssds.Count; i++) 
                {
                    flag = flag & equ.ssds[i] == obj.ssds[i];
                    if (equ.ssds[i] != obj.ssds[i])
                        break;
                }
                if (flag == true) return true;
            }
            return false;
        }

        /* Search algorithm */
        // TO TEST!
        public static void Search(ref Equation root, out List<Equation> ans_list, int movMatch = 1,int max_ans_num = 10)
        {
            root.Attribute = Action.Remove;
            ans_list = new List<Equation>();

            // Set up Open and Close Table
            List<Equation> Open = new List<Equation>(), Close = new List<Equation>();

            /*            深度优先搜索算法：         */

            //（1）把起始节点S 放到未扩展节点Open 表中。如果此节点为一目标节点，则得到一
            //个解。
            Open.Add(root);
            if (isCorrect(root)) ans_list.Add(root);
            //（2）如果Open 为一空表，或ans_list数量超过一定范围，则失败退出。
            while (Open.Count > 0 && ans_list.Count < max_ans_num)
            {
                //（3）把Open 中第一个节点n 从Open 表移到Closed 表。
                Equation cur_equ = Open[0];
                Open.RemoveAt(0); Close.Add(cur_equ);
                // Check if Open[0].depth < 2 * movMatch
                if (cur_equ.get_depth() > 2 * movMatch) continue;

                //（4）扩展节点n ，产生其全部（时间太长，如何改进）后裔，对后裔生成其祖先、属性，(检查是否与前序节点一样)，并把它们放入Open 表的前端(末端的节点比前端
                //的节点后移出)。如果没有后裔，则转向步骤（2）。
                // 其中已经检查节点是否与祖父一样；
                List<Equation> children;
                expand(cur_equ, out children);
                Open.InsertRange(0, children);
                //（5）如果后继节点中有任一个为目标节点，则求得一个解，判断其Attributes == Action.Remove && depth == 2*MovMatch，
                // 成功退出；否则，转向步骤（2）。
                for (int i = 0; i < children.Count; i++) 
                {
                    Equation son = children[i];
                    // if "son" needs to be placed one match, algorithm continues!
                    if (son.Attribute == Action.Place) continue;
                    // if "son".depth != 2 * matches, then still not ending!
                    if (son.get_depth() != 2 * movMatch) continue;
                    // now check!
                    if (isCorrect(son) && !exist(ref ans_list,ref son)) ans_list.Add(son);
                }
            }
        }

        public static void Search(ref string equ_str, out List<Equation> ans_list, int movMatch = 1, int max_ans_num = 10)
        {
            Equation root;
            str2ssd(ref equ_str, out root);
            Search(ref root, out ans_list, movMatch, max_ans_num);
        }

        public static void Search(ref string equ_str,out List<string> ans_str_list,int movMatch = 1, int max_ans_num = 10)
        {
            Equation root;
            str2ssd(ref equ_str, out root);
            List<Equation> ans_list;
            Search(ref root, out ans_list, movMatch, max_ans_num);

            ans_str_list = new List<string>(ans_list.Count);

            for (int i = 0; i < ans_list.Count; i++)
            {
                Equation src = ans_list[i];
                string dest;
                if(ssd2str(ref src, out dest))
                    ans_str_list.Add(dest);
            }
        }
    }
}
