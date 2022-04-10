using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Conduit.Utilities
{
    public static class ThreadPoolTool
    {
        public static int MaxThreads;
        public static int MinThreads;
        public static void Setup()
        {
            MaxThreads = Environment.ProcessorCount * 4;
            MinThreads = 2;
            ThreadPool.SetMaxThreads(MaxThreads, MaxThreads);
            ThreadPool.SetMinThreads(MinThreads, MinThreads);
        }
    }
}
