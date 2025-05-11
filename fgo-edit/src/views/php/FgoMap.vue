<template>
    <FgoTop/>
    <h1>地圖</h1>
    
  <div>
    <GMapMap :center="{ lat: 25.033671, lng: 121.564427 }" :zoom="15" map-type-id="roadmap" style="width: 100%; height: 90vh;" @click="handleMapClick">
        <GMapMarker v-for="(marker,index) in markers" :key="marker.mpid" :position="{lat:parseFloat(marker.lat), lng:parseFloat(marker.lng)}" :label="'標記' + (index)" @click="selectMarker(index)"/>
    
        <GMapInfoWindow
        v-if="selectedMarkerIndex != null && markers[selectedMarkerIndex]"
        :key="selectedMarkerIndex" 
        :position="{lat:parseFloat(markers[selectedMarkerIndex].lat), lng:parseFloat(markers[selectedMarkerIndex].lng)}"
        @closeclick="selectedMarkerIndex = null">    
        <div>
            <p>標記 {{ selectedMarkerIndex + 1 }}</p>
            <button @click="removeMarker(selectedMarkerIndex)">移除</button>
        </div>
        </GMapInfoWindow>
    
</GMapMap>

  </div>

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
import { getMapPos,addMapPos,delMapPos} from '@/APIController';
import { FgoTop } from '@/components/FgoComponents' ;
import VueGoogleMaps from '@fawmi/vue-google-maps'

export default {
  components: {
    FgoTop,
  },
  setup() {
    const router = useRouter();
    const route = useRoute();
    const store = useStore();

    const markers = ref([]);
    const selectedMarkerIndex = ref(null);
 
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
       getAllMapPos();
    });

    onUpdated( async () => {



    }) ;

    function getAllMapPos()
    {
        markers.value = [];
        getMapPos({},(result)=>
        {
            var data = result.map_pos.data;
            markers.value = data;
        });
    }

// 點擊地圖時觸發
    const handleMapClick = (event) => {
      const lat = event.latLng.lat();
      const lng = event.latLng.lng();
      //markerPosition.value = { lat, lng };
      console.log('點擊位置：', lat, lng);
      //markers.value.push({lat,lng});

        addMapPos({"lat":lat,"lng":lng},(result)=>
        {
            getAllMapPos();
        });
    };

    const removeMarker = (index) => {
        var data = markers.value[index];
        markers.value.splice(index, 1);
        selectedMarkerIndex.value = null;
        delMapPos(data,(result)=>
        {
            getAllMapPos();
        });

    };

    const selectMarker = (index) => {
    if (selectedMarkerIndex.value === index) {
        // 強制觸發 InfoWindow 更新
        selectedMarkerIndex.value = null
        nextTick(() => selectedMarkerIndex.value = index)
    } else {
        selectedMarkerIndex.value = index
    }

        console.log("index : " + index + "->" + markers.value[index].lat + "," + markers.value[index].lng);
    };

    return {
        selectedMarkerIndex,
        markers,
        selectMarker,
        handleMapClick,
        removeMarker,
        getAllMapPos,
    };
  },
};
</script>