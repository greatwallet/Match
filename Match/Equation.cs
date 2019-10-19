using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Match
{
    // Level for generating equation
    enum Level
    {
        EASY = 0, // '+','-',1-digit 
        MEDIUM = 1, // '+','-','*','/', 1-digit
        HARD = 2 // '+','-','*','/', 2-digit or above
    };
    // The state of the search algorithm
    class Equation
    {
        // operators
        readonly static char[] OPTR = new char[4] { '+', '-', '*', '/' };

        static Random rnd = new Random();

        // timer used to measure time
        static Stopwatch sw = new Stopwatch();

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
        private static bool sameAsGrandpa(Equation p, int ssd_idx)
        {
            // if p is in the first 2 layers, then he does not have grandpa
            if (p.get_depth() < 2) return false;
            Equation grandpa = p.anncestors[1];
            return p.ssds[ssd_idx] == grandpa.ssds[ssd_idx];
        }

        // expand One Possible random equation List, define its ancestors, attribute, ssds and optr
        private static void expand(Equation father, out List<Equation> children)
        {
            children = new List<Equation>();

            for (int i = 0; i < father.ssds.Count; i++)
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

                    // optr
                    son.optr = new Dictionary<int, char>(father.optr);
                    // attribute
                    son.Attribute = inverse(father.Attribute);
                    // ancestors
                    son.anncestors = new List<Equation>(father.anncestors);
                    son.anncestors.Insert(0, father);

                    //* Cut branch Operation*/
                    // if same as grandpa: (means it goes back! ) then should be cut!
                    if (sameAsGrandpa(son, i)) continue;
                    // if the son's depth = 3, and son's ssds[i] == grandpa's ssds[i], then cut!
                    // TO TEST
                    if (father.get_depth() == 2 && son.ssds[i] == father.anncestors[0].ssds[i])
                    {
                        continue;
                    }

                    children.Add(son);
                }
            }

        }

        // shuffle a list
        public static void Shuffle(ref List<Equation> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                Equation value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        // expand with random seed
        private static void randomExpand(Equation father, out List<Equation> children)
        {
            expand(father, out children);
            Shuffle(ref children);
        }

        // Exist Function
        private static bool exist(ref List<Equation> equ_list, ref Equation obj)
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
        public static void Search(ref Equation root, out List<Equation> ans_list, int movMatch = 1, int max_ans_num = 10, double MAX_TIME = 3.0)
        {
            root.Attribute = Action.Remove;
            ans_list = new List<Equation>();

            //start watch
            sw.Restart();
            
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
                // check timer
                if (sw.Elapsed.TotalSeconds > MAX_TIME)
                { sw.Stop(); return; }
                
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
                    if (isCorrect(son) && !exist(ref ans_list, ref son)) ans_list.Add(son);
                }
            }
            sw.Stop();
        }

        public static void Search(ref string equ_str, out List<Equation> ans_list, int movMatch = 1, int max_ans_num = 10, double MAX_TIME = 3.0)
        {
            Equation root;
            str2ssd(ref equ_str, out root);
            Search(ref root, out ans_list, movMatch, max_ans_num, MAX_TIME);
        }

        public static void Search(ref string equ_str, out List<string> ans_str_list, int movMatch = 1, int max_ans_num = 10, double MAX_TIME = 3.0)
        {
            Equation root;
            str2ssd(ref equ_str, out root);
            List<Equation> ans_list;
            Search(ref root, out ans_list, movMatch, max_ans_num, MAX_TIME);

            ans_str_list = new List<string>(ans_list.Count);

            for (int i = 0; i < ans_list.Count; i++)
            {
                Equation src = ans_list[i];
                string dest;
                if (ssd2str(ref src, out dest))
                    ans_str_list.Add(dest);
            }
        }

        /* Generate algorithm */

        // generate correct expression, return the answer of the expression
        private static int _generateExprSrc(Level lv, out string expr)
        {
            expr = "";
            // Random rnd = new Random();
            // easy: 1 digit; <= 3 operands, '+', '-'

            int MAX_OPND_NUM, MAX_VAL, MAX_IDX_OPTR;
            if (lv == Level.EASY)
            {
                //  operands' amount
                MAX_OPND_NUM = 3;
                // maximum number
                MAX_VAL = 9;
                // maxium idx for operator
                MAX_IDX_OPTR = 2;
            }

            else if(lv == Level.MEDIUM)
            {
                //  operands' amount
                MAX_OPND_NUM = 4;
                // maximum number
                MAX_VAL = 9;
                // maxium idx for operator
                MAX_IDX_OPTR = 3;
            }
            else
            {
                //  operands' amount
                MAX_OPND_NUM = 5;
                // maximum number
                MAX_VAL = 99;
                // maxium idx for operator
                MAX_IDX_OPTR = 4;
            }
            // opnd's amount
            int num_opnd = rnd.Next(2, MAX_OPND_NUM + 1);
            // operators' amount
            int num_optr = num_opnd - 1;
            int[] opnd = new int[num_opnd];
            char[] optr = new char[num_optr];
            // set up opnd
            for (int i = 0; i < num_opnd; i++) 
                opnd[i] = rnd.Next(MAX_VAL + 1);
            // set up oprt
            for (int i = 0; i < num_optr; i++)
                optr[i] = OPTR[rnd.Next(MAX_IDX_OPTR)];

            // get result 
            
            //int ans = _getResult(ref opnd, ref optr);
            for (int i = 0; i < num_optr; i++)
            {
                expr += opnd[i].ToString();
                expr += optr[i].ToString();
            }
            expr += opnd[opnd.Length - 1].ToString();
            int ans = Expr.evaluate(expr);
            expr += "=";
            expr += ans.ToString();
            return ans;
        }

        // generate correct expression util the result is positive
        public static void generateExprSrc(Level lv, out string expr)
        {
            while (true)
                if (_generateExprSrc(lv, out expr) >= 0) break;
        }


        // Check if the equation is valid
        public static bool isValidEqu(string equ)
        {
            string left, right;
            Equation.splitEqu(ref equ, out left, out right);
            // no equal or no right expression
            if (right == "") return false;
            // right expression must be an integer
            int _;
            if (!int.TryParse(right, out _)) return false;
            // make sure the generate answer is not negative
            if (_ < 0) return false;

            // try solve the left
            try
            {
                int ans = Expr.evaluate(left);
                // if answer is negative, then cannot be handled
                if (ans < 0) return false;
            }
            catch (Exception)
            { return false; }
            return true;
        }
        private static bool isValidEqu(Equation equ)
        {
            string equ_str;
            // Try transform
            bool trans_flag = ssd2str(ref equ, out equ_str);
            if (trans_flag == false) return false;
            else return isValidEqu(equ_str);
        }

        // count the number of difference of matches
        private static int matchDiff(ref Equation root, ref Equation result)
        {
            // Assert: root.optr == result.optr
            Debug.Assert(root.ssds.Count == result.ssds.Count);
            int count = 0;
            for (int i = 0; i < root.ssds.Count; i++)
                count += SSD.diff(root.ssds[i], result.ssds[i]);
            return count;
        }

        // Generate a puzzle, if found return true, else false
        // TO TEST
        public static bool GenerateSearch(ref Equation root, out Equation puzzle, int movMatch = 1)
        {
            root.Attribute = Action.Remove;
            puzzle = new Equation();

            // Set up Open and Close Table
            List<Equation> Open = new List<Equation>(), Close = new List<Equation>();
            // mark if the puzzle is found

            /*            深度优先搜索算法：         */

            //（1）把起始节点S 放到未扩展节点Open 表中。如果此节点为一目标节点，则得到一
            //个解。
            Open.Add(root);
            //（2）如果Open 为一空表，或ans_list数量超过一定范围，则失败退出。
            while (Open.Count > 0)
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
                randomExpand(cur_equ, out children);
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
                    if (isValidEqu(son))
                    {
                        // Optional: check if the result are "strictly" valid
                        if (matchDiff(ref root, ref son) != 2 * movMatch) continue;

                        puzzle.ssds = new List<SSD>(son.ssds);
                        puzzle.Attribute = Action.Remove;
                        puzzle.optr = new Dictionary<int, char>(son.optr);
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool GenerateSearch(ref string equ_str, out string puzzle_str, int movMatch = 1)
        {
            Equation root;
            str2ssd(ref equ_str, out root);
            Equation puzzle;
            GenerateSearch(ref root, out puzzle, movMatch);
            return ssd2str(ref puzzle,out puzzle_str);
        }

        // Generate Core Function
        public static bool Generate(Level lv, out string dest_expr, int movMatch = 1)
        {
            string src_expr;
            generateExprSrc(lv, out src_expr);
            return GenerateSearch(ref src_expr, out dest_expr, movMatch);
        }
    }
}
