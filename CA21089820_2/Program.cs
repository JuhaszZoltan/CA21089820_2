using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace CA21089820_2
{
    class Palinka
    {
        private int _fok;
        private string _gyumolcs;
        private int _mennyiseg;
        private int _ev;
        private int _ar;

        public int Fok
        {
            get => _fok;
            set
            {
                if (value < 0)
                    throw new Exception("hiba: túl alacsony alkoholfok");
                if (value > 87)
                    throw new Exception("hiba: túl magas alkioholfok");

                _fok = value;
            }
        }
        public string Gyumolcs
        {
            get => _gyumolcs;
            set
            {
                if (value is null)
                    throw new Exception("hiba: a gyümölcs nem lehet null");
                if (value.Length < 3)
                    throw new Exception("hiba: túl rövid gyümölcsnév");
                if (value.Length > 20)
                    throw new Exception("hiba: túl hosszú gyümölcsnév");
                
                _gyumolcs = value;
            }
        }
        public int Mennyiseg
        {
            get => _mennyiseg;
            set
            {
                if (value < 0)
                    throw new Exception("hiba: a mennyiség nem lehet negatív");
                if (value > 50)
                    throw new Exception("hiba: túl sok pálinka!");

                _mennyiseg = value;
            }
        }
        public int Ev
        {
            get => _ev;
            set
            {
                if (value < 2000)
                    throw new Exception("hiba: túl öreg pálinka!");
                if (value > DateTime.Now.Year)
                    throw new Exception("hiba: pálinka a jövőből!");

                _ev = value;
            }
        }

        public int Eves
        {
            get
            {
                return DateTime.Now.Year - this.Ev;
            }
        }

        public int Ar
        {
            get => _ar;
            set
            {
                if (value < 50)
                    throw new Exception("hiba: túl olcsó pálinka!");
                if (value > 1000)
                    throw new Exception("hiba: túl drága pálinka!");

                _ar = value;
            }
        }

        //private string _pwdHash;
        private byte[] _pwdHash;
        public string Jelszo
        {
            set
            {
                //var md5Hash = MD5.Create();
                //var sourceBytes = Encoding.UTF8.GetBytes(value);
                //var hashBytes = md5Hash.ComputeHash(sourceBytes);
                //var hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

                var md5Hash = MD5.Create();

                _pwdHash = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(value));


            }
        }

        public bool Ellenorzes(string jelszo)
        {
            //var md5Hash = MD5.Create();
            //var sourceBytes = Encoding.UTF8.GetBytes(jelszo);
            //var hashBytes = md5Hash.ComputeHash(sourceBytes);
            //var hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

            var md5Hash = MD5.Create();

            return Enumerable.SequenceEqual(
                md5Hash.ComputeHash(Encoding.UTF8.GetBytes(jelszo)),
                _pwdHash);

            //md5Hash.ComputeHash(Encoding.UTF8.GetBytes(jelszo)) 
        }

    }




    class Program
    {
        static Random rnd = new Random();
        static string[] gyumolcsok =
        {
            "szilva",
            "barack",
            "körte",
            "dió",
            "alma",
            "kaktusz",
        };

        static void Main(string[] args)
        {
            var palinkak = new List<Palinka>();

            for (int i = 0; i < 20; i++)
            {
                palinkak.Add(new Palinka()
                {
                    Fok = rnd.Next(30, 88),
                    Gyumolcs = gyumolcsok[rnd.Next(gyumolcsok.Length)],
                    Ar = rnd.Next(5, 101) * 10,
                    Ev = rnd.Next(2000, DateTime.Now.Year + 1),
                    Mennyiseg = rnd.Next(10, 51),
                });
            }

            Kiir(palinkak);

            int bevetel = 0;
            for (int i = 0; i < 50; i++)
            {
                int j = rnd.Next(palinkak.Count);
                bevetel += (palinkak[j].Mennyiseg / 2) * palinkak[j].Ar;
                palinkak[j].Mennyiseg -= palinkak[j].Mennyiseg / 2;
            }

            Console.WriteLine($"bevétel: {bevetel} Ft");

            Kiir(palinkak);

            //var p = new Palinka();
            //p.Jelszo ="1234";
            //Console.WriteLine(p.Ellenorzes(Console.ReadLine()));


            Console.ReadKey();
        }

        private static void Kiir(List<Palinka> lista)
        {
            Console.WriteLine("-----------------------------");
            foreach (var p in lista)
            {
                Console.WriteLine("{0, -7} pálinka ({1}) {2}% {3:0.0} liter {4,4}Ft/dl",
                    p.Gyumolcs, p.Ev, p.Fok, (float)p.Mennyiseg / 10, p.Ar);
            }
            Console.WriteLine("-----------------------------");
        }
    }
}
