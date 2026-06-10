<template>
  <!-- Department Section -->
  <h3 class="section-title">Department</h3>
  <div class="department-dropdown">
    <select v-model="employee.departmentid" class="form-select">
      <option disabled value="">-- Select Department --</option>
      <option v-for="dep in departments" :key="dep.id" :value="dep.id">
        {{ dep.name }}
      </option>
    </select>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, reactive } from "vue";
import { getDepartments } from "@/service/department.service";
import { EmployeeCreate } from "@/models/Employee";

const props = defineProps<{
  employee: EmployeeCreate;
}>();

const departments = ref<{ id: number; name: string }[]>([]);

onMounted(async () => {
  try {
    const dep = await getDepartments();
    departments.value = dep;
  } catch (err) {
    console.error(err);
  }
});
</script>


<style scoped>

.department-section {
  background: #d9d9d9;
  border-radius: 16px;
  padding: 1.5rem;
  box-shadow: 0 10px 25px rgba(0, 0, 0, 0.1);
  border: 1px solid rgba(255, 255, 255, 0.2);
  backdrop-filter: blur(10px);
  height: fit-content;

  margin-top: 2rem;
}

.section-title {
  font-weight: 400;
  color: #2d3748;
}

.department-dropdown {
  position: relative;
}

.form-select {
  width: 100%;
  padding: 0.75rem 1rem;
  border: 2px solid #e2e8f0;

  font-size: 1rem;
  background: white;
  color: #2d3748;
  transition: all 0.3s ease;
  appearance: none;
  background-image: url("data:image/svg+xml,%3csvg xmlns='http://www.w3.org/2000/svg' fill='none' viewBox='0 0 20 20'%3e%3cpath stroke='%236b7280' stroke-linecap='round' stroke-linejoin='round' stroke-width='1.5' d='M6 8l4 4 4-4'/%3e%3c/svg%3e");
  background-position: right 0.75rem center;
  background-repeat: no-repeat;
  background-size: 1.5em 1.5em;
  padding-right: 3rem;
}

.form-select:focus {
  outline: none;
  border-color: #4299e1;
  box-shadow: 0 0 0 3px rgba(66, 153, 225, 0.1);
}

</style>