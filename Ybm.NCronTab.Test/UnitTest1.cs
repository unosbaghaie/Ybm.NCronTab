/*
┌───────────── minute (0 - 59)
│ ┌───────────── hour (0 - 23)
│ │ ┌───────────── day of month (1 - 31)
│ │ │ ┌───────────── month (1 - 12)
│ │ │ │ ┌───────────── day of week (0 - 6) (Sunday to Saturday;
│ │ │ │ │                                       7 is also Sunday)
│ │ │ │ │
│ │ │ │ │
* * * * *  command to execute
* */

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.Text;
using System.Collections.Generic;
using ngIPE.NCronTab;

namespace Ybm.NCronTab.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var startDate= new DateTime(2016,6,1,10,50,25);
            var endDate= new DateTime(2019, 12, 1, 10, 50, 25);

            string fifthOfFarvardin = "0 0 15 1 *";
            var f4Result = new CronTabScheduler().NextOccurances(fifthOfFarvardin  , startDate,endDate);




            //string patternStr11 = "0 0 8-14 */1 TUE";
            //var fResult11 = new CronTabScheduler().NextOccurances(patternStr11 , startDate,endDate);

            //var nowForNextOccurance = new DateTime(2016, 8, 1, 10, 50, 25);
            //var fResult111 = new CronTabScheduler().NextOccurance(patternStr11, startDate, endDate, DateTime.Now);

            //var fResult112 = new CronTabScheduler().NextOccurance(patternStr11,startDate,endDate, new DateTime(2016, 6, 1, 10, 50, 25));


            string patternStr10 = "0 0 * */1 5";
            var fResult10 = new CronTabScheduler().NextOccurances(patternStr10,startDate,endDate);
            

            string patternStr9 = "0 0 * * SAT,SUN,MON,TUE,WED,THU";
            var fResult9 = new CronTabScheduler().NextOccurances(patternStr9,startDate,endDate);


            string patternStr8 = "*/360 * * * *";
            var f8Result = new CronTabScheduler().NextOccurances(patternStr8, startDate, endDate);

            string patternStr7 = "50 2 * * *";
            var f7Result = new CronTabScheduler().NextOccurances(patternStr7, startDate, endDate);

            string patternStr6 = "0 0 */10 * *";
            var f6Result = new CronTabScheduler().NextOccurances(patternStr6, startDate, endDate);

            string secondOfEveryOtherMonth = "0 0 10 */2 *";
            var f5Result = new CronTabScheduler().NextOccurances(secondOfEveryOtherMonth, startDate, endDate);

            string everyWeekSatSun = "0 0 * * SAT,SUN";
            var f3Result = new CronTabScheduler().NextOccurances(everyWeekSatSun, startDate, endDate);

            string firstOfEveryMonth = "0 0 1 */1 *";
            var f1Result = new CronTabScheduler().NextOccurances(firstOfEveryMonth, startDate, endDate);

            string everyOtherDay = "0 0 */2 * *";
            var f2Result = new CronTabScheduler().NextOccurances(everyOtherDay, startDate, endDate);
            
           
          
        }

        [TestMethod]
        public void TestNextOccurance()
        {
            var startDate = new DateTime(2016, 6, 1, 10, 50, 25);
            var endDate = new DateTime(2016, 12, 1, 10, 50, 25);

            string patternStr11 = "0 0 8-14 */1 TUE";

            var res3 = new CronTabScheduler().NextOccurance(patternStr11, startDate, endDate, new DateTime(2016, 8, 1, 10, 50, 25));

            var res4 = new CronTabScheduler().NextOccurance(patternStr11, startDate, endDate, new DateTime(2016, 6, 1, 10, 50, 25));

            var res5 = new CronTabScheduler().NextOccurance(patternStr11, startDate, endDate, "1395/09/01 10:20");
        }

        [TestMethod]
        public void TestNextOccurance2()
        {
            var startDate = new DateTime(2016, 1, 1, 10, 50, 25);
            var endDate = new DateTime(2019, 12, 1, 10, 50, 25);
            string patternStr11 = "0 0 */2 * *"; 

            for (var i = startDate; i < endDate; i = i.AddDays(1))
            {
                var res3 = new CronTabScheduler().NextOccurance(patternStr11, i, endDate, i.AddDays(2));
            }
            
        }

        [TestMethod]
        public void TestNextOccurance3()
        {
            var startDate = new DateTime(2016, 5, 1, 10, 50, 25);
            var endDate = new DateTime(2016, 12, 1, 10, 50, 25);
            string patternStr11 = "0 0 */2 * *";

            var res = new CronTabScheduler().NextOccurances(patternStr11, startDate, endDate );


            for (var i = startDate; i < endDate; i = i.AddDays(1))
            {
                var res3 = new CronTabScheduler().NextGregorianOccurance(patternStr11, i, endDate, i.AddDays(2));
            }

        }

       
    }
}
