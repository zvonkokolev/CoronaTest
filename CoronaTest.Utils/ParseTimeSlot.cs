using CoronaTest.Core.Entities;
using CoronaTest.Core.Interfaces;
using CoronaTest.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using static CoronaTest.Core.Enums.Enums;

namespace CoronaTest.Utils
{
    public class ParseTimeSlot
    {
        #region fields
        private static int _count = 0;
		private static int _period;
        private IUnitOfWork _uow;
        #endregion

        #region properties
        public int Count { get => _count; set => _count = value; }
        #endregion
        //Constructor - Standard
        public ParseTimeSlot(IUnitOfWork unitOfWork)
        {
			_uow = unitOfWork;
        }

		#region methods
		/// <summary>
		/// Returns interval-number from input (time as string in hh:mm format)
		/// for example 02:00 ==> interval-number=8 (1 slot equals 15 minutes)
		/// </summary>
		/// <param name="timeString"></param>
		/// <returns></returns>
		public static int ParseTimeString(string timeString)
		{
			int value = -1, hours = 0, minutes = 0, z = -1;
			if (timeString != null && timeString.Contains(':'))
			{
				bool h = false, m = false;
				string[] tempus = timeString.Split(":");
				h = int.TryParse(tempus[0], out hours);
				m = int.TryParse(tempus[1], out minutes);
				if (h && m && hours < 24) // && hours * 60 % 15 == 0 && minutes < 60 && minutes % 15 == 0)
				{
					value = (hours * 60 + minutes) / 15;
				}
			}
			else if (timeString != null && int.TryParse(timeString, out z) && z <= 95 && z >= 0)
			{
				value = z;
			}
			return value;
		}

		/// <summary>
		/// Returns amount of slots for interval of time 
		/// </summary>
		/// <param name="beginTime"></param>
		/// <param name="endTime"></param>
		/// <returns></returns>
		public int CountInIntervall(string beginTime, string endTime)
		{
			int inValue = -1, bt = -1, et = -1;
			et = ParseTimeString(endTime);
			bt = ParseTimeString(beginTime);
			if (bt >= 0 && et >= 0 && et >= bt)
			{
				inValue = et - bt;
			}
			return inValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="time"></param>
		/// <param name="exam"></param>
		public static DateTime SetExaminationAtTime(DateTime dateTime)
		{
			_period = ParseTimeString(dateTime.ToShortTimeString());
			string ymd = dateTime.ToShortDateString();
			string temp = SetExaminationAtPeriod(_period);
			string dateTimeAsString = ymd + ' ' + temp;

			DateTime dt = new DateTime();
			dt = Convert.ToDateTime(dateTimeAsString);
			return dt;
		}

		private static string SetExaminationAtPeriod(int period)
        {
			int hour, minutes;
			StringBuilder sb = new StringBuilder();
			
            if (period < 96 && period >= 0)
            {
				hour = (period * 15) / 60;
				sb.Append(hour.ToString());
				sb.Append(':');
				minutes = (period * 15) % 60;
				sb.Append(minutes.ToString());
			}
            return sb.ToString();
        }
		/*
		public double GetAverageTemperatureAllDay()
		{
			double sum = 0;
			for (int i = 0; i < Program.measurements.Length; i++)
			{
				if (Program.measurements[i] == null)
					continue;
				sum = sum + Program.measurements[i].Temperature;
			}
			return sum / Count;
		}

		public double GetAverageHumidityAllDay
		{
			get
			{
				double sum = 0;
				for (int i = 0; i < Program.measurements.Length; i++)
				{
					if (Program.measurements[i] == null)
						continue;
					sum = sum + Program.measurements[i].Humidity;
				}
				return sum / Count;
			}
		}
		*/
		#endregion
	}
}
