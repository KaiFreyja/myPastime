<template>
  <FgoTop/>
  
  <h1>轉蛋</h1> 

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
import {getProfession,getGender,getRole,AddNewRole,modifyRole,getOneRole} from '@/APIController';
import { LevelAttrEdit,RoleImageEdit,FeatureEdit } from '@/components/FgoRole' ;
import { FgoTop } from '@/components/FgoComponents' ;

export default {
  components: {
    FgoTop,
  },
  setup() {

    /**
     *  swal start
     */
    const swal = inject('$swal') ;
    /**
     *  swal end 
     */

    /**
     *  i18n Start 
     */
    /*
    const { t, locale } = useI18n({
      useScope:'global' // 必需設定 SCOPE 才能覆蓋設定
    }); 

    locale.value = 'zh-TW';
    */
    /**
     *  i18n end 
     */

    const router = useRouter();
    const route = useRoute();
    const store = useStore();
 
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
        
    });

    onUpdated( async () => {



    }) ;

    return {
    };
  },
};
</script>
