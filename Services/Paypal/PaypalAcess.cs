using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BookBarn.Services.Paypal
{
    public class PaypalAcess
    {
        public static PaypalRedirect ExpressCheckout(PaypalOrder order)
        {
            var values = Submit(new Dictionary<string, string>
            {
                ["USER"] = PaypalSettings.Username,
                ["PWD"] = PaypalSettings.Password,
                ["SIGNATURE"] = PaypalSettings.Signature,
                ["METHOD"] = "SetExpressCheckout",
                ["VERSION"] = "93", //2.3
                ["RETURNURL"] = PaypalSettings.ReturnUrl,
                ["CANCELURL"] = PaypalSettings.CancelUrl,
                ["AMT"] = order.Amount.ToString(),
                ["TRXTYPE"] = "S",
                //["PAYMENTACTION"] = "Sale";
                ["CURRENCYCODE"] = "USD"
                // ["BUTTONSOURCE"] = "PP-ECWizard";
                // ["SUBJECT"] = "Test";
            });

            string ack = values["ACK"].ToLower();

            if (ack.Equals("success") || ack.Equals("successwithwarning"))
            {
                return new PaypalRedirect
                {
                    Token = values["TOKEN"],
                    Url = $"{PaypalSettings.CgiDomain}?cmd=_express-checkout&token={values["TOKEN"]}"
                };
            }
            else
            {
                throw new ArgumentException(values["L_LONGMESSAGE0"]);
            }
        }

        private static Dictionary<string, string> Submit(Dictionary<string, string> values)
        {
            HttpContent content = new FormUrlEncodedContent(values);
            Task<HttpResponseMessage> response = new HttpClient().PostAsync(PaypalSettings.ApiDomain, content);

            string responceResult = response.Result.Content.ReadAsStringAsync().Result;
            var result = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(responceResult);
            var finalResult = result.ToDictionary(item => item.Key, item => item.Value[0]);

            return finalResult;
        }
    }
}
