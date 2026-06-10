<template>
  <div class="">
    <div class="page-header"></div>

    <form method="post" @submit.prevent="onSubmit">
      <div class="page-container">
        <!-- Department Section -->
        <div class="department-section">
          <!-- <h3 class="section-title">Department</h3>
          <div class="department-dropdown">
            <select v-model="rootEmployee.departmentid" class="form-select">
              <option disabled value="">-- Select Department --</option>
              <option v-for="dep in departments" :key="dep.id" :value="dep.id">
                {{ dep.name }}
              </option>
            </select>
          </div> -->
          <div>
            <Department :employee="rootEmployee" />
          </div>

          <div>
            <MySignature ref="signatureRef" :employee="rootEmployee" />
          </div>

          <!-- Signature Section (อยู่ใต้ Department)
          <div class="employee-section mt-6">
            <h3 class="section-title mb-2">Signature of Manager</h3>
            <canvas ref="signatureCanvas" class="signature-canvas"></canvas>
            <div class="mt-2 flex gap-2">
              <button type="button" class="btn btn-cancel" @click="clearSignature">Clear</button>
            </div>
          </div> -->
        </div>

        <!-- Employee Info Section -->
        <div>
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
import { ref, onMounted, reactive } from "vue";
import Employeeform from "./template/Employeeform.vue";
import Department from "./template/Department.vue";
import MySignature from "./template/MySignature.vue";

import { createEmployee, getEmployees } from "@/service/TechCorp.service";
import { EmployeeCreate } from "@/models/Employee";
import { useRouter } from "vue-router";
import { uploadFile } from "@/service/file.service";
// import { getDepartments } from "@/service/department.service";

import { uploadSign } from "@/service/signature.service";

const signatureRef = ref<InstanceType<typeof MySignature> | null>(null);
const router = useRouter();

// reactive คือการส่งแบบ rerender หน้า auto แบบไม่ต้องใช้ ref key
const rootEmployee = reactive(new EmployeeCreate());

// const emTest: EmployeeCreate = new EmployeeCreate();



// const departments = ref<{ id: number; name: string }[]>([]);



async function onSubmit() {
  await uploadEmployeeFiles(rootEmployee);

  await uploadEmployeeSignature(rootEmployee);

  rootEmployee.departmentid = Number(rootEmployee.departmentid);

  try {
    const created = await createEmployee(rootEmployee);
    console.log("Created employee:", created);
    alert(`สร้างพนักงานเรียบร้อย: ${created.name}`);
    router.push("/");
  } catch (err) {
    console.error("Error creating employee", err);
    alert("สร้างพนักงานล้มเหลว");
  }
}

async function uploadEmployeeFiles(employee: EmployeeCreate) {
  // อัปโหลดไฟล์ของตัวเอง
  if (employee.CoverPhotoFile) {
    console.log("Uploading file for:", employee.name);
    const uploaded = await uploadFile(employee.CoverPhotoFile);
    console.log("Uploaded id:", uploaded.id);
    employee.idfile = uploaded.id;

    employee.CoverPhotoFile = undefined; //เอารูปออกจาก emp เพราะ มันไม่รับอย่างอื่นนอกจาก json
  }

  // อัปโหลดไฟล์ลูกน้อง
  for (const sub of employee.inverseManager) {
    await uploadEmployeeFiles(sub);
  }
}

async function uploadEmployeeSignature(employee: EmployeeCreate) {
  const signatureBase64 = signatureRef.value?.getSignature();

  if (signatureBase64) {
    const fileSize = Math.round((signatureBase64.length * (3 / 4)) / 1024); // KB
    console.log(`Signature size: ${fileSize} KB`);

    const uploadedSig = await uploadSign(signatureBase64);
    console.log("Uploaded signature id:", uploadedSig.id);

    rootEmployee.signatureid = uploadedSig.id;
  }
}

</script>

<style scoped>
.signature-canvas {
  width: 100%;
  height: 200px;
  border: 1px solid #cbd5e0;
  border-radius: 12px;
  background: #fff;
}

.page-header {
  text-align: center;
  margin-bottom: 3rem;
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

  margin-top: 2rem;
}

.employee-section {
  background: white;
  border-radius: 16px;
  padding: 1.5rem;
  box-shadow: 0 10px 25px rgba(0, 0, 0, 0.1);
  border: 1px solid rgba(255, 255, 255, 0.2);
  backdrop-filter: blur(10px);
  margin-top: 2rem;
}

.section-title {
  font-weight: 400;
  color: #2d3748;
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
}
</style>
