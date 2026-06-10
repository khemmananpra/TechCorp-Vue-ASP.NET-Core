<script setup lang="ts">
import Navbar from "./components/Navbar.vue";
</script>

<template>
  <div class="app">
    <Navbar />
    <div class="content">
      <router-view />
    </div>
  </div>
</template>

<style>
html,
body {
  margin: 0;
  padding: 0;
  height: 100%;
  width: 100%;
  background-color: #f5f6fa;
}

#app {
  height: 100%;
  width: 100%;
  display: flex;
  flex-direction: column;
}

.app {
  /* display: flex; */
  flex-direction: column;
  height: 100%;
}

.content {
  flex: 1;
  overflow: auto;
  background-color: #f5f6fa;
}
</style>
