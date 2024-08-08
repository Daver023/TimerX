using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimerControl
{
    public static class TimerX
    {
        static AsyncLocal<CancellationTokenSource> _AsyncLocalToken = new AsyncLocal<CancellationTokenSource>();

        public static void Start(Func<bool> action, double interval)
        {
            var res = CTimer.Statrt(interval,
                    (s, e) =>
                    {
                        if (s is System.Timers.Timer timer)
                        {
                            if (action?.Invoke() == true)
                            {
                                timer.Start();
                            }
                            else
                            {
                                if (_AsyncLocalToken.Value is not null)
                                {
                                    _AsyncLocalToken.Value.Cancel();
                                }
                            }
                        }
                    },
                     (t, cts) =>
                     {
                         //Console.WriteLine($"任务1:已经花费{t}秒");
                     });
            res.Item1.Start();
            _AsyncLocalToken.Value = res.Item2;
        }
    }
}
