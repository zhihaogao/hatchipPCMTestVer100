using Arcone.Component.Tester.Att;
using Arcone.Component.Tester.Function;
using System;
using System.Linq;
using System.Threading;

namespace Arcone.Component.Tester.Function.Hatchip
{
    /// <summary>
    /// 每个10个测试方法放一个文件，太多难于查找
    /// </summary>
    public partial class TESTDISP : TesterFunctionBase
    {
        TESTPROC _TP = null;
        public TESTPROC TP
        {
            get
            {
                if (_TP == null)
                    _TP = new TESTPROC();
                return _TP;
            }
            set
            {
                _TP = value;
            }
        }

        public TESTDISP()
        {
            TP = new TESTPROC();
        }
    }
}
