<template>
  <div class="employee-page">
    <router-link to="/Create" class="btn-create"> Add </router-link>

    <div class="card shadow-card">
      <div class="card-header">
        {{ title }} <strong class="text-primary">{{ totalEmployees }} Persons </strong>

        <div>
          <button @click="toggleScanner" class="btn-create">QR Scanner</button>
        </div>

        <!-- Modal สำหรับ Scanner -->
        <div v-if="showScanner" class="modal-overlay">
          <div class="modal-content">
            <button class="close-btn" @click="toggleScanner">✖</button>
            <QrScanner />
          </div>
        </div>
      </div>
      <div class="card-body">
        <table class="table-employee">
          <thead>
            <tr>
              <th>Employee</th>
              <th>Department</th>
              <th>Position</th>
              <th>Subordinates</th>
              <th>Signature of Manager</th>
              <th>Actions</th>
              <th>QR Code</th>
            </tr>
          </thead>
          <tbody>
            <tr v-if="!employees || employees.length === 0">
              <td colspan="7" class="text-center text-muted">No Employee available.</td>
            </tr>
            <tr v-for="employee in employees" :key="employee.id" class="row-employee">
              <td>
                <div class="emp-info">
                  <img
                    v-if="employee.idfileNavigation?.filepath"
                    :src="'https://localhost:7025' + employee.idfileNavigation?.filepath"
                    alt="Photo"
                    class="emp-photo"
                  />
                  <span v-else class="text-muted emp-no-photo">No Photo</span>

                  <span class="emp-name">
                    {{ employee.name || "No Manager" }}
                  </span>
                </div>
              </td>

              <td>
                <span v-if="employee.department">{{ employee.department.name }}</span>
                <span v-else class="text-muted">No Dept</span>
              </td>
              <td>
                <span v-if="employee.position">{{ employee.position?.title }}</span>
                <span v-else class="text-muted">No Position</span>
              </td>
              <td>
                <span v-if="employee.inverseManager && employee.inverseManager.length > 0">
                  {{
                    getAllSubordinates(employee)
                      .map((e) => e.name)
                      .join(", ")
                  }}
                </span>
                <span v-else>-</span>
              </td>

              <td>
                <img
                  v-if="employee.signature?.signature1"
                  :src="'https://localhost:7025' + employee.signature.signature1"
                  alt="Signature"
                  class="emp-signature"
                  width="150"
                />
                <span v-else class="text-muted">No Signature</span>
              </td>

              <td class="text-end">
                <!-- Edit Icon -->
                <a :href="`edit/${employee.id}`" class="icon-btn edit" title="Edit">
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    viewBox="0 0 24 24"
                    fill="currentColor"
                    class="size-6"
                  >
                    <path
                      d="M21.731 2.269a2.625 2.625 0 0 0-3.712 0l-1.157 1.157 3.712 3.712 1.157-1.157a2.625 2.625 0 0 0 0-3.712ZM19.513 8.199l-3.712-3.712-12.15 12.15a5.25 5.25 0 0 0-1.32 2.214l-.8 2.685a.75.75 0 0 0 .933.933l2.685-.8a5.25 5.25 0 0 0 2.214-1.32L19.513 8.2Z"
                    />
                  </svg>
                </a>

                <!-- Delete Icon -->
                <button @click="removeEmployee(employee.id)" class="icon-btn delete" title="Delete">
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 24 24"
                    stroke-width="1.5"
                    stroke="currentColor"
                    class="size-6"
                  >
                    <path
                      stroke-linecap="round"
                      stroke-linejoin="round"
                      d="m14.74 9-.346 9m-4.788 0L9.26 9m9.968-3.21c.342.052.682.107 1.022.166m-1.022-.165L18.16 19.673a2.25 2.25 0 0 1-2.244 2.077H8.084a2.25 2.25 0 0 1-2.244-2.077L4.772 5.79m14.456 0a48.108 48.108 0 0 0-3.478-.397m-12 .562c.34-.059.68-.114 1.022-.165m0 0a48.11 48.11 0 0 1 3.478-.397m7.5 0v-.916c0-1.18-.91-2.164-2.09-2.201a51.964 51.964 0 0 0-3.32 0c-1.18.037-2.09 1.022-2.09 2.201v.916m7.5 0a48.667 48.667 0 0 0-7.5 0"
                    />
                  </svg>
                </button>
              </td>

              <td>
                <div>
                  <button @click="toggleQr(employee.id)" class="btn-create">
                    {{ visibleQr[employee.id] ? "ซ่อน QR" : "แสดง QR" }}
                  </button>

                  <div v-if="visibleQr[employee.id]" class="qr-box">
                    <QrCodeDisplay :text="employee.id.toString()" />
                  </div>
                </div>
              </td>
              
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
import { getEmployees, deleteEmployee as deleteEmployeeService } from "@/service/TechCorp.service";
import type { Employee } from "@/models/Employee";
import { computed } from "vue";
import QrScanner from "@/components/QrScanner.vue";
import QrCodeDisplay from "@/components/QrCodeDisplay.vue";

const totalEmployees = computed(() => employees.value.filter((e) => !e.isdeleted).length);
const title = "Total Employees:";
const employees = ref<Employee[]>([]);

