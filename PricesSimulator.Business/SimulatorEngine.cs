using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricesSimulator.Business
{
    public static class SimulatorEngine
    {
        static SimulatorEngine()
        {
            SubscriptionSources = new List<string>();
            Tickers = new List<Ticker>();
        }
        public static List<string> SubscriptionSources
        {
            get;
            set;
        }
        public static List<Ticker> Tickers
        {
            get;
            set;
        }
        public static void Simulate()
        {
            foreach (var sourceName in SubscriptionSources)
            {
                Random r = new Random();
                lock (Tickers)
                {
                    var existingSource = Tickers.FirstOrDefault(t=>t.Name == sourceName);
                    if (existingSource == null)
                    {
                        Tickers.Add(new Ticker
                        {
                            Name = sourceName,
                            Price = (float)Math.Round(r.NextDouble() * 1000,2)
                        });
                    }
                    else
                    {
                        existingSource.Price = (float)Math.Round(existingSource.Price + (r.NextDouble() * r.Next(-1, 1)),2);
                    }
                }

            }            

        }        
        public static void Subscribe(string ticker)
        {
            SubscriptionSources.Add(ticker);
            Simulate();
        }
    }
}
