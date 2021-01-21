﻿using CoronaTest.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace CoronaTest.Core.Services
{
    public class TwilioSmsService : ISmsService
    {
        private readonly string _accountSid;
        private readonly string _authToken;

        public TwilioSmsService(string accountSid, string authToken)
        {
            _accountSid = accountSid;
            _authToken = authToken;
        }

        public bool SendSms(string to, string message)
        {
            try
            {
                // Find your Account Sid and Token at twilio.com/console
                // and set the environment variables. See http://twil.io/secure
                string accountSid = _accountSid;
                string authToken = _authToken;

                TwilioClient.Init(accountSid, authToken);

                var sms = MessageResource.Create(
                    body: message,
                    from: new Twilio.Types.PhoneNumber("+13213254289"),
                    to: new Twilio.Types.PhoneNumber(to)
                );
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }
    }
}
