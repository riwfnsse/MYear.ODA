/* 1.分布式数据库：目前最比较稳定有效的算法是 Paxos 可以保证各个数据库数据一致。
 * 2.分布式事务比较难做而且性能不理想，所以暂时不体做分布式数据库。
 * 目前的分布式事务解决方案是二阶段提交（PreCommit、doCommit）或三阶段提交（CanCommit、PreCommit、doCommit）。
 * 跨库事务一般都是数据库层面考虑。
*/
using System;
using System.Collections.Generic;

namespace MYear.ODA
{
    internal delegate void ODATransactionEventHandler();
    /// <summary>
    /// ODA事务
    /// </summary>
    internal class ODATransaction
    {
        private System.Timers.Timer Tim = null; 
        private event ODATransactionEventHandler _DoCommit;
        private event ODATransactionEventHandler _DoRollBack;

      
        /// <summary>
        /// 启动了事务的DB,为分布式
        /// </summary>
        public Dictionary<string,IDBAccess> TransDB { get; } = new Dictionary<string, IDBAccess>();
        public string CurrentObject { get; set; }
        public event ODATransactionEventHandler CanCommit;
        public event ODATransactionEventHandler PreCommit;
        public event ODATransactionEventHandler DoCommit
        {
            add
            {
                if (_DoCommit != null)
                {
                    Delegate[] dls = _DoCommit.GetInvocationList();
                    foreach (Delegate dl in dls)
                        if (dl.Method == value.Method)
                            return;
                }
                _DoCommit += value;
            }
            remove
            {
                if (_DoCommit != null)
                    _DoCommit -= value;
            }
        }
        public event ODATransactionEventHandler RollBacking
        {
            add
            {
                if (_DoRollBack != null)
                {
                    Delegate[] dls = _DoRollBack.GetInvocationList();
                    foreach (Delegate dl in dls)
                        if (dl.Method == value.Method)
                            return;
                }
                _DoRollBack += value;
            }
            remove
            {
                if (_DoRollBack != null)
                    _DoRollBack -= value;
            }
        }

        public Action TransactionTimeOut;

        public string TransactionId { get; private set; }
        public bool IsTimeout { get; private set; } = false;
        internal ODATransaction(int TimeOut)
        {
            TransactionId = Guid.NewGuid().ToString("N");
            Tim = new System.Timers.Timer(TimeOut * 1000);
            Tim.Elapsed += new System.Timers.ElapsedEventHandler(Tim_Elapsed);
            Tim.Start();
        }
        private void Tim_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            IsTimeout = true;
            TransactionTimeOut?.Invoke();
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            //分布式事务，二阶段提交，或三阶段提交
            //暂不支持
            try
            {
                DisposeTimer();
                CanCommit?.Invoke();
                PreCommit?.Invoke();
                _DoCommit?.Invoke();
            }
            finally
            {
                CanCommit = null;
                PreCommit = null;
                _DoCommit = null;
            } 
        }
        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollBack()
        {
            try
            {
                DisposeTimer();
                _DoRollBack?.Invoke();
            }
            finally
            {
                _DoRollBack = null;
            }
        }
        private void DisposeTimer()
        { 
            if (Tim != null)
            {
                if (Tim.Enabled)
                    Tim.Stop();
                Tim.Close();
                Tim.Dispose();
                Tim = null;
            }
        }
    }
}
