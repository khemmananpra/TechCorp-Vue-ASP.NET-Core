<template>
  <div class="">
    <div class="page-header"></div>

    <form method="post" @submit.prevent="onSubmit">
      <div class="page-container">
        <!-- Department Selection -->
        <div class="department-section">
          <h3 class="section-title">Department</h3>
          <div class="department-dropdown">
            <select v-model="rootEmployee.departmentid" class="form-select">
              <option disabled value="">-- Select Department --</option>
              <option v-for="dep in departments" :key="dep.id" :value="dep.id">
                {{ dep.name }}
              </option>
            </select>
          </div>
        </div>

        <div>
          <!-- Employee Info -->
          <Employeeform :employee="rootEmployee" />
        </div>
      </div>

      <div class="form-actions">
        <button type="submit" class="btn btn-save">Create</button>
        <button type="button" class="btn btn-cancel" @click="$router.push('/')">Cancel</button>
      </div>
    </form>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from "vue";
import { useRoute, useRouter } from "vue-router";
import Employeeform from "./template/Employeeform.vue";
import { getEmployeeHierarchy, updateEmployee } from "@/service/TechCorp.service";
import { EmployeeCreate } from "@/models/Employee";
import { updateFile, uploadFile } from "@/service/file.service";
import { getDepartments } from "@/service/department.service";

const router = useRouter();
const route = useRoute();
const employeeId = Number(route.params.id);

const departments = ref<{ id: number; name: string }[]>([]);
const rootEmployee = reactive(new EmployeeCreate());
// const emTest: EmployeeCreate = new EmployeeCreate();
console.log("Failed to load employee", rootEmployee);

onMounted(async () => {
  try {
    // ใช้ getEmployeeHierarchy แทน getEmployeeById
    const empData = await getEmployeeHierarchy(employeeId);
    // rootEmployee.name = empData.name;
    // rootEmployee.departmentid = empData.departmentid;
    // rootEmployee.positionid = empData.positionid;
    // rootEmployee.idfileNavigation = empData.idfileNavigation;
    // rootEmployee.inverseManager = empData.inverseManager;

    Object.assign(rootEmployee, empData);

    setProfileImages(rootEmployee);

    // ดึง department ทั้งหมด
    const dep = await getDepartments();
    departments.value = dep;
  } catch (err) {
    console.error("Failed to load employee", err);
    alert("ไม่สามารถโหลดข้อมูลพนักงานได้");
    router.push("/");
  }
});

function setProfileImages(employee: EmployeeCreate) {
  if (employee.idfileNavigation?.filepath) {
    employee.profileImage = `https://localhost:7025${employee.idfileNavigation.filepath}`;
  }
  employee.inverseManager?.forEach((sub) => setProfileImages(sub));
}

async function uploadFilesRecursively(employee: EmployeeCreate) {
  // ถ้ามี CoverPhotoFile ให้อัปโหลด
  if (employee.CoverPhotoFile) {
    if (employee.idfile) {
      // มีไฟล์เก่า → update
      const updatedFiles = await updateFile(employee.CoverPhotoFile, employee.idfile);
      employee.idfile = updatedFiles.id;
      employee.idfileNavigation = updatedFiles; // อัปเดตรูป preview
    } else {
      // ไม่มีไฟล์เก่า → upload ใหม่
      const uploadedFiles = await uploadFile(employee.CoverPhotoFile);
      employee.idfile = uploadedFiles.id;
      employee.idfileNavigation = uploadedFiles;
    }
    employee.CoverPhotoFile = undefined; // ลบไฟล์ไม่ให้เข้า JSON
  }

  // ทำซ้ำลูกน้อง
  for (const sub of employee.inverseManager) {
    await uploadFilesRecursively(sub);
  }
}

async function onSubmit() {
  rootEmployee.departmentid = Number(rootEmployee.departmentid);

  await uploadFilesRecursively(rootEmployee);
  removeCoverFiles(rootEmployee);
  try {
    const updated = await updateEmployee(employeeId, rootEmployee);

    console.log("Updated employee:", updated);
    alert(`แก้ไขพนักงานเรียบร้อย: ${updated.name}`);
    router.push("/");
  } catch (err) {
    console.error("Error updating employee", err);
    alert("แก้ไขพนักงานล้มเหลว");
  }
}
function removeCoverFiles(employee: EmployeeCreate) {
  // ลบ CoverPhotoFile ของตัวเอง
  delete employee.CoverPhotoFile;
  delete employee.idfileNavigation;
  // ทำซ้ำกับลูกน้อง
  if (employee.inverseManager?.length) {
    employee.inverseManager.forEach(removeCoverFiles);
  }
}
</script>

<style scoped>
.page-wrapper {
  min-height: 100vh;
  background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
  padding: 2rem;
}

.page-header {
  text-align: center;
  margin-bottom: 3rem;
}

.page-title {
  font-size: 2.5rem;
  font-weight: 700;
  color: #2d3748;
  margin-bottom: 0.5rem;
  text-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.page-subtitle {
  font-size: 1.1rem;
  color: #718096;
  font-weight: 400;
}

.page-container {
  max-width: 1200px;
  margin: 0 auto;
  display: grid;
  grid-template-columns: 300px 1fr;
  gap: 2rem;
  align-items: start;
}

.department-section {
  background: #d9d9d9;
  border-radius: 16px;
  padding: 1.5rem;
  box-shadow: 0 10px 25px rgba(0, 0, 0, 0.1);
  border: 1px solid rgba(255, 255, 255, 0.2);
  backdrop-filter: blur(10px);
  height: fit-content;
}

.employee-section {
  background: white;
  border-radius: 16px;
  padding: 1.5rem;
  box-shadow: 0 10px 25px rgba(0, 0, 0, 0.1);
  border: 1px solid rgba(255, 255, 255, 0.2);
  backdrop-filter: blur(10px);
}

.section-title {
  font-weight: 400;
  color: #2d3748;
  margin-bottom: 1rem;
  padding-bottom: 0.5rem;
  border-bottom: 2px solid #e2e8f0;
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

.form-actions {
  max-width: 1200px;
  margin: 2rem auto;
  display: flex;
  justify-content: space-between;
  gap: 1rem;
}

.btn {
  padding: 0.75rem 2rem;
  border-radius: 12px;
  font-weight: 600;
  font-size: 1rem;
  cursor: pointer;
  transition: all 0.3s ease;
  border: none;
  position: relative;
  overflow: hidden;
}

.btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 8px 25px rgba(0, 0, 0, 0.15);
}

.btn-cancel {
  background: #f7fafc;
  color: #4a5568;
  border: 2px solid #e2e8f0;
  border-radius: 50px;
}

.btn-cancel:hover {
  background: #edf2f7;
  border-color: #cbd5e0;
}

.btn-save {
  background: #3d3de5;
  color: white;
  box-shadow: 0 4px 15px rgba(102, 126, 234, 0.4);
  border-radius: 50px;
}

.btn-save:hover {
  box-shadow: 0 8px 25px rgba(102, 126, 234, 0.6);
}

/* Responsive Design */
@media (max-width: 768px) {
  .page-container {
    grid-template-columns: 1fr;
    gap: 1.5rem;
  }

  .page-wrapper {
    padding: 1rem;
  }

  .page-title {
    font-size: 2rem;
  }
}
</style>
