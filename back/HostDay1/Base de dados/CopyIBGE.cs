using hostDay.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static hostDay.Models.Cidade;

namespace hostDay.Base_de_dados
{
    public class CopyIBGE:IDisposable
    {   
        /// <summary>
        /// Copia todas as informações úteis da API do IBGE
        /// </summary>
        public void copiaTudo()
        {
            insertRegioes();
            insertEstados();
            insertMesorregioes();
            insertCidades();
        }
        /// <summary>
        /// Verifica se a algum dado no banco de dados
        /// </summary>
        /// <returns></returns>
        public bool dadosNaBase()
        {
            SqlConnection cone = Conexao.sqlConnection();
            SqlCommand sql = new SqlCommand("select count(*) from Regioes",cone);
            cone.Open();
            SqlDataReader read = sql.ExecuteReader();
            read.Read();
            int count = Convert.ToInt32(read[0]);
            cone.Close();
            return count > 0;


        }
        public void insertRegioes() {
            DataTable regioes = new DataTable();
            regioes.Columns.Add("id");
            regioes.Columns.Add("sigla");
            regioes.Columns.Add("nome");
            List<Regiao> regiaos = GetRegiaos();
            regiaos.ForEach(d => {
                regioes.Rows.Add(d.id,d.sigla,d.nome);
            });
            SqlConnection cone =  Conexao.sqlConnection();
            cone.Open();
            SqlBulkCopy copy = new SqlBulkCopy(cone);
            copy.DestinationTableName = "Regioes";
            copy.WriteToServer(regioes);
            cone.Close();
        }
        public void insertEstados() {
            DataTable estados = new DataTable();
            estados.Columns.Add("id");
            estados.Columns.Add("idRegiao");
            estados.Columns.Add("sigla");
            estados.Columns.Add("nome");
            List<Estado> regiaos = GetEstados();
            regiaos.ForEach(d => {
                estados.Rows.Add(d.id,d.regiao.id,d.sigla ,d.nome);
            });
     
            SqlConnection cone = Conexao.sqlConnection();
            cone.Open();
            SqlBulkCopy copy = new SqlBulkCopy(cone);
            copy.DestinationTableName = "Estados";
            copy.WriteToServer(estados);
            cone.Close();
        }
        public void insertMesorregioes()
        {
            DataTable mesorregioes = new DataTable();
            mesorregioes.Columns.Add("id");
            mesorregioes.Columns.Add("idEstado");
            mesorregioes.Columns.Add("nome");
            List<Mesorregiao> regiaos = GetMesorregiaos();
          
            regiaos.ForEach(d => {
                mesorregioes.Rows.Add(d.id,d.Estado.id,d.nome);
            });
            SqlConnection cone = Conexao.sqlConnection();
            cone.Open();
            SqlBulkCopy copy = new SqlBulkCopy(cone);
            copy.DestinationTableName = "Mesorregiao";
            copy.WriteToServer(mesorregioes);
            cone.Close();
        }
        public void insertCidades()
        {
            DataTable cidades = new DataTable();
            cidades.Columns.Add("id");
            cidades.Columns.Add("idMesorregiao");
            cidades.Columns.Add("nome");
            List<Cidade> regiaos = GetCidades();
            regiaos.ForEach(d => {
                cidades.Rows.Add(d.id,d.microrregiao.mesorregiao.id,d.nome);
            });
            SqlConnection cone = Conexao.sqlConnection();
            cone.Open();
            SqlBulkCopy copy = new SqlBulkCopy(cone);
            copy.DestinationTableName = "Cidades";
            copy.WriteToServer(cidades);
            cone.Close();

        }
        public List<Regiao> GetRegiaos()
        {
            var requisicaoWeb = WebRequest.CreateHttp("https://servicodados.ibge.gov.br/api/v1/localidades/regioes");
            requisicaoWeb.Method = "GET";
            requisicaoWeb.UserAgent = "RequisicaoWebDemo";
            using (var resposta = requisicaoWeb.GetResponse())
            {
                var streamDados = resposta.GetResponseStream();
                StreamReader reader = new StreamReader(streamDados);
                object objResponse = reader.ReadToEnd();
                var post = JsonConvert.DeserializeObject<List<Regiao>>(objResponse.ToString());

                streamDados.Close();
                resposta.Close();

                return post;
            }

        } 
        public List<Estado> GetEstados()
        {
            var requisicaoWeb = WebRequest.CreateHttp("https://servicodados.ibge.gov.br/api/v1/localidades/estados");
            requisicaoWeb.Method = "GET";
            requisicaoWeb.UserAgent = "RequisicaoWebDemo";
            using (var resposta = requisicaoWeb.GetResponse())
            {
                var streamDados = resposta.GetResponseStream();
                StreamReader reader = new StreamReader(streamDados);
                object objResponse = reader.ReadToEnd();
                var post = JsonConvert.DeserializeObject<List<Estado>>(objResponse.ToString());

                streamDados.Close();
                resposta.Close();

                return post;
            }
        } 
        public List<Mesorregiao> GetMesorregiaos()
        {
            var requisicaoWeb = WebRequest.CreateHttp("https://servicodados.ibge.gov.br/api/v1/localidades/mesorregioes");
            requisicaoWeb.Method = "GET";
            requisicaoWeb.UserAgent = "RequisicaoWebDemo";
            using (var resposta = requisicaoWeb.GetResponse())
            {
                var streamDados = resposta.GetResponseStream();
                StreamReader reader = new StreamReader(streamDados);
                object objResponse = reader.ReadToEnd();
                var post = JsonConvert.DeserializeObject<List<Mesorregiao>>(objResponse.ToString());

                streamDados.Close();
                resposta.Close();

                return post;
            }

        }
        public List<Cidade> GetCidades()
        {

            var requisicaoWeb = WebRequest.CreateHttp("https://servicodados.ibge.gov.br/api/v1/localidades/municipios");
            requisicaoWeb.Method = "GET";
            requisicaoWeb.UserAgent = "RequisicaoWebDemo";
            using (var resposta = requisicaoWeb.GetResponse())
            {
                var streamDados = resposta.GetResponseStream();
                StreamReader reader = new StreamReader(streamDados);
                object objResponse = reader.ReadToEnd();
                var post = JsonConvert.DeserializeObject<List<Cidade>>(objResponse.ToString());

                streamDados.Close();
                resposta.Close();

                return post;
            }

        }

        public void Dispose()
        {
            
        }
    }
}
