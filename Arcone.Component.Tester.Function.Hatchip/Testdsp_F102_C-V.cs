﻿using System;
using Arcone.Component.Tester.Att;
using Arcone.Comm.Models.TesterCurve;
using Arcone.Comm.Resources;
using System.Threading;
using Arcone.Comm.Models.Tester;
using System.Linq;
using Arcone.Comm.Helper.System;
using System.Collections.Generic;

namespace Arcone.Component.Tester.Function.Hatchip
{
    /// <summary>
    /// 放置10-19测试方法
    /// </summary>
    public partial class TESTDISP
    {
        /// <summary>
        /// 加电流，测电压，算电阻
        /// </summary>
        /// <param name="IsTestingMode"></param>
        [Characteristic(Name = "C-V")]
        [CharacteristicVariable(Name = "C", Unit = "pF", Min = 0, Max = 0, LLimit = 0, HLimit = 0, Ratio = 0, Sigma = 0)]
        //[CharacteristicParameter(Code = "CurveNum")]
        [CharacteristicParameter(Code = "SweepSMU", IsShow = false)]
        [CharacteristicParameter(Code = "StepHoldTime", Name = "HoldTime(ms)", Desc = "Measure Hold Time", DefaultValue = "0",
            ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Int)]
        [CharacteristicParameter(Code = "StepDelayTime", IsShow = false)]
        [CharacteristicParameter(Code = "X1", Name = "X1(V)", Desc = "start value", DefaultValue = "-1",
            ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "X2", Name = "X2", Desc = "stop value", DefaultValue = "1",
            ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "XStep", Name = "XStep", Desc = "step", DefaultValue = "0.4",
            ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "DotNum", Name = "DotNum", Desc = "dot number", DefaultValue = "100",
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
        [CharacteristicParameter(Code = "SmuSetValue0", IsShow = false)]
        [CharacteristicParameter(Code = "SmuSetValue1", IsShow = false)]
        [CharacteristicParameter(Code = "SmuSetValue2", IsShow = false)]
        [CharacteristicParameter(Code = "SmuSetValue3", IsShow = false)]
        [CharacteristicParameter(Code = "Compliance0", IsShow = false)]
        [CharacteristicParameter(Code = "Compliance1", IsShow = false)]
        [CharacteristicParameter(Code = "Compliance2", IsShow = false)]
        [CharacteristicParameter(Code = "Compliance3", IsShow = false)]
        [CharacteristicParameter(Code = "Para2", Name = "OSCLev(V)", Desc = "VOLT LEV", DefaultValue = "0.1",
            ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "Para3", Name = "Freq(HZ)", Desc = "FREQ HZ", DefaultValue = "100E3",
            ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "Para4", Name = "AtVCap", Desc = "at V CAP", DefaultValue = "0.5",
            ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "FUNC", Name = "FUNC", Desc = "measure function", DefaultValue = "1#1,Cp-D;4,Cp-Rp;5,Cs-D;7,Cs-Rs;9,Lp-D;11,Lp-Rp;12,Ls-D;14,Ls-Rs",
            X = 220, Y = 135, Width = 100, ControlType = ParameterLayoutControlType.DropList, DataType = ParameterLayoutDataType.Int)]
        [CharacteristicParameter(Code = "ALC", Name = "ALC", Desc = "", DefaultValue = "0#0,OFF;1,ON",
            X = 220, Y = 200, Width = 100, ControlType = ParameterLayoutControlType.DropList, DataType = ParameterLayoutDataType.Int)]
        public void characteristics102(bool IsTestingMode)
        {
            //step1.设置图表样式
            //Graph.CurveNumber  表示曲线数量，应该再测试方法设置里设置
            //获取曲线，从0开始；可以有多条曲线，最多能获取到CurveNumber条
            var curve0 = Graph.TryGetCurve();
            curve0.AxisXTitleVisiable = false;   //不设置标题，应设置不显示
            curve0.AxisYTitleVisiable = false;   //不设置标题，应设置不显示
            curve0.LegendVisiable = false;   //不设置图例，应设置不显示
            //curve0.AxisXMinValue = 0;
            //curve0.AxisXStep = 0.01;
            //curve0.AxisXMaxValue = 0.06;
            //curve0.AxisYMinValue = 0;
            //curve0.AxisYStep = 0.05;
            //curve0.AxisYMaxValue = 0.5;

            //测试模式，数据赋值
            if (IsTestingMode)
            {
                double xstep = Parameters.XStep; //步长>0
                int No_step = Parameters.DotNum + 1;
                double x1 = Parameters.X1;
                double x2 = Parameters.X2;

                double OSCLev = StaticParameter.Para2;
                double Freq = StaticParameter.Para3;
                double atV_Cap = StaticParameter.Para4;
                double DelayTime = StaticParameter.StepDelayTime;
                int FUNC = Convert.ToInt32(Parameters.FUNC);
                int ALC = Convert.ToInt32(Parameters.ALC);

                var result = new List<GraphPointViewModel>();
                {
                    bool userHighPower = false;

                    if (x2 > 2) userHighPower = true;
                    HP4284.Reset();
                    HP4284.Send($"DISP:PAGE MSET");//FUNC:IMP CPD
                    HP4284.Send($"FUNC:IMP {TP.getHP4284MeasureFunction(FUNC)}");//FUNC:IMP CPD
                    HP4284.Send($"FORM ASCII");
                    HP4284.Send($"AMPL:ALC {ALC}");
                    if (OSCLev >= 1) HP4284.Send($"VOLT {OSCLev:0.0##E+00} V");
                    else
                    {
                        OSCLev = OSCLev * 1000;
                        HP4284.Send($"VOLT {OSCLev:0.0##E+00} mV");
                    }
                    if (userHighPower) HP4284.Send($"OUTP:HPOW ON");//>2V
                    HP4284.Send($"MEM:DIM DBUF, 1");//MEM:DIM DBUF,128
                    HP4284.Send($"TRIG:SOUR BUS");
                    HP4284.Send($"APER MED");//SHOR,MED,LONG
                                             //HP4284.Send($"TRIG:DEL {DelayTime,4:E2} S");
                    HP4284.Send($"MEM:FILL DBUF");
                    //HP4284.Send($"OUTP:DC:ISOL ON");
                    HP4284.Send($"BIAS:STATE ON");

                    if (Freq < 1000) HP4284.Send($"FREQ {Freq}HZ");
                    else
                    {
                        Freq = Freq / 1000;
                        if (Freq < 1000)
                            HP4284.Send($"FREQ {Freq}KHZ");
                        else
                            HP4284.Send($"FREQ 1MHZ");
                    }
                    //HP4284.Send($"DISP:PAGE MEAS");

                    //double e = 12.8 * 8.854E-12, A = 75E-6 * 75E-6, q = 1.6E-19;
                    for (int i = 0; i < No_step; i++)
                    {
                        var BIAS = x1 + i * xstep;
                        HP4284.Send($"DISP:PAGE MSET");
                        HP4284.Send($"MEM:CLE DBUF;FILL DBUF");
                        HP4284.Send($"BIAS:VOLT {BIAS:0.0##E+00} V");
                        HP4284.Send($"DISP:PAGE MEAS");
                        HP4284.Send($"TRIG");
                        HP4284.Send($"MEM:READ? DBUF");

                        var CvStr = HP4284.Receive(50);
                        string[] dataArr = CvStr.Split(',');

                        double dataA = 0f;
                        if (dataArr.Length > 0)
                        {
                            string DataA_Str = dataArr[0];
                            //DataB_Str = dataArr[1];
                            //Status_Str = dataArr[2];
                            //BinNo_Str = dataArr[3];

                            double.TryParse(DataA_Str, out dataA);
                            dataA *= 1E12;
                        }

                        result.Add(new GraphPointViewModel() { X = BIAS, Y = dataA });
                    }
                    HP4284.Send($"BIAS:STATE OFF");
                }
                curve0.AddNewPoints(result);

                var arr = result.Select(c => c.Y).ToArray();
                double val = ArconeMath.FindY(x1, x2, xstep, No_step, atV_Cap, ref arr);
                Vars[0] = val;
            }
        }
    }
}


