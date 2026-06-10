<template>
  <!-- <div :key="componentKey"> -->
  <div class="employee-card">
    <div class="">
      <div class="subordinates-header">
        <p></p>
        <span class="subordinates-count">{{ employee.inverseManager.length }}</span>
      </div>
      <div class="form-group">
        <label class="form-label">Full Name</label>
        <input v-model="employee.name" placeholder="Enter full name" class="form-control" />
      </div>

      <!-- <div class="form-group">
        <label class="form-label">Position</label>
        <input
          v-model="employee.position.title"
          placeholder="Enter position title"
          class="form-control"
        />
      </div> -->

      <div class="form-group">
        <label class="form-label">Position</label>
        <select v-model="employee.positionid" class="form-control">
          <option disabled value="">-- Select Position --</option>
          <option v-for="pos in positions" :key="pos.id" :value="pos.id">
            {{ pos.title }}
          </option>
        </select>
      </div>

      <div class="form-group">
        <label class="form-label">Profile Image</label>
        <div class="file-upload-wrapper">
          <input
            type="file"
            @change="(e) => onFileChange(e, employee)"
            class="file-input"
            accept="image/*"
            :id="'upload-file-' + path"
          />
          <label :for="'upload-file-' + path" class="file-upload-btn">
            <svg class="upload-icon" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M7 16a4 4 0 01-.88-7.903A5 5 0 1115.9 6L16 6a5 5 0 011 9.9M15 13l-3-3m0 0l-3 3m3-3v12"
              />
            </svg>
            {{ employee.CoverPhotoFile?.name || "Choose File" }}
          </label>

          <div v-if="employee.profileImage" class="preview-container mb-3">
            <p>Preview:</p>
            <img :src="employee.profileImage" alt="Preview" class="preview-img" />
          </div>
        </div>
      </div>
    </div>

    <div class="">
      <!-- Department 0fr -->
    </div>

    <div class="action-buttons">
      <button type="button" @click="addSubordinate" class="btn btn-add">+ Add Subordinate</button>

      <button
        v-if="parent"
        type="button"
        @click="removeEmployee(props.employee)"
        class="btn btn-remove"
      >
        - Remove
      </button>
    </div>

    <div class="">
      <!-- Department 0fr -->
    </div>
  </div>
  <div v-if="employee.inverseManager?.length" class="subordinates">
    <p>-------</p>
    <div class="subordinates-header">
      <h4 class="subordinates-title">Subordinates</h4>
    </div>
    <div class="subordinates-list">
      <Employeeform
        v-for="(sub, index) in employee.inverseManager.filter((emp) => !emp.isdeleted)"
        :key="index"
        :employee="sub"
        :parent="employee"
        :path="(path || '') + '-' + index"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
import Employeeform from "./Employeeform.vue";
import { EmployeeCreate, type Department, type Position } from "@/models/Employee";
import { getPositions } from "@/service/position.service";

// const componentKey = ref(0);

//มันต้อง define รับ class ไม่งั้นมันจะไม่รู้ว่า employee คืออะไร มันส่งมาจากตัวนี้ <Employeeform :employee="rootEmployee" />
const props = defineProps<{
  employee: EmployeeCreate;
  parent?: EmployeeCreate;
  path?: string;
}>();

console.log(props.employee);

function addSubordinate() {
  if (!props.employee.inverseManager) {
    props.employee.inverseManager = [];
  }

  props.employee.inverseManager.push(new EmployeeCreate());
  // componentKey.value += 1; // Update key to force re-render
}

function removeEmployee(employee: EmployeeCreate) {
  if (!employee) return;

  employee.isdeleted = true;
  employee.CoverPhotoFile = undefined;

  if (employee.name == null) {
    employee.name = "Deleted";
  }

  if (employee.inverseManager?.length > 0) {
    employee.inverseManager.forEach((child) => removeEmployee(child));
  }
}

function onFileChange(e: Event, employee: EmployeeCreate) {
  const file = (e.target as HTMLInputElement).files?.[0];
  if (file) {
    employee.CoverPhotoFile = file;
    // Create preview
    const reader = new FileReader();
    reader.onload = (e) => {
      employee.profileImage = e.target?.result as string;
    };

    reader.readAsDataURL(file);
  }
}

const positions = ref<Position[]>([]);
onMounted(async () => {
  //Department
  try {
    const pos = await getPositions();
    positions.value = pos;
  } catch (error) {
    console.error("Failed to load employees", error);
  }
});
</script>

<style scoped>
.employee-card {
  background: #d9d9d9;
  border-radius: 16px;
  padding: 1.5rem;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
  border: 1px solid rgba(226, 232, 240, 0.8);
  transition: all 0.3s ease;
  margin-top: 2rem;
}

