using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ParseMail{
    public class InfoAvto {
        string Brand;
        string Model;
        string Year;
        string DateRegistration;
        string Volume;
        
        public InfoAvto() {
            this.Brand = "";
            this.Model = "";
            this.Year = "";
            this.DateRegistration = "";
            this.Volume = "";
        }
        public InfoAvto(string Brand, string Model, string Year, string DateRegistration, string Volume) {
            this.Brand = Brand;
            this.Model = Model;
            this.Year = Year;
            this.DateRegistration = DateRegistration;
            this.Volume = Volume;
        }
    }
    class Program{
        static (string, List<InfoAvto>) ParseMail(string File){
            Regex regex = new Regex(@" \d{10}");
            string IDcod = regex.Match(File).Value.Substring(1);
            
            int index = File.IndexOf("<table cellspacing=\"0\" cellpadding=\"7\"");
            File = File.Substring(index);
            index = File.IndexOf("</table>");
            File = File.Substring(0, index + 1);
            
            List<InfoAvto> ListInfo = new List<InfoAvto>();
            //если нет автомобилей
            if(File.Split('\n').Length < 10)
                return (IDcod, ListInfo);
            //если есть
            else {
                List<string> list = new List<string>();
                foreach(var item in File.Split('\n')) {
                    if(item.Contains(@"<td>"))
                        list.Add(item.Substring(4, (item.Length - 10)));
                }
                for(int x = 0; x < list.Count; x += 5) {
                    InfoAvto infoAvto = new InfoAvto(list[x], list[x + 1], list[x + 2], list[x + 3], list[x + 4]);
                    ListInfo.Add(infoAvto);
                }
                return (IDcod, ListInfo);
            }
        }
        static void Main(string[] args){
            Console.OutputEncoding = System.Text.Encoding.Default;
            StreamReader reader = new StreamReader("EstAvto.txt");
            string EstAvto = reader.ReadToEnd();
            reader.Close();
            reader = new StreamReader("NetAvto.txt");
            string NetAvto = reader.ReadToEnd();
            reader.Close();
            (string, List<InfoAvto>) result1 =  ParseMail(EstAvto);
            (string, List<InfoAvto>) result2 = ParseMail(NetAvto);
            Console.WriteLine(result1);
            Console.WriteLine(result2);


        }
    }
}
