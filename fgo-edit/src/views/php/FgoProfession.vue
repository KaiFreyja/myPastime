<template>
    <FgoTop/>
    <h1>職階</h1>
  <table class="table table-bordered">
    <thead>
      <tr>
        <th>攻擊方\守備方</th>
        <th v-for="(profession,index) in professionArray">{{profession.name}}</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="(profession,index) in professionArray">
        <th>{{profession.name}}</th>
        <td v-for="(def_profession,index) in professionArray">           
            <input type="number" min="0" max="1000" step="10" v-model="rateMatrix[`${profession.pid}_${def_profession.pid}`]"></input>            
        </td>
      </tr>
    </tbody>
  </table>


  <button type="button" class="btn btn-primary" v-on:click="clickSave">儲存</button>


</template>

<script>
/* eslint-disable import/no-absolute-path */
// eslint-disable-next-line import/no-unresolved
import { config } from '@/include/config';
import { h, ref, reactive , watch , inject , onMounted , onUpdated} from 'vue';
//import { oltConsole, oAA , oApiError, oUrl, oAuth } from '@/lib/oltModule/initClientVue3';
//import { initOltClientFrame } from '@/lib/initOltClientFrame';
import { useI18n } from 'vue-i18n';
import { useRouter, useRoute } from 'vue-router';
import { useStore } from 'vuex';
import { Button } from 'bootstrap';
import {getProfession,saveProfessionAtkRate,getProfessionAtkRate} from '@/APIController';
import { FgoTop } from '@/components/FgoComponents' ;

export default {
  components: {
    FgoTop,
  },
  setup() {
    const router = useRouter();
    const route = useRoute();
    const store = useStore();
 
    const professionArray = ref([]);
    const professionAtkRateArray = ref([]);

    const rateMatrix = reactive({});

    /*
    watch(pageSearchName, (newValue, oldValue) => {
      //isLoading.value = true; 
      console.log('The new value is: ' + pageSearchName.value);
      pageSearchName.value = newValue ; 
      drawPageRecord();
    }) ;
    */

    /**
     *  Record Page End
     */ 

    onMounted(async () => {
        //professionAtkRateArray.value = [{"parid":1,"atk_pid":1,"def_pid":1,"rate":120}];
        
        getProfession({},(result)=>
        {
            professionArray.value = result.profession.data;
            getProfessionAtkRate({},(result)=>
            {
                professionAtkRateArray.value = result.profession_atk_rate.data;
            
                for (let atk of professionArray.value) {
                    for (let def of professionArray.value) {
                        var rate = 100;
                        for(let data of professionAtkRateArray.value)
                        {
                            if(data.atk_pid == atk.pid && data.def_pid == def.pid)
                            {
                                rate = data.rate;
                                break; 
                            }
                        }
                        const key = `${atk.pid}_${def.pid}`;
                        rateMatrix[key] = rate; // default value
                    }
                }
            });
        });
        //

    });

    onUpdated( async () => {



    }) ;

    function clickSave()
    {
        console.log("save");
        const saveArray = [];

        for (const key in rateMatrix) {
            const [atk_pid, def_pid] = key.split('_').map(Number);
            const rate = rateMatrix[key];
            saveArray.push({ atk_pid, def_pid, rate });
        }
        // 印出來看一下
        console.log("要儲存的資料:", saveArray);

        var input = {"data":saveArray};
        saveProfessionAtkRate(input,(result)=>
        {
            
        });
    }

    return {
        professionArray,
        professionAtkRateArray,
        rateMatrix,
        clickSave,
    };
  },
};
</script>