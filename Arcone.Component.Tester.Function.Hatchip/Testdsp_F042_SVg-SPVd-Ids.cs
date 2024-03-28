
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
using System.Reflection;

namespace Arcone.Component.Tester.Function.Hatchip
{
    public partial class TESTDISP
    {
        [Characteristic(Name = "SVg-SPVd-Ids")]
        [CharacteristicParameter(Code = "SweepSMU", Name = "Plus Period(ms)", Desc = "Period Time [Max(10, 2*(pulse width)),500],", DefaultValue = "10",
            ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Int)]
        [CharacteristicParameter(Code = "StepHoldTime", Name = "HoldTime(ms)", Desc = "Hold Time[0 to 655350]", DefaultValue = "0",
            ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Int)]
        [CharacteristicParameter(Code = "StepDelayTime", Name = "Plus Width(ms)", Desc = "Plus Width[1 to 50]", DefaultValue = "1",
            ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "X1", Name = "X1(D)", Desc = "d pulse sweep start value", DefaultValue = "0",
            ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "X2", Name = "X2(D)", Desc = "d pulse sweep stop value", DefaultValue = "10",
            ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "XStep", Name = "Step(D)", Desc = "d pulse sweep step", DefaultValue = "0.1",
            ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "DotNum", Name = "DotNum", Desc = "dot number", DefaultValue = "100",
            ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Int)]
        [CharacteristicParameter(Code = "TLM0", Name = "X1(G)", Desc = "g sweep start value", DefaultValue = "0",
            Width = 100, ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "TLM1", Name = "X2(G)", Desc = "g sweep stop value", DefaultValue = "-5",
            Width = 100, ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "TLM2", Name = "Step(G)", Desc = "g sweep step", DefaultValue = "-0.5",
            Width = 100, ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "TLM3", IsShow = false)]
        [CharacteristicParameter(Code = "TLM4", IsShow = false)]
        [CharacteristicParameter(Code = "TLM5", IsShow = false)]
        [CharacteristicParameter(Code = "TLM6", IsShow = false)]
        [CharacteristicParameter(Code = "TLM7", IsShow = false)]
        [CharacteristicParameter(Code = "TLM8", IsShow = false)]
        [CharacteristicParameter(Code = "TLM9", IsShow = false)]
        [CharacteristicParameter(Code = "SmuSetValue0", Name = "Base Current", Desc = "The start pulse current, stop pulse current and base current must be the same polarity.", DefaultValue = "0", ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "SmuSetValue1", IsShow = false)]
        [CharacteristicParameter(Code = "SmuSetValue2", IsShow = false)]
        [CharacteristicParameter(Code = "SmuSetValue3", IsShow = false)]
        [CharacteristicParameter(Code = "Compliance0", Name = "D COMP(I)", Desc = "D compliance", DefaultValue = "0.1", ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "Compliance1", Name = "G COMP(I)", Desc = "G compliance", DefaultValue = "0.1", ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "Compliance1", IsShow = false)]
        [CharacteristicParameter(Code = "Compliance2", IsShow = false)]
        [CharacteristicParameter(Code = "Compliance3", IsShow = false)]
        [CharacteristicParameter(Code = "Para2", IsShow = false)]
        [CharacteristicParameter(Code = "Para3", IsShow = false)]
        [CharacteristicParameter(Code = "Para4", IsShow = false)]
        [CharacteristicParameter(Code = "GSMU", Name = "SMU(G)", Desc = "select G smu", DefaultValue = "1#1,SMU1;3,SMU2;4,SMU3;5,SMU4", X = 220, Y = 135, Width = 100, ControlType = ParameterLayoutControlType.DropList, DataType = ParameterLayoutDataType.Int)]
        [CharacteristicParameter(Code = "DSMU", Name = "SMU(D)", Desc = "select D smu", DefaultValue = "4#1,SMU1;3,SMU2;4,SMU3;5,SMU4", X = 220, Y = 200, Width = 100, ControlType = ParameterLayoutControlType.DropList, DataType = ParameterLayoutDataType.Int)]
        [CharacteristicParameter(Code = "SSMU", Name = "SMU(S)", Desc = "select S smu", DefaultValue = "5#1,SMU1;3,SMU2;4,SMU3;5,SMU4", X = 220, Y = 265, Width = 100, ControlType = ParameterLayoutControlType.DropList, DataType = ParameterLayoutDataType.Int)]
        public void characteristics42(bool IsTestingMode)
        {
            //测试模式，数据赋值
            if (IsTestingMode)
            {
                double d_x1 = Parameters.X1;
                double d_x2 = Parameters.X2;
                double d_step = Parameters.XStep; //步长>0
                int d_dotnum = Parameters.DotNum + 1;

                double g_x1 = Parameters.TLM0;
                double g_x2 = Parameters.TLM1;
                double g_step = Parameters.TLM2; //步长>0
                int g_dotnum = (int)((g_x2 - g_x1) / g_step + 1.05);

                int GSMU = Convert.ToInt32(Parameters.GSMU);
                double GSMU_BIAS = Parameters.SmuSetValue1;
                double GSMU_COMPLIANCE = StaticParameter.Compliance1;

                int DSMU = Convert.ToInt32(Parameters.DSMU);
                double DSMU_BIAS = Parameters.SmuSetValue0;
                double DSMU_COMPLIANCE = StaticParameter.Compliance0;

                int SSMU = Convert.ToInt32(Parameters.SSMU);
                double SSMU_BIAS = Parameters.SmuSetValue2;
                double SSMU_COMPLIANCE = StaticParameter.Compliance2;
                SSMU_BIAS = 0;
                SSMU_COMPLIANCE = 100E-3;

                double hold_time = StaticParameter.StepHoldTime * 1E-3;
                double plus_width = StaticParameter.StepDelayTime * 1E-3;
                double plus_period = StaticParameter.SweepSMU * 1E-3;

                double base_current = Parameters.SmuSetValue0;
                double atI = Parameters.Para2;

                double min = 0, max = 0;
                for (int i = 0; i < g_dotnum; i++)
                {
                    GSMU_BIAS = g_x1 + i * g_step;

                    var result = new List<GraphPointViewModel>();
                    if (IsTestingMode)//测试模式，数据赋值
                    {
                        int sweep_mode = 1;
                        int output_range = 0;

                        if (d_x1 < 0 && d_x2 > 0)
                        {
                            var No_step = (int)((0 - d_x1) / d_step + 1.05);
                            HP4142.Reset();
                            HP4142.Send($"CN{GSMU},{DSMU},{SSMU}");
                            HP4142.Send("FL0");
                            HP4142.Send($"PT{hold_time},{plus_width},{plus_period}");
                            //PWI ch#, sweep mode, output range, base current, start pulse current,stop pulse current, number of steps[, V compliance]
                            HP4142.Send($"PWV{DSMU},{sweep_mode},{output_range},{base_current},{d_x1},{0},{No_step},{DSMU_COMPLIANCE}");
                            HP4142.Send($"DV{GSMU},0,{GSMU_BIAS},{GSMU_COMPLIANCE}");
                            HP4142.Send($"DV{SSMU},0,{SSMU_BIAS},{SSMU_COMPLIANCE}");
                            HP4142.Send($"MM4,{DSMU}");
                            HP4142.Send("XE");
                            Thread.Sleep(5);

                            for (int j = 0; j < No_step; j++)
                            {
                                DSMU_BIAS = d_x1 + j * d_step;
                                double Id = HP4142.ReadDouble();
                                if (j != No_step - 1)
                                    result.Add(new GraphPointViewModel() { X = DSMU_BIAS, Y = Id });
                            }

                            No_step = (int)((d_x2 - 0) / d_step + 1.05);
                            HP4142.Reset();
                            HP4142.Send($"CN{GSMU},{DSMU},{SSMU}");
                            HP4142.Send("FL0");
                            HP4142.Send($"PT{hold_time},{plus_width},{plus_period}");
                            HP4142.Send($"PWV{DSMU},{sweep_mode},{output_range},{base_current},{0},{d_x2},{No_step},{DSMU_COMPLIANCE}");
                            HP4142.Send($"DV{GSMU},0,{GSMU_BIAS},{GSMU_COMPLIANCE}");
                            HP4142.Send($"DV{SSMU},0,{SSMU_BIAS},{SSMU_COMPLIANCE}");
                            HP4142.Send($"MM4,{DSMU}");
                            HP4142.Send("XE");
                            Thread.Sleep(5);

                            for (int j = 0; j < No_step; j++)
                            {
                                DSMU_BIAS = 0 + j * d_step;
                                double Id = HP4142.ReadDouble();
                                if (j != No_step - 1)
                                    result.Add(new GraphPointViewModel() { X = DSMU_BIAS, Y = Id });
                            }
                        }
                        else
                        {
                            HP4142.Reset();
                            HP4142.Send($"CN{GSMU},{DSMU},{SSMU}");
                            HP4142.Send("FL0");
                            HP4142.Send($"PT{hold_time},{plus_width},{plus_period}");
                            HP4142.Send($"PWV{DSMU},{sweep_mode},{output_range},{base_current},{d_x1},{d_x2},{d_dotnum},{DSMU_COMPLIANCE}");
                            HP4142.Send($"DV{GSMU},0,{GSMU_BIAS},{GSMU_COMPLIANCE}");
                            HP4142.Send($"DV{SSMU},0,{SSMU_BIAS},{SSMU_COMPLIANCE}");
                            HP4142.Send($"MM4,{DSMU}");
                            HP4142.Send("XE");
                            Thread.Sleep(5);

                            for (int j = 0; j < d_dotnum; j++)
                            {
                                DSMU_BIAS = d_x1 + j * d_step;

                                double Id = HP4142.ReadDouble();
                                result.Add(new GraphPointViewModel() { X = DSMU_BIAS, Y = Id });
                            }
                        }
                        _ = HP4142.Send("FL1");
                    }

                    min = Math.Min(min, result.Min(c => c.Y));
                    max = Math.Max(max, result.Max(c => c.Y));
                    //step1.设置图表样式
                    //Graph.CurveNumber  表示曲线数量，应该再测试方法设置里设置
                    //获取曲线，从0开始；可以有多条曲线，最多能获取到CurveNumber条
                    var curve = Graph.TryGetCurve(i);
                    curve.AxisXTitleVisiable = false;   //不设置标题，应设置不显示
                    curve.AxisYTitleVisiable = false;   //不设置标题，应设置不显示
                    curve.LegendTitle = $"Vg={GSMU_BIAS}";
                    //curve.AxisYMinValue = min;
                    //curve.AxisYMaxValue = max;
                    //curve.AxisYStep = (max - min) / 10;
                    //curve.IsGraphAutoSize = false;
                    curve.LegendVisiable = true;
                    if (i == 0)
                        curve.AxisYVisiable = true;
                    else
                        curve.AxisYVisiable = false;

                    /*一次性显示数据，推荐*/
                    var points = result;
                    curve.AddNewPoints(points);
                }
            }
        }
    }
}


