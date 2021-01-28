using hostDay.Base_de_dados;
using hostDay.Email;
using hostDay.JSON;
using hostDay.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace hostDay.Planilhas
{
    public class PlanilhaFactory
    {

        /// <summary>
        /// Cria a planilha em formato excel e retorna o nome do arquivo
        /// </summary>
        /// <param name="cidades"></param>
        /// <returns></returns>
        public string getPlanilha(List<Cidade> cidades)
        {  /*
              siglaEstado, regiaoNome,
nomeCidade, nomeMesorregiao, nomeFormatado {cidade/UF}
            */
            DataTable dt = new DataTable();
            dt.Columns.Add("siglaEstado");
            dt.Columns.Add("regiaoNome");
            dt.Columns.Add("nomeCidade");
            dt.Columns.Add("nomeMesorregiao");
            dt.Columns.Add("nomeFormatado");
            cidades.ForEach(city =>
            {
                string siglaEstado = city.microrregiao.mesorregiao.Estado.sigla;
                dt.Rows.Add(siglaEstado, city.microrregiao.mesorregiao.Estado.regiao.nome, city.nome,
                    city.microrregiao.mesorregiao.nome, city.nome + "/" + siglaEstado);
            });

            FileInfo fileInfoTemplate = new FileInfo("~/Template/Template.xlsx");
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            OfficeOpenXml.ExcelPackage excel = new ExcelPackage(fileInfoTemplate);

            ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add("teste");
            worksheet.Cells["A1"].LoadFromDataTable(dt, true);

            //salva arquivo
            Random aleatorio = new Random();
            string pastaTemp = "tabela" + aleatorio.Next(0, 100000000) + ".xlsx";

            FileInfo fileInfo = new FileInfo(pastaTemp);

            excel.SaveAs(fileInfo);

            return pastaTemp;

        }
        /// <summary>
        /// Armazena no banco de dados uma nova planilha para ser enviada 
        /// </summary>
        /// <param name="pla"></param>
        public void setPlanilha(PlanilhaJSON pla)
        {
            SqlConnection cone = Conexao.sqlConnection();
            SqlCommand insert = new SqlCommand("insert into PlanilhaHead(email,paraEnviar) values(@email,0); select SCOPE_IDENTITY ();", cone);
            insert.Parameters.AddWithValue("@email", pla.email);
            cone.Open();
            SqlDataReader read = insert.ExecuteReader();
            read.Read();
            int id = Convert.ToInt32(read[0]);
            read.Close();
            DataTable cidades = new DataTable();
            cidades.Columns.Add("idCidade");
            cidades.Columns.Add("idPlanilha");
            pla.cidades.ForEach(a =>
            {
                cidades.Rows.Add(a.id, id);
            });
            SqlBulkCopy copy = new SqlBulkCopy(cone);
            copy.DestinationTableName = "Planilha";
            copy.WriteToServer(cidades);
            SqlCommand update = new SqlCommand("update PlanilhaHead set paraEnviar = 1 where id = @id", cone);
            update.Parameters.AddWithValue("@id", id);
            update.ExecuteNonQuery();
            cone.Close();
        }
        public void setPlanilha(string email)
        {
            GetModels get = new GetModels();
            string file = getPlanilha(GetModels.GetCidades());
            EmailSend.enviarEmail(email, file);
            File.Delete(file);
        }
        /// <summary>
        /// Recupera todas as planilhas que não foram enviadas 
        /// e que estão prontas para serem enviadas 
        /// </summary>
        /// <returns></returns>
        public List<Planilha> PlanilhasNotSend()
        {
            List<Planilha> retorno = new List<Planilha>();
            SqlConnection cone = Conexao.sqlConnection();
            SqlCommand select = new SqlCommand("select * from PlanilhaHead where paraEnviar = 1 ", cone);
            cone.Open();
            SqlDataReader read = select.ExecuteReader();
            while (read.Read())
            {
                Planilha novo = new Planilha();
                novo.id = Convert.ToInt32(read["id"]);
                novo.email = Convert.ToString(read[0]);
                retorno.Add(novo);
            }
            cone.Close();
            return retorno;
        }
        /// <summary>
        /// Recupera do banco de dado um planilha a parti do ID 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Cidade> getPlanilha(int id)
        {
            List<Cidade> retorno = new List<Cidade>();
            SqlConnection cone = Conexao.sqlConnection();
            SqlCommand selet = new SqlCommand("select * from planilhaView where idPlanilha = @id", cone);
            selet.Parameters.AddWithValue("@id", id);
            cone.Open();
            SqlDataReader read = selet.ExecuteReader();
            while (read.Read())
            {
                Cidade nova = new Cidade();
                nova.id = Convert.ToInt32(read["idCidade"]);
                nova.nome = read["nome"].ToString();
                nova.microrregiao = new Cidade.Microrregiao()
                {
                    id = 0,
                    mesorregiao = new Mesorregiao()
                    {
                        nome = read["nomeMeso"].ToString(),
                        Estado = new Estado()
                        {
                            sigla = read["sigla"].ToString(),
                            nome = read["nomeEstado"].ToString(),
                            regiao = new Regiao()
                            {

                                id = Convert.ToInt32(read["idRegiao"]),
                                nome = read["nomeRegiao"].ToString(),
                                sigla = read["siglaRegiao"].ToString()
                            }
                        }
                    }
                };
                retorno.Add(nova);
            }
            cone.Close();
            return retorno;
        }
        /// <summary>
        /// Exclui do banco de dados um planilha a parti do ID 
        /// </summary>
        /// <param name="id"></param>
        public void deletePlanilha(int id)
        {
            SqlConnection cone = Conexao.sqlConnection();
            SqlCommand select = new SqlCommand("delete from Planilha where idPlanilha = @id; delete from PlanilhaHead where id = @id; ", cone);
            select.Parameters.AddWithValue("@id", id);
            cone.Open();
            select.ExecuteNonQuery();
            cone.Close();
        }

    }
}
