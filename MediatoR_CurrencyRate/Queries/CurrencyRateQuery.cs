using MediatoR_CurrencyRate.Models;
using MediatR;
using System.Globalization;
using System.Text;
using System.Xml.Linq;

namespace MediatoR_CurrencyRate.Queries
{
    public class CurrencyRateQuery : IRequest<Currencies> { }

    public class CurrencyRateQueryHandler : IRequestHandler<CurrencyRateQuery, Currencies>
    {
        public async Task<Currencies> Handle(CurrencyRateQuery request, CancellationToken cancellationToken)
        {
            try
            {             
                Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-Ru");

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                XDocument xml = XDocument.Load("https://cbr.ru/scripts/XML_daily.asp");

                var USD = Convert.ToDouble(xml.Elements("ValCurs").Elements("Valute")
                    .FirstOrDefault(x => x.Element("NumCode").Value == "840").Elements("Value").FirstOrDefault().Value);

                var EUR = Convert.ToDouble(xml.Elements("ValCurs").Elements("Valute")
                   .FirstOrDefault(x => x.Element("NumCode").Value == "978").Elements("Value").FirstOrDefault().Value);
                
                return new Currencies((float)Math.Round(USD,2), (float)Math.Round(EUR, 2));
            }
            catch (Exception ex)
            {               
                return default;
            }                       
        }
    }
}
