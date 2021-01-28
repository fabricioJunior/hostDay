<template >
  <body id="bg">
    <div id="app">
      <Busca id = "sourceComponent" class = "busca" v-show="api" @apioff = "circuitBreak" @dadosAtualizados = "updatecidades($event)"/>
    
      <div>

        
      </div>  
  
      <CidadesTable class = "busca"  :cities = "cidades"/>
         
       <form action="">
             <div id = "send">
             <label for="email" >Digite seu email: </label>
             <input type="email"  v-model="email"  > 
             <input type="submit" @click="sendDados">
        </div>

       </form>
          
        
       
     
      
    </div>
  </body>
</template>

<script>
import CidadesTable from "./components/CidadesTable";
import Busca from "./components/Busca";
import axios from 'axios';


function format(dados){
  let retorno = [];
  dados.forEach(element => {
     retorno.push(
        {
          id: element.id,
          siglaEstado: element.microrregiao.mesorregiao.UF.sigla,
          regiaoNome:element.microrregiao.mesorregiao.UF.regiao.nome,
          nomeCidade:element.nome,
          nomeMesorregiao: element.microrregiao.mesorregiao.nome
        }
     );
  });
  return retorno;
}
export default {
  name: "App",
  components: {
    CidadesTable,
    Busca
    
  },
  data(){
     return{ 
       cidades:[ 
       
       ],email:"",
       url:"https://localhost:44373/",
       api: true
     }
  },
  methods:{
     updatecidades:function(event){
        this.cidades = format(event.dados);
     },
     sendDados:function(){
         console.log( {
          cidades:this.cidades,
          email:this.email
         });
         alert("Enviamos a planilha para o seu email :)");
         axios.post(this.url + "export",
         {  
           
          cidades:this.cidades,
          email:this.email
         }
         
         );
      
     }, circuitBreak:function(){
         if(this.api){
            alert("Infelizmente os serviços de dados do IBGE estão indisponíveis no momento. Tal situação deixa indisponíveis os filtros.");
         }
         this.api = false;

      }
}
}
</script>


<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  text-align: center;
 
  margin: 0;
  margin-block: 5px; 
}

*{
    box-sizing: border-box;
    -webkit-box-sizing: border-box;
    -moz-box-sizing: border-box;
}

body{
    font-family: Helvetica;
    -webkit-font-smoothing: antialiased;
    background: rgba( 71, 147, 227, 1);
}
</style>
