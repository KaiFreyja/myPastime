<template>
  <div>

    <div>==============================================================</div>
    <div style="display: flex">
        <div v-for="item in images">
            <img :src="item.url" alt="圖片預覽" style="max-width: 300px;">  
        </div>
    </div>

    
    <input type="file" @change="handleFileUpload">
    <div v-if="selectedFile">
      已選擇檔案：{{ selectedFile.name }} ({{ formatFileSize(selectedFile.size) }})
    </div>

    <div v-if="previewUrl">
      預覽：
      <img :src="previewUrl" alt="圖片預覽" style="max-width: 300px;">
    </div>
      <button type="button" class="btn btn-primary" v-on:click="clickSaveImage">上傳圖片</button>

  </div>
</template>

<script>
import { ref ,onMounted,watch, provide} from 'vue';
import {saveRoleImage,getRoleResourceImage} from '@/APIController';

export default {
  props:{
    rid : String,
  },
 components: {

  },
  setup(props) {

    const nowRid = ref("");
    const selectedFile = ref(null);
    const previewUrl = ref(null);
    const images = ref([]);


    onMounted(async () => {
    });

    watch(
      () => props.rid,
      (newRid) => {
        nowRid.value = newRid;
        selectedFile.value = null;
        var input = {"rid":newRid};
        getRoleResourceImage(input,(result)=>
        {
            images.value = result.role_resource.data;
        });


      },
      { deep: true, immediate: true }
    );

    const handleFileUpload = (event) => {
    selectedFile.value = event.target.files[0];
    LoadImage();
    };

    const formatFileSize = (bytes) => {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
    };

    function LoadImage()
    {
        previewUrl.value = null; // 清空之前的預覽

        if (selectedFile.value) {
            const reader = new FileReader();

        reader.onload = (e) => {
        previewUrl.value = e.target.result; // 將讀取到的 Data URL 賦值給 previewUrl
    };

    reader.readAsDataURL(selectedFile.value); // 開始讀取檔案為 Data URL
  }
    }

    function clickSaveImage()
    {
      
      console.log("rid : " + nowRid.value);
      //console.log("selectedFile : " + previewUrl.value);
      
      
        var rid = nowRid.value;
        if(rid == "" || selectedFile.value == "")
        {
          console.log("無法上傳圖片");
          return;
        }
        var input = {"rid":rid,"type":"role_image"};
        saveRoleImage(input,selectedFile.value,(result)=>
        {
            previewUrl.value = null; 
            selectedFile.value = null;
            getRoleResourceImage(input,(result)=>
            {
                images.value = result.role_resource.data;
            });
        });
    }

    return {
        nowRid,
        selectedFile,
        previewUrl,
        
        LoadImage,
        handleFileUpload,
        formatFileSize,
        clickSaveImage,
        images,
    };
  },
};
</script>