.employee-card:hover {
  box-shadow: 0 8px 25px rgba(0, 0, 0, 0.1);
  transform: translateY(-2px);
}

.card-header {
  display: flex;
  justify-content: center;
  margin-bottom: 1.5rem;
}

.employee-avatar {
  width: 80px;
  height: 80px;
  border-radius: 50%;
  overflow: hidden;
  border: 4px solid #e2e8f0;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}

.avatar-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.avatar-placeholder {
  width: 100%;
  height: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
}

.avatar-icon {
  width: 40px;
  height: 40px;
}

.form-group {
  margin-bottom: 1.25rem;
}

.form-label {
  display: block;
  font-weight: 400;
  color: #374151;
  margin-bottom: 0.5rem;
  font-size: 0.875rem;
  letter-spacing: 0.05em;
}

.form-control {
  width: 100%;
  padding: 0.75rem 1rem;
  border: 2px solid #e5e7eb;
  font-size: 1rem;
  transition: all 0.3s ease;
  background: #fafafa;
}

.form-control:focus {
  outline: none;
  border-color: #3b82f6;
  background: white;
  box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1);
}

.form-control::placeholder {
  color: #9ca3af;
}

.file-upload-wrapper {
  position: relative;
}

.file-input {
  position: absolute;
  opacity: 0;
  width: 0;
  height: 0;
}

.file-upload-btn {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.75rem 1.5rem;
  background: linear-gradient(135deg, #f3f4f6 0%, #e5e7eb 100%);
  border: 2px dashed #d1d5db;
  border-radius: 12px;
  cursor: pointer;
  font-weight: 500;
  color: #6b7280;
  transition: all 0.3s ease;
  width: 100%;
  justify-content: center;
}

.file-upload-btn:hover {
  border-color: #3b82f6;
  background: linear-gradient(135deg, #dbeafe 0%, #bfdbfe 100%);
  color: #3b82f6;
}

.upload-icon {
  width: 20px;
  height: 20px;
}

.image-preview {
  margin-top: 10px;
}

.preview-img {
  width: 120px;
  height: 120px;
  object-fit: cover;
  border-radius: 60px;
  border: 1px solid #ccc;
}

.action-buttons {
  display: flex;
  gap: 0.75rem;
  margin-top: 1.5rem;
  /* flex-wrap: wrap; */
  justify-content: space-between;
}

.btn {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.625rem 1.25rem;
  border-radius: 10px;
  font-weight: 500;
  font-size: 0.875rem;
  cursor: pointer;
  transition: all 0.3s ease;
  border: none;
  justify-content: center;
  min-width: 120px;
  margin-top: -5px;
}

.btn-icon {
  width: 16px;
  height: 16px;
}

.btn-add {
  background: #f5f5fa;
  color: #4a5567;
  box-shadow: 0 2px 8px #4a5567;
}

.btn-add:hover {
  transform: translateY(-1px);
  box-shadow: 0 4px 12px #4a5567;
}

.btn-remove {
  background: linear-gradient(135deg, #ef4444 0%, #dc2626 100%);
  color: white;
  box-shadow: 0 2px 8px rgba(239, 68, 68, 0.3);
}

.btn-remove:hover {
  transform: translateY(-1px);
  box-shadow: 0 4px 12px rgba(239, 68, 68, 0.4);
}

.subordinates {
  margin-top: 2rem;
  padding-top: 1.5rem;
  border-top: 2px solid #f1f5f9;
}

.subordinates-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 1rem;
}

.subordinates-title {
  font-size: 1.1rem;
  font-weight: 600;
  color: #374151;
  margin: 0;
}

.subordinates-count {
  background: #374151;
  color: white;
  font-size: 0.75rem;
  font-weight: 600;
  padding: 0.25rem 0.75rem;
  border-radius: 20px;
  min-width: 24px;
  text-align: center;
}

.subordinates-list {
  margin-left: 1.5rem;
  padding-left: 1.5rem;
  border-left: 2px solid #e5e7eb;
  position: relative;
}

.subordinates-list::before {
  content: "";
  position: absolute;
  left: -6px;
  top: 0;
  width: 10px;
  height: 10px;
  background: #3b82f6;
  border-radius: 50%;
  border: 2px solid white;
}

/* Responsive adjustments */
@media (max-width: 768px) {
  .action-buttons {
    flex-direction: column;
  }

  .btn {
    flex: none;
  }

  .subordinates-list {
    margin-left: 0;
    padding-left: 1rem;
  }
}
</style>
