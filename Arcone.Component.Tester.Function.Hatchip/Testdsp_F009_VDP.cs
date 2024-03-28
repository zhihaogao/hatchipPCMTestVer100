
using Arcone.ComAdapter.Instruments;
using Arcone.Comm.Models.Tester;
using Arcone.Comm.Models.TesterCurve;
using Arcone.Component.Tester.Att;
using Arcone.Component.Instruments;
using System;
using System.Threading;
using Arcone.Comm.Helper.System;
using System.Linq;
using System.Collections.Generic;

namespace Arcone.Component.Tester.Function.Hatchip
{
    public partial class TESTDISP
    {
        [Characteristic(Name = "VDP")]
        [CharacteristicVariable(Name = "R", Unit = "ohm", Min = 0, Max = 0, LLimit = 0, HLimit = 0, Ratio = 0, Sigma = 0)]
        //[CharacteristicParameter(Code = "CurveNum")]
        [CharacteristicParameter(Code = "SweepSMU", IsShow = false)]
        [CharacteristicParameter(Code = "StepHoldTime", IsShow = false)]
        [CharacteristicParameter(Code = "StepDelayTime", Name = "DelayTime(ms)", Desc = "Measure Delay Time[0 to 65535]", DefaultValue = "0",
            ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
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
        [CharacteristicParameter(Code = "SmuSetValue0", IsShow = false)]
        [CharacteristicParameter(Code = "SmuSetValue1", IsShow = false)]
        [CharacteristicParameter(Code = "SmuSetValue2", IsShow = false)]
        [CharacteristicParameter(Code = "SmuSetValue3", IsShow = false)]
        [CharacteristicParameter(Code = "SmuSetValue0", Name = "Force I", Desc = "force I", DefaultValue = "10E-3",
            ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "Compliance1", Name = "Compliance2", Desc = "measure1 compliance", DefaultValue = "20",
            ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "Compliance2", Name = "Compliance3", Desc = "measure2 compliance", DefaultValue = "20",
            ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "Compliance3", IsShow = false)]
        [CharacteristicParameter(Code = "Para2", Name = "AtIVR", Desc = "get the I VR", DefaultValue = "0",
           ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "Para3", Name = "SamplesNum", Desc = "measurement average samples number[0~100]", DefaultValue = "0",
           ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "Para4", IsShow = false)]
        [CharacteristicParameter(Code = "ForceSMU", Name = "Sweep SMU", Desc = "select sweep smu", DefaultValue = "1#1,SMU1;3,SMU2;4,SMU3;5,SMU4",
            X = 220, Y = 135, Width = 100, ControlType = ParameterLayoutControlType.DropList, DataType = ParameterLayoutDataType.Int)]
        [CharacteristicParameter(Code = "GNDSMU", Name = "GND SMU", Desc = "select GND smu", DefaultValue = "5#0,-NULL-;1,SMU1;3,SMU2;4,SMU3;5,SMU4",
            X = 220, Y = 200, Width = 100, ControlType = ParameterLayoutControlType.DropList, DataType = ParameterLayoutDataType.Int)]
        [CharacteristicParameter(Code = "M1SMU", Name = "Measure1 SMU", Desc = "select measure smu", DefaultValue = "3#1,SMU1;3,SMU2;4,SMU3;5,SMU4",
            X = 220, Y = 265, Width = 100, ControlType = ParameterLayoutControlType.DropList, DataType = ParameterLayoutDataType.Int)]
        [CharacteristicParameter(Code = "M2SMU", Name = "Measure2 SMU", Desc = "select measure smu", DefaultValue = "4#1,SMU1;3,SMU2;4,SMU3;5,SMU4",
            X = 220, Y = 330, Width = 100, ControlType = ParameterLayoutControlType.DropList, DataType = ParameterLayoutDataType.Int)]
        public void characteristics9(bool IsTestingMode)
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
                double forceSmu_BIAS = Parameters.SmuSetValue0;
                double forceSmu_COMPLIANCE = StaticParameter.Compliance0;

                int M1SMU = Convert.ToInt32(Parameters.M1SMU);
                double M1SMU_BIAS = Parameters.SmuSetValue1;
                double M1SMU_COMPLIANCE = StaticParameter.Compliance1;
                M1SMU_BIAS = 0;

                int M2SMU = Convert.ToInt32(Parameters.M2SMU);
                double M2SMU_BIAS = Parameters.SmuSetValue2;
                double M2SMU_COMPLIANCE = StaticParameter.Compliance2;
                M2SMU_BIAS = 0;

                int gndSmu = Convert.ToInt32(Parameters.GNDSMU);
                double gndSmu_BIAS = Parameters.SmuSetValue3;
                double gndSmu_COMPLIANCE = StaticParameter.Compliance3;
                gndSmu_BIAS = 0;
                gndSmu_COMPLIANCE = 100E-3;
                double HoldTime = StaticParameter.StepHoldTime * 1E-3;
                double DelayTime = StaticParameter.StepDelayTime * 1E-3;

                int av = Convert.ToInt32(StaticParameter.Para3);

                HP4142.Reset();
                _ = gndSmu > 0 ? HP4142.Send($"CN{forceSmu},{M1SMU},{M2SMU},{gndSmu}") : HP4142.Send($"CN{forceSmu},{M1SMU},{M2SMU}");
                HP4142.Send($"DI {forceSmu},0,{forceSmu_BIAS},{forceSmu_COMPLIANCE}");
                HP4142.Send($"DI {M1SMU},0,{M1SMU_BIAS},{M1SMU_COMPLIANCE}");
                HP4142.Send($"DI {M2SMU},0,{M2SMU_BIAS},{M2SMU_COMPLIANCE}");
                HP4142.Send($"DV {gndSmu},0,{gndSmu_BIAS},{gndSmu_COMPLIANCE}");
                _ = (av > 0) ? HP4142.Send($"AV {av},0") : 0;
                _ = gndSmu > 0 ? HP4142.Send($"MM 1,{forceSmu},{M1SMU},{M2SMU},{gndSmu}"): HP4142.Send($"MM 1,{forceSmu},{M1SMU},{M2SMU}");
                HP4142.Send("XE");

                if (DelayTime > 0) Thread.Sleep((int)DelayTime);

                double smu1V = HP4142.ReadDouble();
                double smu2V = HP4142.ReadDouble();
                double smu3V = HP4142.ReadDouble();

                double smu4I = forceSmu_BIAS;
                if (gndSmu > 0)
                    smu4I = HP4142.ReadDouble();

                double I = forceSmu_BIAS;
                if (I.AlmostEqual(smu4I))
                    I = smu4I;

                double V = Math.Abs(smu3V - smu2V);
                double R = V / I;
                curve0.GraphAppendPoint = new GraphPointViewModel() { X = I, Y = V };
                Vars[0] = smu2V;
                Vars[1] = smu3V;
                Vars[0] = R;

            }
        }
    }
}


