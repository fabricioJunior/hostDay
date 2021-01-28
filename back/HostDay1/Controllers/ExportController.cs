using hostDay.Models;




using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using hostDay.JSON;
using hostDay.Planilhas;
using hostDay.Email;

namespace hostDay.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExportController : ControllerBase
    {
        PlanilhaFactory factory = new PlanilhaFactory();
        /// <summary>
        /// Recebe uma nova planilha para ser enviada para o email
        /// </summary>
        /// <param name="planilha"></param>
        [HttpPost]
        public void Post(PlanilhaJSON planilha)
        {
           
            factory.setPlanilha(planilha);
        }
        [HttpPost]
        [Route("{email}")]
        public void PostEmail(string email)
        {
            
            factory.setPlanilha(email);
        }
    }
}
