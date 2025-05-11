<template>
  <div class="row">
    <button type="button" class="btn btn-primary col-sm-1" v-on:click="goToRole">角色</button>
        <button type="button" class="btn btn-primary col-sm-1" v-on:click="gotoProfession">職階</button>
    <button type="button" class="btn btn-primary col-sm-1" v-on:click="gotoFeature">屬性</button>
    <button type="button" class="btn btn-primary col-sm-1" v-on:click="gotoSkill">技能</button>
    <button type="button" class="btn btn-primary col-sm-1" v-on:click="gotoMaterial">素材</button>
        <button type="button" class="btn btn-primary col-sm-1" v-on:click="gotoMap">地圖</button>
  </div>
</template>

<script>
import { ref ,onMounted,watch} from 'vue';
//import { oltConsole, oAA , oApiError, oUrl, oAuth } from '@/lib/oltModule/initClientVue3';
import { useRouter, useRoute } from 'vue-router';
export default {
    
    props: {
  },
 components: {
  },
  setup(props) {
    const router = useRouter();
    function goToRole()
    {
      router.push('FgoRole');
    }

    function gotoProfession()
    {
      router.push('FgoProfession');
    }

    function gotoFeature()
    {
      router.push('FgoFeature');
    }

    function gotoSkill()
    {
      router.push('FgoSkill');
    }

    function gotoMaterial()
    {
      router.push('FgoMaterial');
    }

    function gotoMap()
    {
      router.push("FgoMap");
    }

    return {
      goToRole,
      gotoProfession,
      gotoFeature,
      gotoSkill,
      gotoMaterial,
      gotoMap,
    };
  },
};
</script>