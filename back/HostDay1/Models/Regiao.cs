using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hostDay.Models
{
    public class Regiao
    {
        public int id { get; set; }
        public string nome { get; set; }
        public string sigla { get; set; }

        public override bool Equals(object obj)
        { 
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            return id == ((Regiao)obj).id;
        }
        public override int GetHashCode()
        {
            int hCode = this.id;
            return hCode.GetHashCode();
        }
    }
}
