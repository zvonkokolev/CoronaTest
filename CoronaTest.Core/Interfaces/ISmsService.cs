using System;
using System.Collections.Generic;
using System.Text;

namespace CoronaTest.Core.Interfaces
{
    public interface ISmsService
    {
        /// <summary>
        /// Sendet eine SMS.
        /// </summary>
        /// <param name="to">Handynummer des Empfänger</param>
        /// <param name="message">Inhalt der Nachricht</param>
        /// <returns>Ob erfolgreich versendet wurde</returns>
        bool SendSms(string to, string message);
    }
}
