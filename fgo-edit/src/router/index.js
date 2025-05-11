import { createRouter, createWebHashHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import Fgo from '@/router/php/Fgo';

const routes = [];
routes.push(...Fgo);

const router = createRouter({
  history: createWebHashHistory(), // 使用 Hash 模式（有 # 號）
  linkActiveClass: 'cur',
  linkExactActiveClass: 'cur',
  routes,
});

export default router;