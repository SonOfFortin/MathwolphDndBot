using System;

namespace MathwolphDndBot.MVVM.Model
{
    public enum MessageType
    {
        First = 0,
        TimeDiff = 1,
        AffDate = 2,
        Normal = 3
    }

    internal class Message
    {
        public User? User { get; set; }
        public string? Msg {  get; set; }
        public MessageType Type { get; set; }
        public DateTime Moment { get; set; }

        public string AffMoment
        {
            get { 
                if (Type == MessageType.First || Type == MessageType.AffDate)
                {
                    var dDif = (int)(DateTime.Now - Moment).TotalDays;

                    if (dDif > 7)
                    {
                        if(dDif > 365) 
                        {
                            return Moment.ToString("dd/MM/yyyy h:mm tt");
                        }

                        return Moment.ToString("dd/MM h:mm tt");
                    }

                    if (dDif < 2)
                    {
                        if(Type == MessageType.AffDate)
                        {
                            return (dDif == 0 ? string.Empty : "Hier ") + Moment.ToString("h:mm tt");
                        }

                        return "Aujourd'hui " + Moment.ToString("h:mm tt");
                    }

                    return Moment.ToString("dddd h:mm tt");
                }

                if (Type == MessageType.TimeDiff)
                {
                    return Moment.ToString("h:mm tt");
                }

                return string.Empty; 
            }
        }

    }
}
