<template>
  <div>
    <div class="slider-container">
      <div v-for="(slider, index) in sliders" :key="index" class="vertical-slider">
        <input
          type="range"
          min="0"
          max="2000"
          :value="slider.value"
          orient="vertical"
          @input="updateSliderValue(index, $event.target.value)"
        />
        
      </div>
    </div>
  </div>
</template>

<script>
import { ref ,onMounted,watch} from 'vue';

export default {
    
    props: {
    sliderData: {
      type: Array,
      required: false,
      // validator(value) {
      //   return value.every(item => 'label' in item && 'value' in item);
      // }
    }
  },
 components: {
  },
  setup(props) {
    var sliders = ref([]);
 
    
    watch(
      () => props.sliderData,
      (newSliderData) => {
        sliders.value = [...newSliderData]; // 使用傳入的資料更新 local 的 sliders
      },
      { deep: true, immediate: true }
    );
    
/*
    onMounted(async () => {

        var array = [];
        for(var i = 0;i < 120;i++)
       {
            array[i] = { label : i ,value : i * 10 };
       }
       sliders.value = array;
 

   });
*/
   const updateSliderValue = (index, newValue) => {
  const newSliders = [...sliders.value]; // 創建 sliders.value 的淺拷貝
  newSliders[index].value = parseInt(newValue);

  const count = newSliders.length;

  // 向前調
  if (index > 0) {
    for (let i = index; i > 0; i--) {
      const nowValue = newSliders[i].value;
      const preValue = newSliders[i - 1].value;
      if (nowValue < preValue) {
        newSliders[i - 1].value = Math.max(0, nowValue - 10); // 確保不小於 0
      }
    }
  }

  // 向後調
  if (index < count - 1) {
    for (let i = index; i < count - 1; i++) {
      const nowValue = newSliders[i].value;
      const nextValue = newSliders[i + 1].value;
      if (nowValue > nextValue) {
        newSliders[i + 1].value = Math.min(2000, nowValue + 10); // 確保不大於 2000
      }
    }
  }

  sliders.value = newSliders; // 將修改後的陣列賦值回去
};

    return {
      sliders,
      updateSliderValue,
    };
  },
};
</script>

<style scoped>
.slider-container {
  display: flex;
  gap: 1px; /* 調整 slider 之間的間距 */
}

.vertical-slider {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.vertical-slider label {
  margin-bottom: 5px;
}

.vertical-slider input[type="range"] {
  width: 10px; /* 調整 slider 的寬度 */
  height: 200px; /* 調整 slider 的高度 */
  -webkit-appearance: slider-vertical; /* Safari 和 Chrome */
  writing-mode: bt-lr; /* Internet Explorer */
}
</style>