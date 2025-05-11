<template>
    <div>屬性</div>
</template>

<script>
import { ref ,onMounted,watch} from 'vue';
import {} from '@/APIController';

export default {
  props:{
    rid : String,
  },
 components: {

  },
  setup(props) {

    watch(
      () => props.rid,
      (newRid) => {
        
      },
      { deep: true, immediate: true }
    );

    return {

    };
  },
};
</script>