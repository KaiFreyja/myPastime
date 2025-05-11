//import './assets/main.css'

import { createApp } from 'vue'
import VueGoogleMaps from '@fawmi/vue-google-maps'
import App from './App.vue'
import router from './router'
import 'bootstrap/dist/css/bootstrap.min.css'
import 'bootstrap'

const app = createApp(App)

app.use(router)


app.use(VueGoogleMaps, {
  load: {
    key: 'AIzaSyDd-1GzyYWxQcL0RgTEVKlkz8zM9ld6da8', // 將 YOUR_API_KEY 替換成您的金鑰
  },
})

app.mount('#app')
