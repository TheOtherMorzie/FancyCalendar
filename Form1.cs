namespace FancyCalendar
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            List_TextBoxLabel = new List<Tuple<TextBox, Control>>()
            {
                new(textBox1,   label1),
                new(textBox2,   label2),
                new(textBox3,   label3),
                new(textBox4,   label4),
                new(textBox5,   label5),
                new(textBox6,   label6),
                new(textBox7,   label7),
                new(textBox8,   label8),
                new(textBox9,   label9),
                new(textBox10,  label10),
                new(textBox11,  label11),
                new(textBox12,  textBox_CustomDate1),
                new(textBox13,  textBox_CustomDate2),
            };
        }

        List<Tuple<TextBox, Control>> List_TextBoxLabel;

        void PollDates(DateTime date)
        {
            foreach (var obj in List_TextBoxLabel)
            {
                string format = obj.Item2.Text;
                try
                {
                    obj.Item1.Text = date.ToString(format);
                }
                catch (FormatException e)
                {
                    obj.Item1.Text = "[InvalidFormat]";
                }
            }
        }


        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            var date = dateTimePicker1.Value;
            monthCalendar1.SetDate(date);
            PollDates(date);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            if (checkBox1.Checked) PollDates(now);
            var time = now.ToLongTimeString();
            var date = now.ToLongDateString();
            textBox_LiveDate.Text = time + " |----| " + date;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            dateTimePicker1.Value = e.Start;
        }
    }
}