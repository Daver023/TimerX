using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
namespace TimerControl
{
    public class CTimer
    {
        public static AsyncLocal<System.Timers.Timer> localTimer = new AsyncLocal<System.Timers.Timer>();

        public static AsyncLocal<int> Count = new AsyncLocal<int>();

        public static AsyncLocal<CancellationTokenSource> tokenSource = new AsyncLocal<CancellationTokenSource>();


        public static (System.Timers.Timer, CancellationTokenSource) Statrt(double interval, ElapsedEventHandler handler,Action<int, CancellationTokenSource> timeAction)
        {
            localTimer.Value = new System.Timers.Timer();
            tokenSource.Value = new CancellationTokenSource();

            Task.Run(() =>
                  {
                      Parallel.Invoke(

                      #region 执行事件
                          () =>
                          {
                              localTimer.Value.AutoReset = false;
                              localTimer.Value.Elapsed += handler;
                              localTimer.Value.Interval = interval;
                          },
                      #endregion

                      #region 执行时间
                     () =>
                      {
                          while (true)
                          {
                              if (tokenSource.Value.Token.IsCancellationRequested)
                              {
                                  localTimer.Value.Stop();
                                  Console.WriteLine("Task canceled");
                                  break;
                              }
                              if (Count.Value == null)
                              {
                                  Count.Value = 0;
                              }
                              Thread.Sleep(1000);
                              timeAction?.Invoke(++Count.Value, tokenSource.Value);
                          }
                      });
                      #endregion
                  });
            return (localTimer.Value, tokenSource.Value);
        }
    }
}