async function fetchEmployees() {
  employees.value = await getEmployees();
}

const showScanner = ref(false);

function toggleScanner() {
  showScanner.value = !showScanner.value;
}

const visibleQr = ref<Record<number, boolean>>({});

function toggleQr(id: number) {
  visibleQr.value[id] = !visibleQr.value[id];
}

async function removeEmployee(id: number) {
  if (confirm("Are you sure you want to delete this employee?")) {
    try {
      await deleteEmployeeService(id);
      employees.value = employees.value.filter((e) => e.id !== id);
    } catch (err) {
      console.error("Error deleting employee", err);
    }
  }
}

function getAllSubordinates(emp: Employee): Employee[] {
  let result: Employee[] = [];
  if (emp.inverseManager && emp.inverseManager.length > 0) {
    emp.inverseManager.forEach((sub) => {
      if (!sub.isdeleted) {
        // เพิ่มเงื่อนไข isdeleted
        result.push(sub);
        result.push(...getAllSubordinates(sub)); // ลูกหลานต่อ
      }
    });
  }
  return result;
}

onMounted(fetchEmployees);
</script>

<style scoped>
.employee-page {
  padding: 24px;
  background-color: #f5f6fa;
  min-height: 100vh;
  font-family: "Inter", sans-serif;
}

/* Create Button */
.btn-create {
  display: inline-block;
  background-color: #f5f5fa;
  color: #4a5567;
  padding: 8px 16px;
  border-radius: 16px;
  font-weight: 600;
  text-decoration: none;
  transition: all 0.2s ease;
  margin-bottom: 16px;
  width: 100px;
  text-align: center;

  /* ทำให้ดูนูน */
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.2);
}

.btn-create:hover {
  background-color: #cfcfd4;
  box-shadow: 0 6px 10px rgba(0, 0, 0, 0.25);
  transform: translateY(-2px); /* ลอยขึ้นเล็กน้อยเวลาชี้ */
}

/* Card */
.card {
  border-radius: 12px;
  overflow: hidden;
}
.shadow-card {
  box-shadow: 0 4px 16px rgba(0, 0, 0, 0.08);
  background-color: #fff;
}
.card-header {
  font-size: 16px;
  font-weight: 100;
  padding: 16px 20px;
  background-color: #f8fafc;
  border-bottom: 1px solid #e4e6eb;
  color: #32325d;
}
.card-body {
  padding: 16px 20px;
}

/* Table */
.table-employee {
  width: 100%;
  border-collapse: collapse;
}
.table-employee th,
.table-employee td {
  text-align: left;
  padding: 12px 8px;
  vertical-align: middle;
  color: #4a5567;
}
.table-employee thead {
  background-color: #b0ffca;
  color: #fff;
}
.table-employee tbody tr {
  border-bottom: 1px solid #e4e6eb;
  transition: background 0.2s;
}
.table-employee tbody tr:hover {
  background-color: #f7f8fa;
}

.emp-info {
  display: flex;
  align-items: center; /* กึ่งกลางแนวตั้ง */
  gap: 12px; /* ระยะห่างระหว่างรูปกับชื่อ */
}

.emp-photo {
  height: 60px;
  width: 60px;
  object-fit: cover;
  border-radius: 50%;
  border: 2px solid #e4e6eb;
}

.emp-name {
  font-weight: 500;
  color: #32325d;
}

.emp-no-photo {
  color: #8898aa;
}

.icon-btn {
  background: none;
  border: none;
  cursor: pointer;
  padding: 6px;
  border-radius: 6px;
  transition: all 0.2s;
  display: inline-flex;
  align-items: center;
  justify-content: center;
}

.icon-btn svg {
  width: 20px;
  height: 20px;
}

/* สีและ hover effect */
.icon-btn.edit svg {
  color: #000000; /* สีฟ้า */
}
.icon-btn.edit:hover {
  background-color: rgba(94, 114, 228, 0.1);
  transform: translateY(-2px);
}

.icon-btn.delete svg {
  color: #000000; /* สีแดง */
}
.icon-btn.delete:hover {
  background-color: rgba(245, 54, 92, 0.1);
  transform: translateY(-2px);
}

/* Text */
.text-muted {
  color: #8898aa;
}
.text-end {
  text-align: right;
}

.text-primary {
  color: #000000;
  font-weight: bold;
  font-size: 24px;
}

/* พื้นหลังมืดคลุมทั้งจอ */
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100vw;
  height: 100vh;
  background: rgba(0, 0, 0, 0.6);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 9999;
}

/* กล่องตรงกลาง */
.modal-content {
  padding: 20px;
  border-radius: 12px;
  box-shadow: 0 6px 20px rgba(0, 0, 0, 0.25);
  max-width: 350px;
  width: 90%;
  position: relative;
}

/* ปุ่มปิด */
.close-btn {
  position: absolute;
  top: 10px;
  right: 12px;
  background: transparent;
  border: none;
  font-size: 20px;
  cursor: pointer;
}


.qr-box {
  margin-top: 10px;
  padding: 8px;
  border: 1px solid #ddd;
  border-radius: 8px;
  display: inline-block;
}

</style>
