using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer
{
    public static class CaptchaGenerator
    {
        public static string Generate(out int captchaValue)
        {
            Random r = new Random();
            int a = r.Next() % 10 + 1;
            int b = r.Next() % 10 + 1;
            int x = r.Next() % 10 + 1;
            int y = a * x + b;
            captchaValue = x;
            return String.Format("{0} = {1}*x + {2}", y, a, b);
        }
    }
}
