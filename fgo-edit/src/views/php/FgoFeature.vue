<template>
    <FgoTop/>
    <h1>屬性</h1>
    <div class="row">
    <div class="dropdown">
    <button class="btn btn-primary dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
        <div v-if="featureArray.length == 0">請選擇</div>
        <div v-else>{{ featureArray[selectFeatureIndex].name }}</div>
    <span class="caret"></span></button>
    <ul class="dropdown-menu">
        <li v-for="(feature,index) in featureArray"><a class="dropdown-item" v-on:click="clickDrapFeature(index)">{{ feature.name}}</a></li>
    </ul>
    </div>

    <button type="button" class="btn btn-primary" v-on:click="clickAddNewFeature">新增屬性</button>

    </div>

    <div class="form-group">
    <label for="name">Name:</label>
    <input type="text" class="form-control" id="name" v-model="name">
    </div>

    <div class="form-group">
    <label for="description">Description:</label>
    <input type="text" class="form-control" id="description" v-model="description">
    </div>
    <button type="button" class="btn btn-primary" v-on:click="clickModifyFeature">修改</button>
 

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
import {getFeature,addNewFeature,mmodifyFeature} from '@/APIController';
import { FgoTop } from '@/components/FgoComponents' ;

export default {
  components: {
    FgoTop,
  },
  setup() {
    const router = useRouter();
    const route = useRoute();
    const store = useStore();
 
    const featureArray = ref([]);
    const selectFeatureIndex = ref(0);
 
    const fid = ref(0);
    const name = ref("");
    const description = ref("");
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
        getFeature({},(result)=>
        {
           featureArray.value = result.feature.data;         
        })
    });

    onUpdated( async () => {

    }) ;


     function clickDrapFeature(index)
     {
        selectFeatureIndex.value = index;
        var data = featureArray.value[index];
        fid.value = data.fid;
        name.value = data.name;
        description.value = data.description;
     }
     function clickAddNewFeature()
     {
        addNewFeature({"name":"newFeature","description":"newFeature description"},(result)=>
        {
            console.log("clickAddNewRole");
            getFeature({},(result)=>
            {
                featureArray.value = result.feature.data;
            });
        });
     }
     function clickModifyFeature()
     {
      var input = {"fid":fid.value , "name" : name.value , "description" : description.value};

        mmodifyFeature(input,(result)=>
        {
            getFeature({},(result)=>{
                featureArray.value = result.feature.data;
            })
        });
     }

    return {
        featureArray,
        selectFeatureIndex,
        fid,
        name,
        description,
        clickDrapFeature,
        clickAddNewFeature,
        clickModifyFeature,
    };
  },
};
</script>