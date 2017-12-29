using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace TrialApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //判断日期是否合法
            string curYear = DateTime.Now.Year.ToString();
            string curMonth = DateTime.Now.Month.ToString();
            string curDay = DateTime.Now.Day.ToString();

            if (!QueryTrialPeriod(curYear, curMonth, curDay))
            {
                return;
            }

            Console.WriteLine("Hello World!");

            //获取用户下一步输入
            Console.WriteLine("Plz choose the next behavior:<1>exit<2>set trial period");
            string strChoice = Console.ReadLine();
            if (strChoice == null)
            {
                return;
            }
            else if (strChoice == "1")
            {
                return;
            }
            else if (strChoice == "2")
            {
                SetTrialPeriod(curYear, curMonth, curDay);
            }
            return;
        }


        //查询试用期限
        static bool QueryTrialPeriod(string curYear, string curMonth, string curDay)
        {
            //读取注册表项
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("Software\\CaiMengNan\\TrialPeriod");
            if (rk == null)
            {
                return true;
            }
            string[] strValues = rk.GetValueNames();
            int year = Convert.ToInt32(curYear),
                month = Convert.ToInt32(curMonth),
                day = Convert.ToInt32(curDay) - 1;
            foreach (string s in strValues)
            {
                RegistryValueKind rvk = rk.GetValueKind(s);
                switch (rvk)
                {
                    case RegistryValueKind.DWord:
                        {
                            if (s == "trialYear")
                            {
                                year = Decode((int)rk.GetValue(s));
                            }
                            else if (s == "trialMonth")
                            {
                                month = Decode((int)rk.GetValue(s));
                            }
                            else if (s == "trialDay")
                            {
                                day = Decode((int)rk.GetValue(s));
                            }
                            break;
                        }
                    default:
                        break;
                }
            }
            if (year < Convert.ToInt32(curYear))
            {
                return false;
            }
            else if (year == Convert.ToInt32(curYear))
            {
                if (month < Convert.ToInt32(curMonth))
                {
                    return false;
                }
                else if (month == Convert.ToInt32(curMonth))
                {
                    if (day < Convert.ToInt32(curDay))
                    {
                        return false;
                    }
                    return true;
                }
                return true;
            }
            return true;
        }


        //设置试用期限
        static void SetTrialPeriod(string curYear, string curMonth, string curDay)
        {
            //获取试用期的具体日期
            string strLastYear, strLastMonth, strLastDay;
            int year = 1970, month = 1, day = 1;
            bool flag = false;
            do
            {
                flag = false;
                Console.WriteLine("Plz input the year of last day");
                strLastYear = Console.ReadLine();
                if (strLastYear == null)//ctrl c
                {
                    return;
                }
                else if (strLastYear.Length <= 0)
                {
                    Console.WriteLine("Illegal input");
                    flag = true;
                }
                //正确性核查
                year = Convert.ToInt32(strLastYear);
                if (year < Convert.ToInt32(curYear))
                {
                    Console.WriteLine("Illegal input");
                    flag = true;
                }
            } while (flag);
            do
            {
                flag = false;
                Console.WriteLine("Plz input the month of last day");
                strLastMonth = Console.ReadLine();
                if (strLastMonth == null)//ctrl c
                {
                    return;
                }
                else if (strLastMonth.Length <= 0)
                {
                    Console.WriteLine("Illegal input");
                    flag = true;
                }
                //正确性核查
                month = Convert.ToInt32(strLastMonth);
                if (year == Convert.ToInt32(curYear))
                {
                    if (month < Convert.ToInt32(curMonth))
                    {
                        Console.WriteLine("Illegal input");
                        flag = true;
                    }
                }
                else if (month < 0 || month > 12)
                {
                    Console.WriteLine("Illegal input");
                    flag = true;
                }
            } while (flag);
            do
            {
                flag = false;
                Console.WriteLine("Plz input the day of last day");
                strLastDay = Console.ReadLine();
                if (strLastDay == null)//ctrl c
                {
                    return;
                }
                else if (strLastDay.Length <= 0)
                {
                    Console.WriteLine("Illegal input");
                    flag = true;
                }
                //正确性核查
                day = Convert.ToInt32(strLastDay);
                int limitDay = 31;
                if (month == 4
                    || month == 6
                    || month == 9
                    || month == 11)
                {
                    limitDay = 30;
                }
                else if (month == 2)
                {
                    if (year % 4 == 0)
                        limitDay = 29;
                    else
                        limitDay = 28;
                }
                if (year == Convert.ToInt32(curYear)
                    && month == Convert.ToInt32(curMonth))
                {
                    if (day < Convert.ToInt32(curDay))
                    {
                        Console.WriteLine("Illegal input");
                        flag = true;
                    }
                }
                else if (day < 0 || day > limitDay)
                {
                    Console.WriteLine("Illegal input");
                    flag = true;
                }
            } while (flag);
            //写入注册表项
            if (Registry.CurrentUser.OpenSubKey("Software\\CaiMengNan\\TrialPeriod") != null)
            {
                Registry.CurrentUser.DeleteSubKey("Software\\CaiMengNan\\TrialPeriod");
            }
            RegistryKey rk = Registry.CurrentUser.CreateSubKey("Software\\CaiMengNan\\TrialPeriod");
            rk.SetValue("trialYear", Encode(year));
            rk.SetValue("trialMonth", Encode(month));
            rk.SetValue("trialDay", Encode(day));
        }


        static int Encode(int str)
        {
            return str + 1;
        }


        static int Decode(int str)
        {
            return str - 1;
        }
    }
}
