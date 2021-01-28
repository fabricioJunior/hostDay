using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hostDay.Models
{
    public class Mesorregiao:Regiao
    {   
        [JsonProperty("UF")]
        public Estado Estado { get; set; }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            return id == ((Mesorregiao)obj).id;
        }
        public override int GetHashCode()
        {
            int hCode = this.id;
            return hCode.GetHashCode();
        }
    }
}
