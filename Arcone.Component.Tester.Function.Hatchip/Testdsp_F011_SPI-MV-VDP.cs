
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
        [Characteristic(Name = "SPI-MV-VDP")]
        [CharacteristicVariable(Name = "V", Unit = "V", Min = 0, Max = 0, LLimit = 0, HLimit = 0, Ratio = 0, Sigma = 0)]
        [CharacteristicVariable(Name = "I", Unit = "A", Min = 0, Max = 0, LLimit = 0, HLimit = 0, Ratio = 0, Sigma = 0)]
        [CharacteristicVariable(Name = "R", Unit = "ohm", Min = 0, Max = 0, LLimit = 0, HLimit = 0, Ratio = 0, Sigma = 0)]
        //[CharacteristicParameter(Code = "CurveNum")]
        [CharacteristicParameter(Code = "SweepSMU", Name = "Plus Period(ms)", Desc = "Period Time [Max(10, 2*(pulse width)),500],", DefaultValue = "10",
            ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Int)]
        [CharacteristicParameter(Code = "StepHoldTime", Name = "HoldTime(ms)", Desc = "Hold Time[0 to 655350]", DefaultValue = "0",
            ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Int)]
        [CharacteristicParameter(Code = "StepDelayTime", Name = "Plus Width(ms)", Desc = "Plus Width[1 to 50]", DefaultValue = "1",
            ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "X1", Name = "Sweep X1(I)", Desc = "The start pulse current, stop pulse current and base current must be the same polarity.", DefaultValue = "-0.01",
            ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "X2", Name = "Sweep X2", Desc = "stop pulse current", DefaultValue = "0.01",
            ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "XStep", Name = "Sweep Step", Desc = "step", DefaultValue = "0.0002",
            ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "DotNum", Name = "Sweep Dot", Desc = "dot number", DefaultValue = "100",
            ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Int)]
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
        [CharacteristicParameter(Code = "SmuSetValue0", Name = "Base Current", Desc = "The start pulse current, stop pulse current and base current must be the same polarity.", DefaultValue = "0",
            ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "SmuSetValue1", IsShow = false)]
        [CharacteristicParameter(Code = "SmuSetValue2", IsShow = false)]
        [CharacteristicParameter(Code = "SmuSetValue3", IsShow = false)]
        [CharacteristicParameter(Code = "Compliance0", Name = "Force COMP(V)", Desc = "Force SMU compliance", DefaultValue = "2",
            ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "Compliance1", Name = "Compliance2", Desc = "measure1 compliance", DefaultValue = "20",
            ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "Compliance2", Name = "Compliance3", Desc = "measure2 compliance", DefaultValue = "20",
            ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "Compliance3", IsShow = false)]
        [CharacteristicParameter(Code = "Para2", Name = "AtIVR", Desc = "get the I VR", DefaultValue = "0",
           ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "Para3", IsShow = false)]
        [CharacteristicParameter(Code = "Para4", IsShow = false)]
        [CharacteristicParameter(Code = "ForceSMU", Name = "Sweep SMU", Desc = "select sweep smu", DefaultValue = "1#1,SMU1;3,SMU2;4,SMU3;5,SMU4",
            X = 220, Y = 135, Width = 100, ControlType = ParameterLayoutControlType.DropList, DataType = ParameterLayoutDataType.Int)]
        [CharacteristicParameter(Code = "GNDSMU", Name = "GND SMU", Desc = "select GND smu", DefaultValue = "5#0,-NULL-;1,SMU1;3,SMU2;4,SMU3;5,SMU4",
            X = 220, Y = 200, Width = 100, ControlType = ParameterLayoutControlType.DropList, DataType = ParameterLayoutDataType.Int)]
        [CharacteristicParameter(Code = "M1SMU", Name = "Measure1 SMU", Desc = "select measure smu", DefaultValue = "3#1,SMU1;3,SMU2;4,SMU3;5,SMU4",
            X = 220, Y = 265, Width = 100, ControlType = ParameterLayoutControlType.DropList, DataType = ParameterLayoutDataType.Int)]
        [CharacteristicParameter(Code = "M2SMU", Name = "Measure2 SMU", Desc = "select measure smu", DefaultValue = "4#1,SMU1;3,SMU2;4,SMU3;5,SMU4",
            X = 220, Y = 330, Width = 100, ControlType = ParameterLayoutControlType.DropList, DataType = ParameterLayoutDataType.Int)]
        public void characteristics11(bool IsTestingMode)
        {
            //测试模式，数据赋值
            if (IsTestingMode)
            {
                int forceSmu = Convert.ToInt32(Parameters.ForceSMU);
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

                double xstep = Parameters.XStep; //步长>0
                int No_step = Parameters.DotNum + 1;
                double x1 = Parameters.X1;
                double x2 = Parameters.X2;

                double hold_time = StaticParameter.StepHoldTime * 1E-3;
                double plus_width = StaticParameter.StepDelayTime * 1E-3;
                double plus_period = StaticParameter.SweepSMU * 1E-3;

                double base_current = Parameters.SmuSetValue0;
                double atI = Parameters.Para2;

                var matrixPages = Matrix.GetPages().OrderBy(c => c).ToList();
                var matrixFirstPage = 0;

                int index = 0;
                foreach (var page in matrixPages)
                {
                    //step1.设置图表样式
                    //Graph.CurveNumber  表示曲线数量，应该再测试方法设置里设置
                    //获取曲线，从0开始；可以有多条曲线，最多能获取到CurveNumber条
                    var curve0 = Graph.TryGetCurve(index);
                    curve0.AxisXTitleVisiable = false;   //不设置标题，应设置不显示
                    curve0.AxisYTitleVisiable = false;   //不设置标题，应设置不显示
                    curve0.LegendVisiable = false;   //不设置图例，应设置不显示

                    if (page != matrixFirstPage)
                    {
                        Matrix.Reset();
                        Thread.Sleep(1);
                        Matrix.Connect(page);
                        Thread.Sleep(1);
                    }
                    int sweep_mode = 1;
                    int output_range = 0;
                    double forceSmu_BIAS = 0;
                    var result = new List<GraphPointViewModel>();

                    if (x1 < 0 && x2 > 0)
                    {
                        No_step = (int)((0 - x1) / xstep + 1.05);
                        HP4142.Reset();
                        _ = gndSmu > 0 ? HP4142.Send($"CN{forceSmu},{M1SMU},{M2SMU},{gndSmu}") : HP4142.Send($"CN{forceSmu},{M1SMU},{M2SMU}");
                        _ = HP4142.Send("FL0");
                        _ = HP4142.Send($"PT{hold_time},{plus_width},{plus_period}");
                        //PWI ch#, sweep mode, output range, base current, start pulse current,stop pulse current, number of steps[, V compliance]
                        _ = HP4142.Send($"PWI{forceSmu},{sweep_mode},{output_range},{base_current},{x1},{0},{No_step},{forceSmu_COMPLIANCE}");
                        _ = HP4142.Send($"DI{M1SMU},0,{M1SMU_BIAS},{M1SMU_COMPLIANCE}");
                        _ = HP4142.Send($"DI{M2SMU},0,{M2SMU_BIAS},{M2SMU_COMPLIANCE}");
                        _ = gndSmu > 0 ? HP4142.Send($"DV{gndSmu},0,{gndSmu_BIAS},{gndSmu_COMPLIANCE}") : 0;
                        //HP4142.Send($"MM4,{forceSmu},{M1SMU},{M2SMU}");
                        HP4142.Send($"MM4,{forceSmu}");
                        HP4142.Send("XE");
                        Thread.Sleep(5);

                        for (int i = 0; i < No_step; i++)
                        {
                            forceSmu_BIAS = x1 + i * xstep;
                            double forceSmuV = HP4142.ReadDouble();
                            //double M1SMU_V = HP4142.ReadDouble();
                            //double M2SMU_V = HP4142.ReadDouble();
                            double delta = forceSmuV;//M1SMU_V - M2SMU_V;
                            if (i != No_step - 1)
                                result.Add(new GraphPointViewModel() { X = forceSmu_BIAS, Y = Math.Abs(delta) });
                        }

                        No_step = (int)((x2-0) / xstep + 1.05);
                        HP4142.Reset();
                        _ = gndSmu > 0 ? HP4142.Send($"CN{forceSmu},{M1SMU},{M2SMU},{gndSmu}") : HP4142.Send($"CN{forceSmu},{M1SMU},{M2SMU}");
                        _ = HP4142.Send("FL0");
                        _ = HP4142.Send($"PT{hold_time},{plus_width},{plus_period}");
                        //PWI ch#, sweep mode, output range, base current, start pulse current,stop pulse current, number of steps[, V compliance]
                        _ = HP4142.Send($"PWI{forceSmu},{sweep_mode},{output_range},{base_current},{0},{x2},{No_step},{forceSmu_COMPLIANCE}");
                        _ = HP4142.Send($"DI{M1SMU},0,{M1SMU_BIAS},{M1SMU_COMPLIANCE}");
                        _ = HP4142.Send($"DI{M2SMU},0,{M2SMU_BIAS},{M2SMU_COMPLIANCE}");
                        _ = gndSmu > 0 ? HP4142.Send($"DV{gndSmu},0,{gndSmu_BIAS},{gndSmu_COMPLIANCE}") : 0;
                        //HP4142.Send($"MM4,{forceSmu},{M1SMU},{M2SMU}");
                        HP4142.Send($"MM4,{forceSmu}");
                        HP4142.Send("XE");
                        Thread.Sleep(5);

                        for (int i = 0; i < No_step; i++)
                        {
                            forceSmu_BIAS = 0 + i * xstep;
                            double forceSmuV = HP4142.ReadDouble();
                            //double M1SMU_V = HP4142.ReadDouble();
                            //double M2SMU_V = HP4142.ReadDouble();
                            double delta = forceSmuV;//M1SMU_V - M2SMU_V;
                            result.Add(new GraphPointViewModel() { X = forceSmu_BIAS, Y = Math.Abs(delta) });
                        }
                    }
                    else
                    {
                        HP4142.Reset();
                        _ = gndSmu > 0 ? HP4142.Send($"CN{forceSmu},{M1SMU},{M2SMU},{gndSmu}") : HP4142.Send($"CN{forceSmu},{M1SMU},{M2SMU}");
                        _ = HP4142.Send("FL0");
                        _ = HP4142.Send($"PT{hold_time},{plus_width},{plus_period}");
                        //PWI ch#, sweep mode, output range, base current, start pulse current,stop pulse current, number of steps[, V compliance]
                        _ = HP4142.Send($"PWI{forceSmu},{sweep_mode},{output_range},{base_current},{x1},{x2},{No_step},{forceSmu_COMPLIANCE}");
                        _ = HP4142.Send($"DI{M1SMU},0,{M1SMU_BIAS},{M1SMU_COMPLIANCE}");
                        _ = HP4142.Send($"DI{M2SMU},0,{M2SMU_BIAS},{M2SMU_COMPLIANCE}");
                        _ = gndSmu > 0 ? HP4142.Send($"DV{gndSmu},0,{gndSmu_BIAS},{gndSmu_COMPLIANCE}") : 0;
                        //HP4142.Send($"MM4,{forceSmu},{M1SMU},{M2SMU}");
                        HP4142.Send($"MM4,{forceSmu}");
                        HP4142.Send("XE");
                        Thread.Sleep(5);

                        for (int i = 0; i < No_step; i++)
                        {
                            forceSmu_BIAS = x1 + i * xstep;

                            double forceSmuV = HP4142.ReadDouble();
                            //double M1SMU_V = HP4142.ReadDouble();
                            //double M2SMU_V = HP4142.ReadDouble();
                            double delta = forceSmuV;//M1SMU_V - M2SMU_V;
                            result.Add(new GraphPointViewModel() { X = forceSmu_BIAS, Y = Math.Abs(delta) });
                        }
                    }
                    _ = HP4142.Send("FL1");

                    curve0.AddNewPoints(result);
                    
                    if (!DoubleUtil.IsZero(atI) && result.Any())
                    {
                        var Y1 = result.Select(c => c.Y).ToArray();
                        double v = ArconeMath.FindY(x1, x2, xstep, No_step, atI, ref Y1, 0);
                        double R = v / (atI + 1E-15);
                        Vars[index] = R;
                    }
                    index++;
                }
            }
        }
    }
}


