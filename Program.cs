using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Match
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Expr a = new Expr();
            // Console.WriteLine(a.evaluate("11+22-11*22+28/14"));
            //SSD a = new SSD(Convert.ToByte(0x15));
            //Console.WriteLine("{0:x4}",a._setOneBit(7));
            //string a, b, c;
            //a = "11+22*15-16/4=337"; b = ""; c = "";
            //Equation.splitEqu(ref a, out b, out c);
            //int r = Expr.evaluate(b);
            //bool t = Equation.isCorrect(ref a);

            // test for search
            string equ = "1+3*16=38";
            List<string> ans_list;
            Equation.Search(ref equ, out ans_list, 2, 5);

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainWindow());
        }
    }
}
