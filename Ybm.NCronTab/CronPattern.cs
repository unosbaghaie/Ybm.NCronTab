using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ngIPE.NCronTab
{
    internal class CronPattern
    {
        public CronPattern()
        {
            Minutes = new List<int>();
            Hours = new List<int>();
            Days = new List<int>();
            Months = new List<int>();
            Years = new List<int>();
            WeekDays = new List<int>();

        }
        public List<int> Minutes { get; set; }

        public List<int> Hours { get; set; }

        public List<int> Days { get; set; }
        public List<int> Months { get; set; }
        public List<int> Years { get; set; }

        public List<int> WeekDays { get; set; }


        public CronPattern Parse(string cron, JalaliCalendar startDate, JalaliCalendar endDate)
        {
            //CronTabScheduler cronTabScheduler = new NCronTab.CronTabScheduler();
            var persianCalendarPart = new JalaliCalendar();

            var parts = cron.Split(' ');


            #region [Days]
            int dayStep = 0;
            int currentDayOfYear = 0;
            if (parts[2] == "*")
                for (int i = startDate.DaysInYears; i <= endDate.DaysInYears; i++)
                {
                    Days.Add(i);
                }
            else
            {
                var dayParts = parts[2].Split('/');
                if (dayParts.Count() == 1)
                {
                    Days.Add(int.Parse(dayParts[0]));
                }
                else if (dayParts.Count() == 2)
                {
                    var days = dayParts[1].Split(',');
                    dayStep = int.Parse(days[0]);
                    currentDayOfYear = startDate.DayOfYear;
                    for (int i = startDate.DaysInYears; i <= endDate.DaysInYears; i = i + dayStep)
                    {
                        Days.Add(i);
                    }
                }
            }

            //else
            //{
            //    if (parts[2].Contains("-"))
            //    {
            //        Days.Clear();
            //        var dayParts = parts[2].Split('-');
            //        var firstOfWeek = int.Parse(dayParts[0]);
            //        var endOfWeek = int.Parse(dayParts[1]);
            //        currentDayOfYear = startDate.DayOfYear;

            //        for (int i = startDate.DaysInYears; i <= endDate.DaysInYears; i++)
            //        {
            //            var dayOfMonth = persianCalendarPart.DayOfYearToDayOfMonth(i);
            //            var dayNum = persianCalendarPart.DayOfYearToDayNum(i);

            //            if (WeekDays.Contains(dayNum) && dayOfMonth >= firstOfWeek && dayOfMonth < endOfWeek)
            //                Days.Add(i);
            //        }
            //        Months = null;
            //        return this;
            //    }
            //    else
            //    {
            //        var dayParts = parts[2].Split('/');
            //        if (dayParts.Count() == 1)
            //        {
            //            Days.Add(int.Parse(dayParts[0]));
            //        }
            //        else if (dayParts.Count() == 2)
            //        {
            //            var days = dayParts[1].Split(',');
            //            dayStep = int.Parse(days[0]);
            //            currentDayOfYear = startDate.DayOfYear;
            //            for (int i = startDate.DayOfYear; i <= endDate.DayOfYear; i = i + dayStep)
            //            {
            //                Days.Add(i);
            //            }
            //        }
            //    }
            //}
            #endregion





            //for (int i = startDate.Year; i <= endDate.Year; i++)
            //{
            //    Years.Add(i);
            //}




            #region [WeekDays]
            if (parts[4] != "*")
            {
                


                Days = new List<int>();
                var yearParts = parts[4].Split(',');
                List<int> daysForWeek = new List<int>();
                for (int i = startDate.DaysInYears; i <= endDate.DaysInYears; i++)
                {
                    daysForWeek.Add(i);
                }
                foreach (var item in yearParts)
                {
                    var dayNum = persianCalendarPart.WeekMapping.FirstOrDefault(q => q.Key == item);
                    WeekDays.Add(dayNum.Value);
                }
                foreach (var dayForWeek in daysForWeek)
                {
                    var dayNum = persianCalendarPart.DaysInYearsToDayNum(dayForWeek);
                    if (WeekDays.Contains(dayNum))
                    {
                        Days.Add(dayForWeek);
                    }
                }
            }
            #endregion


            #region [Minutes]
            if (parts[0] == "*")
            {
                for (int i = 0; i < 60; i++)
                {
                    Minutes.Add(i);
                }
            }
            else
            {
                var minuteParts = parts[0].Split('/');
                if (minuteParts.Count() == 1)
                {
                    Minutes.Add(int.Parse(minuteParts[0]));
                }
                else if (minuteParts.Count() == 2)
                {
                    int minPart = 0;
                    if (minuteParts[0] == "*" && int.TryParse(minuteParts[1].ToString(), out minPart))
                    {
                        for (int i = startDate.MinuteOfYear; i < endDate.MinuteOfYear; i = i + minPart)
                        {
                            Minutes.Add(i);
                        }

                    }
                    return this;
                }
            }

            #endregion




            #region [Hours]
            if (parts[1] == "*")
            {
                for (int i = 0; i < 24; i++)
                {
                    Hours.Add(i);
                }
            }
            else
            {
                var hourParts = parts[1].Split('/');
                if (hourParts.Count() == 1)
                {
                    Hours.Add(int.Parse(hourParts[0]));
                }
                else if (hourParts.Count() == 2)
                {
                    int minPart = 0;
                    if (hourParts[0] == "*" && int.TryParse(hourParts[1].ToString(), out minPart))
                    {
                        for (int i = startDate.Hour; i < endDate.Hour; i = i + minPart)
                        {
                            Hours.Add(i);
                        }

                    }
                }
            }
            #endregion

            #region [Months]
            bool monthly = false;
            List<int> monthStep =new List<int>();
            int currentMonth = 0;
            if (parts[3] == "*")
                Months.Add(1);
            else
            {
                

                var monthParts = parts[3].Split('/');
                if (monthParts.Count() == 1)
                {
                    monthly = false;
                    monthStep.Add(int.Parse(monthParts[0]));
                }
                else if (monthParts.Count() == 2)
                {
                    monthly = true;
                    foreach (var item in monthParts[1].Split(','))
                    {
		                monthStep.Add(int.Parse(item));
	                }
                    
                }
                
                var daysInMonthPart = new List<int>();
                var days = new List<int>();
                days.AddRange(Days);
                Days.Clear();
                for (int i = startDate.DaysInYears; i <= endDate.DaysInYears; i++)
                {
                    int yearr;
                    int month;
                    int day;
                    persianCalendarPart.DaysInYearsToDateParts(i, out yearr, out month, out day);
                    if (monthly && (monthStep.Any(q => q == month)) && days.Contains(day))
                        Days.Add(i);
                    else
                        if ((month == monthStep[0]) && days.Contains(day))
                        Days.Add(i);
                }
            }
            #endregion
            return this;
        }
        public override string ToString()
        {
            return string.Format("{0}/{1}/{2} {3}:{4}", Years, Months, Days, Hours, Minutes);
        }
    }
}
