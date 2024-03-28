
using Arcone.ComAdapter.Instruments;
using Arcone.Comm.Models.Tester;
using Arcone.Comm.Models.TesterCurve;
using Arcone.Component.Tester.Att;
using Arcone.Component.Instruments;
using System;
using System.Threading;
using Arcone.Comm.Helper.System;
using System.Linq;

namespace Arcone.Component.Tester.Function.Hatchip
{
    public partial class TESTDISP
    {
        /// <summary>
        /// 加电流，测电压，算电阻
        /// </summary>
        /// <param name="IsTestingMode"></param>
        [Characteristic(Name = "DV-MI-R")]
        [CharacteristicVariable(Name = "ForceI", Unit = "A", Min = 0, Max = 0, LLimit = 0, HLimit = 0, Ratio = 0, Sigma = 0)]
        [CharacteristicVariable(Name = "GndI", Unit = "A", Min = 0, Max = 0, LLimit = 0, HLimit = 0, Ratio = 0, Sigma = 0)]
        [CharacteristicVariable(Name = "R", Unit = "ohm", Min = 0, Max = 0, LLimit = 0, HLimit = 0, Ratio = 0, Sigma = 0)]
        //[CharacteristicParameter(Code = "CurveNum")]
        [CharacteristicParameter(Code = "SweepSMU", IsShow = false)]
        [CharacteristicParameter(Code = "StepHoldTime", IsShow = false)]
        [CharacteristicParameter(Code = "StepDelayTime", IsShow = false)]
        [CharacteristicParameter(Code = "X1", IsShow = false)]
        [CharacteristicParameter(Code = "X2", IsShow = false)]
        [CharacteristicParameter(Code = "XStep", IsShow = false)]
        [CharacteristicParameter(Code = "DotNum", IsShow = false)]
        [CharacteristicParameter(Code = "TLM0", IsShow = false)]
        [CharacteristicParameter(Code = "TLM1", IsShow = false)]
        [CharacteristicParameter(Code = "TLM2", IsShow = false)]
        [CharacteristicParameter(Code = "TLM3", IsShow = false)]
        [CharacteristicParameter(Code = "TLM4", IsShow = false)]
        [CharacteristicParameter(Code = "TLM5", IsShow = false)]
        [CharacteristicParameter(Code = "TLM6", IsShow = false)]
        [CharacteristicParameter(Code = "TLM7", IsShow = false)]
        [CharacteristicParameter(Code = "TLM8", IsShow = false)]
        [CharacteristicParameter(Code = "TLM9", IsShow = false)]
        [CharacteristicParameter(Code = "SmuSetValue0", Name = "Force SET(V)", Desc = "Force SMU SET", DefaultValue = "1",
            ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "SmuSetValue1", IsShow = false)]
        [CharacteristicParameter(Code = "SmuSetValue2", IsShow = false)]
        [CharacteristicParameter(Code = "SmuSetValue3", IsShow = false)]
        [CharacteristicParameter(Code = "Compliance0", Name = "Force COMP(I)", Desc = "Force SMU compliance", DefaultValue = "0.001",
            ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "Compliance1", IsShow = false)]
        [CharacteristicParameter(Code = "Compliance2", IsShow = false)]
        [CharacteristicParameter(Code = "Compliance3", IsShow = false)]
        [CharacteristicParameter(Code = "Para2", IsShow = false)]
        [CharacteristicParameter(Code = "Para3", Name = "SamplesNum", Desc = "measurement average samples number[0~100]", DefaultValue = "0", ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "Para4", IsShow = false)]
        [CharacteristicParameter(Code = "ForceSMU", Name = "Force SMU", Desc = "select force smu", DefaultValue = "1#1,SMU1;3,SMU2;4,SMU3;5,SMU4",
            X = 220, Y = 135, Width = 100, ControlType = ParameterLayoutControlType.DropList, DataType = ParameterLayoutDataType.Int)]
        [CharacteristicParameter(Code = "GNDSMU", Name = "GND SMU", Desc = "select GND smu", DefaultValue = "4#0,-NULL-;1,SMU1;3,SMU2;4,SMU3;5,SMU4",
            X = 220, Y = 200, Width = 100, ControlType = ParameterLayoutControlType.DropList, DataType = ParameterLayoutDataType.Int)]
        public void characteristics2(bool IsTestingMode)
        {
            //step1.设置图表样式
            //Graph.CurveNumber  表示曲线数量，应该再测试方法设置里设置
            //获取曲线，从0开始；可以有多条曲线，最多能获取到CurveNumber条
            var curve0 = Graph.TryGetCurve();
            curve0.AxisXTitleVisiable = false;   //不设置标题，应设置不显示
            curve0.AxisYTitleVisiable = false;   //不设置标题，应设置不显示
            curve0.LegendVisiable = false;   //不设置图例，应设置不显示

            //测试模式，数据赋值
            if (IsTestingMode)
            {
                int forceSmu = Convert.ToInt32(Parameters.ForceSMU);
                int gndSmu = Convert.ToInt32(Parameters.GNDSMU);
                double compliance = StaticParameter.Compliance0; //或者也能得到//Parameters.Compliance0
                double biasV = Parameters.SmuSetValue0;
                int av = Convert.ToInt32(StaticParameter.Para3);

                HP4142.Reset();
                _ = gndSmu > 0 ? HP4142.Send($"CN{forceSmu},{gndSmu}") : HP4142.Send($"CN{forceSmu}");
                _ = HP4142.Send($"DV{forceSmu},0,{biasV},{compliance}");
                _ = gndSmu > 0 ? HP4142.Send($"DV{gndSmu},0,{0},{100E-3}") : 0;
                _ = (av > 0) ? HP4142.Send($"AV {av},0") : 0;
                _ = gndSmu > 0 ? HP4142.Send($"MM1,{forceSmu},{gndSmu}"): HP4142.Send($"MM1,{forceSmu}");
                HP4142.Send("XE");

                Thread.Sleep(1);

                double forceMeasure = Math.Abs(HP4142.ReadDouble());
                double gndMeasure = _ = gndSmu > 0 ? Math.Abs(HP4142.ReadDouble()) : 0;

                double V = biasV;
                double R = V / forceMeasure;
                curve0.GraphAppendPoint = new GraphPointViewModel() { X = forceMeasure, Y = V };

                Vars[0] = forceMeasure;
                Vars[1] = gndMeasure;
                Vars[2] = R;
            }
        }
    }
}


