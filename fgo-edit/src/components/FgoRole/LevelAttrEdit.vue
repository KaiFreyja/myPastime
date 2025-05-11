<template>
  <div>
    <div>等級HP</div>
    <NumValueEdit ref="hpEditRef" :sliderData="hpSliders"/>
    <div>等級ATK</div>
    <NumValueEdit ref="atkEditRef" :sliderData="atkSliders"/>
  </div>

  <button type="button" class="btn btn-primary" v-on:click="clickModifyLevelAttr">儲存數值</button>

</template>

<script>
import { ref ,onMounted,watch} from 'vue';
import { NumValueEdit } from '@/components/FgoRole';
import {getRoleLevelAttr,saveOneRoleLevelAttr} from '@/APIController';

export default {
  props:{
    rid : String,
  },
 components: {
    NumValueEdit
  },
  setup(props) {
    const hpSliders = ref([]);
    const atkSliders = ref([]);

    const valueTemp = ref([]);
    const hpEditRef = ref(null); // 建立 hpEdit 的 ref 物件
    const atkEditRef = ref(null); // 建立 atkEdit 的 ref 物件

    onMounted(async () => {
      console.log(props.rid);
        var array = [];
        for(var i = 0;i< 120;i++)
        {
            array[i] = {"level":(i + 1),"hp" : i * 10, "atk" : i *  10};
        }      
        getShowData(array);
    });

    watch(
      () => props.rid,
      (newRid) => {
        settingRole(newRid);
      },
      { deep: true, immediate: true }
    );

    function getShowData(array)
    {
        hpSliders.value = [];
        atkSliders.value = [];

        for(var i = 0;i < array.length;i++)
        {
            var data = array[i];
            hpSliders.value[i] = {"value" : data.hp};
            atkSliders.value[i] = {"value" : data.atk};
        }
    }

    function settingRole(rid)
    { 
      getRoleLevelAttr({"rid":rid},(result)=>
      {
          var data = result.level_attr.data;
          valueTemp.value = data;
          getShowData(data);
      })
    }

    function clickModifyLevelAttr()
    {
        var hpRef = hpEditRef.value.sliderData;
        var atkRef = atkEditRef.value.sliderData;

        var array = valueTemp.value;
        for(var i = 0;i < valueTemp.value.length;i++)
        {
            array[i].hp = hpRef[i].value;
            array[i].atk = atkRef[i].value;
        }
        console.log(JSON.stringify(array));

        for(var i = 0;i < array.length;i++)
        {
          var data = array[i];
          saveOneRoleLevelAttr(data,(result)=>
          {

          })
        }
        /*
        var input = {"level_attr":array};
        saveRoleLevelAttr(input,()=>
        {
            console.log("save end");
        });*/
        }

    return {

        valueTemp,

        hpEditRef, // 將 ref 物件暴露給模板
        atkEditRef, // 將 ref 物件暴露給模板

        hpSliders,
        atkSliders,
        getShowData,
        settingRole,
        clickModifyLevelAttr,

    };
  },
};
</script>