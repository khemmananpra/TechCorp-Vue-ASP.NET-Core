<template>
  <nav class="navbar">
    <!-- กลุ่ม logo + subtitle -->
    <div class="logo-group">
      <div class="logo">TechCorp Co.,Ltd</div>
      <div class="subtitle">View and manage employee</div>
    </div>

    <!-- เมนู navigation -->
    <div class="search-box">
      <input type="text" placeholder="Search employees..." />
    </div>
  </nav>
</template>

<script setup lang="ts"></script>

<style scoped>
.navbar {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem 2rem;
  background-color: #ffffff;
  color: rgb(0, 0, 0);
  border-bottom: 2px solid #bdbaba;
  
}

/* stack logo + subtitle แนวตั้ง */
.logo-group {
  display: flex;
  flex-direction: column;
}

.logo {
  font-size: 1.5rem;
  font-weight: 700;
}

.subtitle {
  font-size: 0.9rem;
  color: #4a5567;
  margin-top: 2px;
}

/* menu links */
.nav-links {
  list-style: none;
  display: flex;
  gap: 1rem;
}

.nav-links a {
  color: white;
  text-decoration: none;
  font-weight: 500;
}

.nav-links a.router-link-active {
  border-bottom: 2px solid #fff;
}

.search-box input {
  padding: 6px 12px;
  border-radius: 16px;
  border: 1px solid #ccc;
  outline: none;
  width: 220px;
  font-size: 0.95rem;
}

/* optional: focus effect */
.search-box input:focus {
  box-shadow: 0 0 0 2px rgba(94, 114, 228, 0.4);
}
</style>
