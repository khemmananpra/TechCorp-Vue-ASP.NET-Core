import { createRouter, createWebHistory } from 'vue-router'
import Home from '@/components/Index.vue'
import Create from '@/components/Create.vue'
import Edit from '@/components/Edit.vue'


const routes = [
  { path: '/', component: Home },
  { path: '/create', component: Create },
  { path: '/edit/:id', component: Edit },
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
