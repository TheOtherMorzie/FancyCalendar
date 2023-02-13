using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FancyCalendar.Calendars
{
    public partial class CalendarForm_stdWeek : Form
    {
        public CalendarForm_stdWeek()
        {
            InitializeComponent();
        }

        int GetWeekNumber(DateTime dateTime)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;

            return cal.GetWeekOfYear(dateTime, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        }

        public static DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            // Use first Thursday in January to get first week of the year as
            // it will never be in Week 52/53
            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            // As we're adding days to a date in Week 1,
            // we need to subtract 1 in order to get the right date for week #1
            if (firstWeek == 1)
            {
                weekNum -= 1;
            }

            // Using the first Thursday as starting week ensures that we are starting in the right year
            // then we add number of weeks multiplied with days
            var result = firstThursday.AddDays(weekNum * 7);

            // Subtract 3 days from Thursday to get Monday, which is the first weekday in ISO8601
            return result.AddDays(-3);
        }

        void SetSelection_Week(int weekNumber)
        {
            var mon = FirstDateOfWeekISO8601(DateTime.Now.Year, weekNumber);
            var end = mon.AddDays(7);
            _lastRange = new(mon, end);
            monthCalendar1.SetSelectionRange(mon, end);
        }

        void SetSelection_Range(SelectionRange range)
        {
            monthCalendar1.SetSelectionRange(range.Start, range.End);
            monthCalendar1.SelectionRange = range;
        }

        int RangeLength(SelectionRange range)
        {
            var diff = range.End - range.Start;
            return diff.Days;
        }

        bool CompareSelectionRange(SelectionRange range1, SelectionRange range2)
        {
            if (range1.Start == range2.Start) return false;
            if (range1.End == range2.End) return false;
            return true;
        }

        SelectionRange? _lastRange;
        string _format_shortDate = "d/MM/yy";
        string _format_shortDateTime = "d/M h:mm";
        string _format_longDate = "dd/MM/yyyy";
        string _format_longDateTime = "d/M/yyyy HH:mm";

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            SelectionRange rangeOld = monthCalendar1.SelectionRange;
            SelectionRange rangeNew = new(e.Start, e.End);
            if (CompareSelectionRange(rangeOld, rangeNew)) return;
            if (RangeLength(rangeNew) <= 1)
            {

                monthCalendar1.SuspendLayout();

                var date = e.Start;
                textBox1.Text = "Date: " + date.ToString(_format_shortDate);
                var num = GetWeekNumber(date);
                textBox2.Text = "Week: " + num;
                SetSelection_Week(num-1);
                textBox3.Text = rangeOld.Start.ToString(_format_shortDate) + " <--> " + rangeOld.End.ToString(_format_shortDate);
                monthCalendar1.ResumeLayout();


            }


        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
