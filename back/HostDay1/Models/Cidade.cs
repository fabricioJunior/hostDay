using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hostDay.Models
{
    public class Cidade
    {
        public int id { get; set; }
        public string nome { get; set; }
        public Microrregiao microrregiao { get;set;}

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            return id == ((Cidade)obj).id;
        }
        public override int GetHashCode()
        {
            int hCode = this.id;
            return hCode.GetHashCode();
        }
        /*
         Pequena fuga no modelo, apenas para a serialização em JSON;
         */
        public class Microrregiao
        {
            public int id { get; set; }
            public string nome { get; set; }
            public Mesorregiao mesorregiao { get; set; }
        }
    }

    
}
