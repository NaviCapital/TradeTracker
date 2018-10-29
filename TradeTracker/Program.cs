using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TradeTracker
{
    class Program
    {
        // args: "username", "password", "dd/mm/yyyy"
        static void Main(string[] args)
        {
            if (args.Length < 2) return;

            string user = args[0];
            string pass = args[1];
            string date = args.Length == 3 ? args[2] : String.Format("{0:dd/MM/yyyy}", DateTime.Now);

            if (user == null || pass == null) throw new Exception("BBI credentials missing");

            SmartBbiService.WServiceSMBradClient client = new SmartBbiService.WServiceSMBradClient();
            string trades_bov = client.ObtemNegociosBOV(user, pass, date, "", false);
            string trades_bmf = client.ObtemNegocios(user, pass, date, "");
            ProjetUtils.SaveFile(trades_bov, "bov", date);
            ProjetUtils.SaveFile(trades_bmf, "bmf", date);
        }
    }

    static class ProjetUtils
    {
        public static bool SaveFile(string trades, string kind, string date)
        {
            date = String.Format("{0:yyyyMMdd}", DateTime.ParseExact(date, "dd/MM/yyyy", null));
            string filename = "negocios_" + kind + "_" + date + ".xml";
            string path = @"C:\\tmp\" + filename;

            try
            {
                StreamWriter SW;
                SW = new StreamWriter(path, false, Encoding.GetEncoding(1252));
                SW.WriteLine(trades);
                SW.Close();
                SW.Dispose();
                Console.WriteLine("Successfully updated " + filename);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
