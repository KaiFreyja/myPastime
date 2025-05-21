
let Fgo = [
  {
    // 角色
    path: '/FgoRole',
    name: 'FgoRole',
    component: () => import('@/views/php/FgoRole.vue')
  },
  {
    // 特性
    path: '/FgoFeature',
    name: 'FgoFeature',
    component: () => import('@/views/php/FgoFeature.vue')
  },
  {
    // 職階
    path: '/FgoProfession',
    name: 'FgoProfession',
    component: () => import('@/views/php/FgoProfession.vue')
  },
  {
    // 地圖
    path: '/FgoMap',
    name: 'FgoMap',
    component: () => import('@/views/php/FgoMap.vue')
  },
  {
    // 技能
    path: '/FgoSkill',
    name: 'FgoSkill',
    component: () => import('@/views/php/FgoSkill.vue')
  },
  {
    // Buff
    path: '/FgoBuff',
    name: 'FgoBuff',
    component: () => import('@/views/php/FgoBuff.vue')
  },
  {
    // Buff
    path: '/FgoGachapon',
    name: 'FgoGachapon',
    component: () => import('@/views/php/FgoGachapon.vue')
  },
  /*
  {
    // 使用教學
    path: '/FgoSkill',
    name: 'FgoSkill',
    component: () => import('@/views/php/FgoSkill.vue')
  },
  {
    // 使用教學
    path: '/FgoMaterial',
    name: 'FgoMaterial',
    component: () => import('@/views/php/FgoMaterial.vue')
  },
*/
];
export default Fgo;
  