using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SlotsForCourseWork.DTO
{
    public class SpinDTO
    {
        public int A { get; }
        public int B { get; }
        public int C { get; }
        public int D { get; }
        public int WinType { get; set; }

        public SpinDTO(int a, int b, int c, int d)
        {
            A = a;
            B = b;
            C = c;
            D = d;
        }
    }
}
