using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using hostDay.Models;
using System.Data.SqlClient;

namespace hostDay.Base_de_dados
{
    public class GetModels
    {

        public static List<Cidade> GetCidades()
        {
            List<Cidade> retorno = new List<Cidade>();
            SqlConnection cone = Conexao.sqlConnection();
            SqlCommand selet = new SqlCommand("select * from cidadesView", cone);
            
            cone.Open();
            SqlDataReader read = selet.ExecuteReader();
            while (read.Read())
            {
                Cidade nova = new Cidade();
                nova.id = Convert.ToInt32(read["id"]);
                nova.nome = read["nome"].ToString();
               
                Cidade.Microrregiao nova1 = new Cidade.Microrregiao();
                Regiao regiao = regiao = new Regiao()
                {
                    id = Convert.ToInt32(read["idRegiao"]),
                    nome = read["nomeRegiao"].ToString(),
                    sigla = read["siglaRegiao"].ToString()
                };
                Estado estado = new Estado()
                {
                    id = Convert.ToInt32(read["idEstado"]),
                    sigla = read["sigla"].ToString(),
                    nome = read["nomeEstado"].ToString(),
                    regiao = regiao
                };
               
                Mesorregiao mesorregiao = new Mesorregiao()
                {
                    nome = read["nomeMesorregiao"].ToString(),
                    id = Convert.ToInt32(read["idMesorregiao"]),
                    Estado = estado
                };
                nova.microrregiao = new Cidade.Microrregiao()
                {
                    id = 0,
                    mesorregiao = mesorregiao
                };
                retorno.Add(nova);
            }
            cone.Close();
            return retorno;

        }
    }
}
