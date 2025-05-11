<template>
  <FgoTop/>
  

  <h1>角色</h1> 
<div class="row">
<div class="dropdown">
  <button class="btn btn-primary dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
    <div v-if="roleArray.length == 0">請選擇</div>
    <div v-else>{{ roleArray[selectRoleIndex].name }}</div>
  <span class="caret"></span></button>
  <ul class="dropdown-menu">
    <li v-for="(role,index) in roleArray"><a class="dropdown-item" v-on:click="clickDrapRole(index)">{{ role.name}}</a></li>
  </ul>
</div>

  <button type="button" class="btn btn-primary" v-on:click="clickAddNewRole">新增使用者</button>

</div>

<div class="form-group">
  <label for="name">Name:</label>
  <input type="text" class="form-control" id="name" v-model="name">
</div>

<div class="form-group">
  <label for="description">Description:</label>
  <input type="text" class="form-control" id="description" v-model="description">
</div>

<div class = "row">

<div class="row">

  <div class="col-sm-1">
<div>性別</div>
<div class="dropdown">
  <button class="btn btn-primary dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
    <div v-if="genderArray.length == 0">請選擇</div>
    <div v-else>{{ genderArray[selectGenderIndex].name }}</div>
  <span class="caret"></span></button>
  <ul class="dropdown-menu">
    <li v-for="(gender,index) in genderArray"><a class="dropdown-item" v-on:click="clickDrapGender(index)">{{ gender.name}}</a></li>
  </ul>
</div>
</div>

<div class="col-sm-1">
<div>職階</div>
<div class="dropdown">
  <button class="btn btn-primary dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
    <div v-if="professionArray.length == 0">請選擇</div>
    <div v-else>{{ professionArray[selectProfessIndex].name }}</div>
  <span class="caret"></span></button>
  <ul class="dropdown-menu">
    <li v-for="(profession,index) in professionArray"><a class="dropdown-item" v-on:click="clickDrapProfession(index)">{{ profession.name}}</a></li>
  </ul>
</div>
</div>
</div>

</div>

  <button type="button" class="btn btn-primary" v-on:click="clickModifyRole">修改使用者</button>
  
  <RoleImageEdit :rid="rid"/>
  <FeatureEdit :rid="rid"/>
  <LevelAttrEdit :rid="rid"/>


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
    LevelAttrEdit,
    RoleImageEdit,
    FeatureEdit,
  },
  setup() {
    const roleArray = ref([]);
    const professionArray = ref([]);
    const genderArray = ref([]);
    const selectRoleIndex = ref(0);
    const selectProfessIndex = ref(0);
    const selectGenderIndex = ref(0);

    const roleData = ref();
    const rid = ref("");
    const name = ref("");
    const description = ref("");

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
        getProfession({},(result)=>
        {
            professionArray.value = result.profession.data;
        });

        getGender({},(result)=>
        {
           genderArray.value = result.gender.data; 
        });

        getRole({},(result)=>
        {
            roleArray.value = result.role.data;
        });
    });

    onUpdated( async () => {



    }) ;

    function clickDrapRole(index)
    {
        selectRoleIndex.value = index;
        var data = roleArray.value[index];
        rid.value = data.rid;
        LoadRoleData();
    }

    function LoadRoleData()
    {    
      var input = {"rid":rid.value};
      getOneRole(input,(result)=>{
          var data = result.role;
          roleData.value = data;
          name.value = data.name;
          rid.value = data.rid;
          description.value = data.description;
          var pid = data.pid;        
          var gid = data.gid;
          var array = genderArray.value;
          for(var i = 0;i < array.length;i++)
          {
              if(array[i].gid == gid)
              {
                selectGenderIndex.value = i;
              }
          }

          var array = professionArray.value;
          for(var i = 0;i < array.length;i++)
          {
              if(array[i].pid == pid)
              {
                selectProfessIndex.value = i;
              }
          }
        });
    }

    function clickDrapGender(index)
    {
        selectGenderIndex.value = index;
    }

    function clickDrapProfession(index)
    {
        selectProfessIndex.value = index;
    }

    function clickAddNewRole()
    {
      AddNewRole({"name":"newRole","description":"newRole description"},(result)=>
      {
        console.log("clickAddNewRole");
        getRole({},(result)=>
        {
            roleArray.value = result.role.data;
        });
      });
    }

    function clickModifyRole()
    { 
      var gender = genderArray.value[selectGenderIndex.value];
      var profession = professionArray.value[selectProfessIndex.value];
       
      var input = {"rid":roleData.value.rid,"gid" : gender.gid, "pid" : profession.pid , "name" : name.value , "description" : description.value};

      modifyRole(input,(result)=>
      {
        getRole({},(result)=>{
          roleArray.value = result.role.data;
          LoadRoleData(roleData.value.rid);
        })
      });
    }

    return {
      // i18n
     // t,
      //locale,      
      // record page 
      // const data
      //token:null,

      //
      // function 
      /*
      gotoPage,
      clickItem,
      clickMore,
      clickChoppingCart,
      */

        rid,
        name,
        description,

        roleArray,
        professionArray,
        genderArray,
        selectGenderIndex,
        selectProfessIndex,
        selectRoleIndex,

        clickDrapRole,
        clickDrapGender,
        clickDrapProfession,
        clickAddNewRole,
        clickModifyRole,

        LoadRoleData,
    };
  },
};
</script>
