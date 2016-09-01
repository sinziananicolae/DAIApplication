using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIApplication.Models
{
    public class TestSubmissionModel
    {
        public int TestId;
        public int Time;
        public Dictionary<int, List<int>> Answers;
    }
}
