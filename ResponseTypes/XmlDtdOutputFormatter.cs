using System;
using System.Text;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace Customer_API.ResponseTypes
{

    public class XmlDtdOutputFormatter : TextOutputFormatter
    {
        public XmlDtdOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/xml-dtd"));
            SupportedEncodings.Add(Encoding.UTF8);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (selectedEncoding == null)
                throw new ArgumentNullException(nameof(selectedEncoding));

            var response = context.HttpContext.Response;
            var xml = "<!DOCTYPE xml>";
            return response.WriteAsync(xml, selectedEncoding);
        }
    }

}
