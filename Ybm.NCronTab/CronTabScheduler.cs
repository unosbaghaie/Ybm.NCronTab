using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ngIPE.NCronTab
{
    public class CronTabScheduler
    {
        private CronPattern Parts { get; set; }

        public List<string> NextOccurances(string cron, DateTime startDate, DateTime endDate)
        {
            List<string> FinalOccurances = new List<string>();
            CronPattern cronPattern = new CronPattern();

            var now = new JalaliCalendar().GetPersianDateTime(startDate);
            var later = new JalaliCalendar().GetPersianDateTime(endDate);

            CronPattern parts = cronPattern.Parse(cron, now, later);

            this.Parts = parts;
            var occurances = NextOccurances();

            foreach (var occurance in occurances)
            {
                var occuranceInGregorian = JalaliToGregorian(occurance);
                if (DateTime.Now <= occuranceInGregorian && endDate > occuranceInGregorian)
                    FinalOccurances.Add(occurance);
            }

            if (FinalOccurances.Count == 0)
                throw new Exception("الگوی انتخابی بین بازه تاریخی مشخص شده نیست .لطفا بازه انتخابی بین یک سال شمسی باشد");

            return FinalOccurances;
        }
        /// <summary>
        /// return next occurances as Jalali DateTime List
        /// </summary>
        /// <param name="parts"></param>
        /// <returns>List<string></returns>
        private List<string> NextOccurances()
        {
            var parts = this.Parts;
            var persianCalendarPart = new JalaliCalendar();

            List<string> dates2 = new List<string>();

            foreach (var dayPart in parts.Days)
            {
                foreach (var hourPart in parts.Hours)
                {
                    foreach (var minutePart in parts.Minutes)
                    {
                        int month;
                        int day;
                        int yearr;
                        persianCalendarPart.DaysInYearsToDateParts(dayPart, out yearr, out month, out day);

                        dates2.Add(string.Format("{0}/{1}/{2} {3}:{4}", yearr, month, day, hourPart.ToString("00"), minutePart.ToString("00")));
                    }

                }

            }
            return dates2;
        }

        /// <summary>
        /// return next occurance as Jalali DateTime
        /// </summary>
        /// <param name="parts"></param>
        /// <param name="jalaliDateTimeNow"></param>
        /// <returns></returns>
        public string NextOccurance(string cron, DateTime startDate, DateTime endDate, string jalaliDateTimeNow)
        {
            try
            {
                DateTime dateTimeToReturn = default(DateTime);

                var list = NextOccurances(cron, startDate, endDate);

                var now = JalaliToGregorian(jalaliDateTimeNow);
                foreach (var lst in list)
                {
                    var dateTimeLst = JalaliToGregorian(lst);
                    if (now < dateTimeLst)
                    {
                        dateTimeToReturn = dateTimeLst;
                        break;
                    }
                }
                return GregorianToJalali(dateTimeToReturn);
            }
            catch (Exception ex)
            {
                return null;   
            }
        }
        /// <summary>
        /// return next occurance as Jalali DateTime
        /// </summary>
        /// <param name="parts"></param>
        /// <param name="now"></param>
        /// <returns></returns>
        public string NextOccurance(string cron, DateTime startDate, DateTime endDate, DateTime now)
        {
            try
            {
                DateTime? dateTimeToReturn = null;
                var list = NextOccurances(cron, startDate, endDate);

                foreach (var lst in list)
                {
                    var dateTimeLst = JalaliToGregorian(lst);
                    if (now < dateTimeLst)
                    {
                        dateTimeToReturn = dateTimeLst;
                        break;
                    }
                }
                if (dateTimeToReturn != null)
                    return GregorianToJalali((DateTime)dateTimeToReturn);
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// return next occurance as Gregorian DateTime
        /// </summary>
        /// <param name="parts"></param>
        /// <param name="now"></param>
        /// <returns></returns>
        public DateTime? NextGregorianOccurance(string cron, DateTime startDate, DateTime endDate, DateTime now)
        {
            try
            {
                DateTime? dateTimeToReturn = null;
                var list = NextOccurances(cron, startDate, endDate);

                foreach (var lst in list)
                {
                    var dateTimeLst = JalaliToGregorian(lst);
                    if (now < dateTimeLst)
                    {
                        dateTimeToReturn = dateTimeLst;
                        break;
                    }
                }

                if (dateTimeToReturn == default(DateTime))
                    throw new Exception("an error occured in NextGregorianOccurance and return value equals default(DateTime)");


                if (dateTimeToReturn != null)
                    return dateTimeToReturn;
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;   
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTimeAsPersian"></param>
        /// <returns> GregorianDateTime </returns>
        public DateTime JalaliToGregorian(string dateTimeAsPersian)
        {
            dateTimeAsPersian = dateTimeAsPersian.Replace("-", "/");
            List<string> datetimeParts = dateTimeAsPersian.Split(new List<char>() { ' ' }.ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList<string>();

            if (datetimeParts.Count == 0) return default(DateTime);
            if (datetimeParts.Count == 1) datetimeParts.Add("00:00");

            List<string> dateSegments = datetimeParts[0].Split('/').ToList<string>();
            List<string> timeSegments = datetimeParts[1].Split(':').ToList<string>();
            if (dateSegments.Count < 3) return default(DateTime);
            int year = int.Parse(dateSegments[0]);
            int month = int.Parse(dateSegments[1]);
            int day = int.Parse(dateSegments[2]);
            int hour = int.Parse(timeSegments[0]);
            int minute = int.Parse(timeSegments[1]);
            PersianCalendar pcal = new PersianCalendar();
            return pcal.ToDateTime(year, month, day, hour, minute, 0, 0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTimeAsGregorian"></param>
        /// <param name="format"></param>
        /// <returns> JalaliDateTime </returns>
        public string GregorianToJalali(DateTime dateTimeAsGregorian, string format = "yyyy/mm/dd")
        {
            if (dateTimeAsGregorian.Year < 1000) dateTimeAsGregorian = DateTime.Now;
            PersianCalendar pc = new PersianCalendar();
            string jalaliDateTime = format.ToLower();
            jalaliDateTime = jalaliDateTime.Replace("yyyy", pc.GetYear(dateTimeAsGregorian).ToString());
            jalaliDateTime = jalaliDateTime.Replace("mm", pc.GetMonth(dateTimeAsGregorian).ToString());
            jalaliDateTime = jalaliDateTime.Replace("dd", pc.GetDayOfMonth(dateTimeAsGregorian).ToString());
            jalaliDateTime = jalaliDateTime + " " + dateTimeAsGregorian.ToLongTimeString();
            return jalaliDateTime;
        }

    }

}
