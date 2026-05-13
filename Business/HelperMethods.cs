using Ludo.Models;
using System.Globalization;

namespace Ludo.Business
{
    public static class HelperMethods
    {
        public static DateTime ConvertShamsiToMiladi(string date, string time)
        {
            DateTime persianDateTime;

            var dateParts = date.Split('/');
            int year = int.Parse(dateParts[0]);
            int month = int.Parse(dateParts[1]);
            int day = int.Parse(dateParts[2]);

            int hour = 0;
            int minute = 0;

            if (!string.IsNullOrEmpty(time))
            {
                var timeParts = time.Split(":");
                hour = Convert.ToInt32(timeParts[0]);
                minute = Convert.ToInt32(timeParts[1]);
            }

            PersianCalendar pc = new PersianCalendar();
            DateTime gregorianDate = pc.ToDateTime(year, month, day, hour, minute, 0, 0);

            return gregorianDate;
        }

        public static string ConvertMiladiToShamsi(DateTime date, bool showTime)
        {
            var pc = new PersianCalendar();

            return $"{pc.GetYear(date):0000}/{pc.GetMonth(date):00}/{pc.GetDayOfMonth(date):00} {(showTime ? date.ToShortTimeString() : "")}";
        }


        public static ReservationStatus GetReservationStatus(DateTime from, DateTime to)
        {
            var now = DateTime.Now;
            if (now > from && now < to)
            {
                return ReservationStatus.Ongoing;
            }

            if (now < from)
            {
                return ReservationStatus.Pending;
            }

            if (now > to)
            {
                return ReservationStatus.Past;
            }

            return ReservationStatus.Invalid;
        }

        public static string GetReservationStatusTitle(ReservationStatus status)
        {
            var title = "";
            switch (status)
            {
                case ReservationStatus.Past:
                    title = "گذشته";
                    break;
                case ReservationStatus.Ongoing:
                    title = "درحال انجام";
                    break;
                case ReservationStatus.Pending:
                    title = "طبق برنامه";
                    break;
                case ReservationStatus.Invalid:
                    title = "نامشخص";
                    break;
                default:
                    break;
            }

            return title;
        }

        public static string GetReservationStatusStyleClass(ReservationStatus status)
        {
            var className = "";
            switch (status)
            {
                case ReservationStatus.Past:
                    className = "reservationPast";
                    break;
                case ReservationStatus.Ongoing:
                    className = "reservationOngoing";
                    break;
                case ReservationStatus.Pending:
                    className = "reervationScheduled";
                    break;
                case ReservationStatus.Invalid:
                    className = "reservationInvalid";
                    break;
                default:
                    break;
            }

            return className;
        }

        public static string GetStationClass(Station model)
        {
            if (!model.IsActive)
            {
                return "resizable-disabled";
            }
            else if (model.ReservationStations != null && model.ReservationStations.Any())
            {
                if(model.ReservationStations.First().Reservation.From > DateTime.Now)
                {
                    return "resizable-coming";
                }

                return "resizable-full";
            }

            return "resizable-available";
        }
    }
}
