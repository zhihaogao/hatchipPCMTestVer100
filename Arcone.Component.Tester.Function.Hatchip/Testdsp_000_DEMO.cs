
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
    /// <summary>
    /// 放置10-19测试方法
    /// </summary>
    public partial class TESTDISP
    {
        /// <summary>
        /// 测试方法主体完成两项工作
        /// 1、发送测试指令，获取数据，计算
        /// 2、设置图形显示样式，如果需要的话
        /// </summary>
        /// <param name="IsTestingMode"></param>
        [Characteristic(Name = "Ids-Vg MOS")]
        [CharacteristicVariable(Name = "", Unit = "", Min = 0, Max = 0, LLimit = 0, HLimit = 0, Ratio = 0, Sigma = 0)]
        [CharacteristicParameter(Code = "", Name = "", Desc = "", DefaultValue = "", ControlType = ParameterLayoutControlType.TextBox, DataType = ParameterLayoutDataType.Double)]
        public void characteristics0(bool IsTestingMode)
        {
            //step1.设置图表样式
            //Graph.CurveNumber  表示曲线数量，应该再测试方法设置里设置
            //获取曲线，从0开始；可以有多条曲线，最多能获取到CurveNumber条
            var curve0 = Graph.TryGetCurve();

            //curve0.LineSmoothness = 0;//0:连续曲线，默认；1：不连续曲线
            //curve0.StrokeThickness = 2;//线宽，默认2
            //curve0.StrokeDashArray = new DoubleCollection();//虚线，默认没有
            //curve0.Stroke = ArconeDefaultColorType.Black;//线条颜色，默认会同步设置XY轴标签颜色
            //curve0.PointGeometrySize = 0;   //标记点设置，暂未实现
            //curve0.PointGeometry = null;    //标记点设置，暂未实现
            //curve0.PointForeground = ArconeDefaultColorType.Black;  //标记点设置，暂未实现

            curve0.AxisXTitle = "demo x title";
            curve0.AxisXTitleVisiable = true;   //不设置标题，应设置不显示
            //curve0.AxisXForeground = ArconeDefaultColorType.Black;  //轴标签颜色
            //curve0.AxisXPosition = AxisPlacement.Bottom;    //默认位置应该再Bottom或者top，其他会出错
            ////坐标轴的最小值、步长、最大值。应该设置数据范围
            //curve0.AxisXMinValue = 0;
            //curve0.AxisXStep = 0.1;
            //curve0.AxisXMaxValue = 1;
            //curve0.AxisXVisiable = true;
            //curve0.SharedAxisXToGrapth = 1; //共享x轴设置，所有x轴默认共享。设置0表示独立x轴，设置N>0表示，与第N条共享x轴

            curve0.AxisYTitle = "demo y title";
            //curve0.AxisYTitleVisiable = true;   //不设置标题，应设置不显示
            //curve0.AxisYForeground = ArconeDefaultColorType.Black;  //轴标签颜色
            //curve0.AxisYPosition = AxisPlacement.Left;    //默认位置应该再left或者right，其他会出错
            ////坐标轴的最小值、步长、最大值。应该设置数据范围
            //curve0.AxisYMinValue = 0;
            //curve0.AxisYStep = 0.1;
            //curve0.AxisYMaxValue = 1;
            //curve0.AxisYVisiable = true;
            curve0.IsGraphAutoSize = true;//自动调整坐标轴，显示全部数据；false手动设置，超出范围的数据会被截取

            curve0.LegendTitle = "demo"; //图例名称
            curve0.LegendVisiable = true;   //不设置图例，应设置不显示

            //测试模式，数据赋值
            if (IsTestingMode)
            {
                int curveNum = Parameters.CurveNum;
                int sweepsmu = Parameters.SweepSMU;
                int holdtime = Parameters.StepHoldTime;
                double delaytime = Parameters.StepDelayTime;

                double xstep = Parameters.XStep; //步长>0
                int No_step = Parameters.DotNum + 1;
                double x1 = Parameters.X1;
                double x2 = Parameters.X2;

                double tlm0 = Parameters.TLM0;
                double tlm1 = Parameters.TLM1;
                double tlm2 = Parameters.TLM2;
                double tlm3 = Parameters.TLM3;
                double tlm4 = Parameters.TLM4;
                double tlm5 = Parameters.TLM5;
                double tlm6 = Parameters.TLM6;
                double tlm7 = Parameters.TLM7;
                double tlm8 = Parameters.TLM8;
                double tlm9 = Parameters.TLM9;

                double smuset0 = Parameters.SmuSetValue0;
                double smuset1 = Parameters.SmuSetValue1;
                double smuset2 = Parameters.SmuSetValue2;
                double smuset3 = Parameters.SmuSetValue3;

                double cmpl0 = Parameters.Compliance0;
                double cmpl1 = Parameters.Compliance1;
                double cmpl2 = Parameters.Compliance2;
                double cmpl3 = Parameters.Compliance3;

                double para2 = Parameters.Para2;
                double para3 = Parameters.Para3;
                double para4 = Parameters.Para4;


                //double atVgs4Is = Convert.ToDouble(Parameters.atVgs4Is);
                //double atVgs4Imax = Convert.ToDouble(Parameters.atVgs4Imax);

                /*逐点添加数据，适合图形显示挨个显示曲线的效果，每个数据点之间应有数据间隔
                 * 
                for (int i = 0; i < 100; i++)
                {
                    curve0.GraphAppendPoint=new GraphPointViewModel() { X = i * 0.1, Y = Math.Sin(i * 0.1) };
                    //Thread.Sleep(10);
                }*/

                /*一次性显示数据，推荐*/
                GraphPointViewModel[] list = new GraphPointViewModel[100];
                for (int i = 0; i < 100; i++)
                {
                    list[i] = new GraphPointViewModel { X = i * 0.1, Y = Math.Sin(i * 0.1) };
                }
                curve0.AddNewPoints(list);

                Vars[0] = GetRandomNumber(0, 10);
                Vars[1] = GetRandomNumber(0, 10);
                Vars[2] = GetRandomNumber(0, 10);
                Vars[3] = GetRandomNumber(0, 10);
                Vars[4] = GetRandomNumber(0, 10);//1E-12;
            }
            Thread.Sleep(100);
        }
    }
}


