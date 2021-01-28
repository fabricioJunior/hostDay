using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hostDay.Models
{
    public class Estado : Regiao
    {  

        public Regiao regiao {get;set;}


        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            return id == ((Estado)obj).id;
        }
        public override int GetHashCode()
        {
            int hCode = this.id;
            return hCode.GetHashCode();
        }
    }
}
