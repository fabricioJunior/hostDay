<template>
  <div class="div">
    <form action="">
      <ul class="uls">
        <li class="lis">
          <div class="select">
            <select
              type="radio"
              placeholder="Busca Região"
              class="select_close"
              name="selectRegioes"
              id="slct"
              @change="regiaoSelect($event)"
            >
              <optgroup label="Regiões"></optgroup>
              <option :value="-1">Selecione uma Região</option>
              <option v-for="es in regioes" :key="es.id" :value="es.id">
                {{ es.nome }}
              </option>
            </select>
          </div>
        </li>
        <li class="lis">
          <div class="select">
            <select
              name="selectEstados"
              id="staties"
              class="box"
              @change="estadoSelect($event)"
              :disabled="!estadosEnable"
            >
              <optgroup label="Estados"></optgroup>
              <option :value="-1">Selecione um Estado</option>
              <option v-for="es in estados" :key="es.id" :value="es.sigla">
                {{ es.nome }}
              </option>
            </select>
          </div>
        </li>
        <li class="lis">
          <div class="select">
            <select
              name="selectMesoregioes"
              id="mesoRegioes"
              @change="mesorregiaoSelect($event)"
              :disabled="!mesoregioesEnable"
            >
              <optgroup label="Estados"></optgroup>
              <option :value="-1">Selecione uma Mesorregião</option>
              <option v-for="es in mesoregioes" :key="es.id" :value="es.id">
                {{ es.nome }}
              </option>
            </select>
          </div>
        </li>
        <li class="lis">
          <div class="select">
            <select
              name="selectCidades"
              id="cities"
              :disabled="!cidadesEnable"
              @change="cidadeSelect($event)"
            >
              <option :value="-1">Selecione uma Cidade</option>
              <optgroup label="Cidades"></optgroup>
              <option v-for="es in cidades" :key="es.id" :value="es.id">
                {{ es.nome }}
              </option>
            </select>
          </div>
        </li>
      </ul>
    </form>
  </div>
</template>

<script>
import axios from "axios";

export default {
  data() {
    return {
      regioes: [],
      estados: [],
      estadosEnable: false,
      mesoregioes: [],
      mesoregioesEnable: false,
      cidades: [],
      cidadesEnable: false,
      url: "https://localhost:44311",
    };
  },
  props: {},
  methods: {
    regiaoSelect: function (event) {
      if (event.target.value != -1) {
        var vm = this;

        let url =
          "https://servicodados.ibge.gov.br/api/v1/localidades/regioes/" +
          event.target.value +
          "/estados";
        let url2 =
          "https://servicodados.ibge.gov.br/api/v1/localidades/regioes/" +
          event.target.value +
          "/municipios";
        ///Atualiza os select box de estados
        axios
          .get(url)
          .then(function (response) {
            vm.estados = response.data;
            vm.estadosEnable = true;
            console.log(response.data);
          })
          .catch(function () {
            vm.$emit("apioff");
          })
          .then(function () {
            // always executed
          });
        ///Atualiza as cidades
        axios
          .get(url2)
          .then(function (response) {
            vm.cidades = response.data;
            console.log(response.data);
            vm.$emit("dadosAtualizados", { dados: vm.cidades });
          })
          .catch(function (error) {
            vm.$emit("apioff");
            console.log(error);
          })
          .then(function () {
            // always executed
          });
      } else {
        this.estadosEnable = false;
        this.mesoregioesEnable = false;
        this.cidadesEnable = false;
      }
    },
    estadoSelect: function (event) {
      if (event.target.value != -1) {
        let url =
          "https://servicodados.ibge.gov.br/api/v1/localidades/estados/" +
          event.target.value +
          "/mesorregioes";
        let url2 =
          "https://servicodados.ibge.gov.br/api/v1/localidades/estados/" +
          event.target.value +
          "/municipios";
        var vm = this;
        ///Atualiza o select box das mesorregiões

        axios
          .get(url)
          .then(function (response) {
            vm.mesoregioes = response.data;
            vm.mesoregioesEnable = true;
          })
          .catch(function (error) {
            // handle error
            console.log(error);
            vm.$emit("apioff");
          })
          .then(function () {
            // always executed
          });
        ///Atualiza as cidades
        axios
          .get(url2)
          .then(function (response) {
            vm.cidades = response.data;
            vm.$emit("dadosAtualizados", { dados: vm.cidades });
          })
          .catch(function (error) {
            vm.$emit("apioff");
            console.log(error);
          })
          .then(function () {});
      } else {
        this.mesoregioesEnable = false;
        this.cidadesEnable = false;
      }
    },
    mesorregiaoSelect: function (event) {
      if (event.target.value != -1) {
        let url =
          "https://servicodados.ibge.gov.br/api/v1/localidades/mesorregioes/" +
          event.target.value +
          "/municipios";
        let vm = this;
        axios
          .get(url)
          .then(function (response) {
            vm.cidades = response.data;
            vm.cidadesEnable = true;
            vm.$emit("dadosAtualizados", { dados: vm.cidades });
          })
          .catch(function (error) {
            // handle error
            vm.$emit("apioff");
            console.log(error);
          })
          .then(function () {
            // always executed
          });
      } else {
        this.cidadesEnable = false;
      }
    },
    cidadeSelect: function (event) {
      var temp = this.cidades.filter((a) => a.id == event.target.value);
      console.log(temp);
      this.cidades = temp;
      this.$emit("dadosAtualizados", { dados: temp });
    },
  },

  created: function () {
    var vm = this;

    axios
      .get("https://servicodados.ibge.gov.br/api/v1/localidades/regioes")
      .then(function (response) {
        vm.regioes = response.data;
      })
      .catch(function (error) {
        // handle error
        console.log(error);
        axios.get(vm.url + "/regiao").then(function (response) {
          vm.regioes = response.data;
          console.log(response.data);
        });
      })
      .then(function () {
        // always executed
      });
  },
};
</script>

<style >
label {
  font-family: monospace;
  font-size: 20px;
}
body {
  margin: 0;
  padding: 0;
  background-color: #004882;
}

.lis {
  display: inline-table;
}

.divs {
  width: 1000px;
}
select {
  -webkit-appearance: none;
  -moz-appearance: none;
  -ms-appearance: none;
  appearance: none;
  outline: 0;
  box-shadow: none;
  border: 0 !important;
  background: #2c3e50;
  background-image: none;
}
/* Remove IE arrow */
select::-ms-expand {
  display: none;
}
/* Custom Select */
.select {
  position: relative;
  display: flex;
  width: 20em;
  height: 3em;
  line-height: 3;
  background: #2c3e50;
  overflow: hidden;
  border-radius: 0.25em;
}
select {
  flex: 1;
  padding: 0 0.5em;
  color: #fff;
  cursor: pointer;
}
/* Arrow */
.select::after {
  content: "\25BC";
  position: absolute;
  top: 0;
  right: 0;
  padding: 0 1em;
  background: #34495e;
  cursor: pointer;
  pointer-events: none;
  -webkit-transition: 0.25s all ease;
  -o-transition: 0.25s all ease;
  transition: 0.25s all ease;
}
/* Transition */
.select:hover::after {
  color: #f39c12;
}
</style>