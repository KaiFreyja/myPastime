<template>
  <RouterView />
</template>

<script>
import { RouterLink, RouterView,useRouter } from 'vue-router'
import { ref,onMounted } from 'vue'

export default {
  setup() {
    const router = useRouter();

    onMounted(() => {
      router.push("/FgoRole");
    })

    // 返回值会暴露给模板和其他的选项式 API 钩子
    return {

    }
  },

}
</script>
