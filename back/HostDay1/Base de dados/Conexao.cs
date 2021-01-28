using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace hostDay.Base_de_dados
{
    public class Conexao
    {
        public static string cone = @"Persist Security Info=False;User ID=sa;Initial Catalog=hostDay;Data Source=DESKTOP-9O7FBSO\SQLEXPRESS; Password = 44668822";
        
        public static SqlConnection sqlConnection()
        {
            SqlConnection nova = new SqlConnection(cone);
            return nova;
        }
    }
}
