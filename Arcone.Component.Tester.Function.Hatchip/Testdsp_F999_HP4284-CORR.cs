using System;
using Arcone.Component.Tester.Att;
using Arcone.Comm.Models.TesterCurve;
using Arcone.Comm.Resources;
using System.Threading;
using Arcone.Comm.Models.Tester;
using System.Linq;
using Arcone.Comm.Helper.System;
using System.Collections.Generic;
using Arcone.Comm.MvvmCore.Views;

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
        [Characteristic(Name = "HP4284-CORR")]
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
        [CharacteristicParameter(Code = "SmuSetValue0", IsShow = false)]
        [CharacteristicParameter(Code = "SmuSetValue1", IsShow = false)]
        [CharacteristicParameter(Code = "SmuSetValue2", IsShow = false)]
        [CharacteristicParameter(Code = "SmuSetValue3", IsShow = false)]
        [CharacteristicParameter(Code = "Compliance0", IsShow = false)]
        [CharacteristicParameter(Code = "Compliance1", IsShow = false)]
        [CharacteristicParameter(Code = "Compliance2", IsShow = false)]
        [CharacteristicParameter(Code = "Compliance3", IsShow = false)]
        [CharacteristicParameter(Code = "Para2", Name = "POS(X)", Desc = "coordinate x", DefaultValue = "0",
            ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "Para3", Name = "POS(Y)", Desc = "coordinate y", DefaultValue = "0",
            ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "Para4", Name = "TimeOut(s)", Desc = "coordinate time out", DefaultValue = "120",
            ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        [CharacteristicParameter(Code = "CORR", Name = "CORR FUNC", Desc = "correction function", DefaultValue = "1#1,OPEN;2,SHOT",
            X = 220, Y = 135, Width = 100, ControlType = ParameterLayoutControlType.DropList, DataType = ParameterLayoutDataType.Int)]
        [CharacteristicParameter(Code = "CORRTIME", Name = "CORR WHEN", Desc = "correction when", DefaultValue = "1#1,Lot First;2,Each Wafer",
            X = 220, Y = 200, Width = 100, ControlType = ParameterLayoutControlType.DropList, DataType = ParameterLayoutDataType.Int)]
        public void characteristics900(bool IsTestingMode)
        {
            //测试模式，数据赋值
            if (IsTestingMode)
            {
                int pos_x = Convert.ToInt32(StaticParameter.Para2);
                int pos_y = Convert.ToInt32(StaticParameter.Para3);
                int timeout_second = Convert.ToInt32(StaticParameter.Para4);
                int corr_function = Convert.ToInt32(Parameters.CORR);
                int corr_when = Convert.ToInt32(Parameters.CORRTIME);
                bool do_correction = false;
                int timeout_mili_second = timeout_second * 1000;

                if (corr_when == 1)
                    do_correction = IsWaferFirstChip && IsWaferFirstDie && IsWaferFirstChip;
                else if (corr_when == 2)
                    do_correction = IsWaferFirstDie && IsWaferFirstChip;

                Thread.Sleep(100);
                if (do_correction)
                {
                    if (corr_function == 1)//open correction
                    {
                        //Prober.Zdown();//确认wafer上有没有open 图形
                        //Dialog.WaitingDelay("HP4284 Correction Open\n \n", timeout_second);
                        int statu = HP4284.CorrectionOpen(timeout_mili_second);
                        if (statu != 0)
                            Dialog.Show("HP4284 OPEN CORR\n    \nNOT FINISHED CORRECT\n    \nPLS CHECK OR MANUAL CORR");
                        //Dialog.Hidden();
                    }
                    else if (corr_function == 2)//short correction{
                    {
                        //Dialog.WaitingDelay("HP4284 Correction Open\n \n", timeout_second);
                        int statu = HP4284.CorrectionShort(timeout_mili_second);
                        if (statu != 0)
                            Dialog.Show("HP4284 OPEN CORR\n    \nNOT FINISHED CORRECT\n    \nPLS CHECK OR MANUAL CORR");
                    }
                }
            }
        }


        /*
100 OUTPUT 717;"*ESE 60" ! Event Status Resister enable
110 ! (error bits enable)
120 OUTPUT 717;"*SRE 32" ! Status Byte Resister enable
130 ! (Event Status Summary bit enable)
140 ON INTR 7,2 CALL Errors
150 ENABLE INTR 7;2
...
500 SUB Errors
510 DIM Err$[50]
520 Sp=SPOLL(717)
530 IF BIT(Sp,5) THEN
540 OUTPUT 717;"*ESR?" ! Clear the Event Status Resister
550 ENTER 717;Esr
560 PRINT "Event Status Resister =";Esr
570 LOOP
580 OUTPUT 717;"SYST:ERR?"! Error No. & message query
590 ENTER 717;Err$
600 EXIT IF VAL(Err$)=0 ! Exit if no error
610 PRINT Err$
620 END LOOP
630 END IF
640 ENABLE
         */
    }
}


