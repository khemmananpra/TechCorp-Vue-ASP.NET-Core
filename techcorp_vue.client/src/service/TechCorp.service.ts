import type { Employee, EmployeeCreate } from "@/models/Employee";

const API_URL = "https://localhost:7025/api/TechCorp";

export async function getEmployees() {
  const res = await fetch(`${API_URL}/Employee`);

  console.log("status:", res.status);
  console.log("ok:", res.ok);

  const text = await res.text();   // อ่าน response body แบบดิบ
  console.log("raw body:", text);

  if (!res.ok) throw new Error("Failed to fetch employees");

  try {
    return JSON.parse(text); // แปลงเป็น JSON ถ้าได้
  } catch (err) {
    throw new Error("Response is not valid JSON: " + text);
  }
}



export async function getEmployeeHierarchy(id: number) {
  const res = await fetch(`${API_URL}/Employee/${id}/FullHierarchy`);
  if (!res.ok) throw new Error("Failed to fetch hierarchy");
  return res.json();
}

export async function createEmployee(emp: EmployeeCreate): Promise<Employee> {
  const res = await fetch(`${API_URL}/Employee`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(emp),
  });
  if (!res.ok) throw new Error("Failed to create employee");
  return res.json();
}

export async function updateEmployee(id: number, emp: EmployeeCreate) {
  const res = await fetch(`${API_URL}/Employee/${id}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(emp),
  });
  if (!res.ok) throw new Error("Failed to update employee");
  return res.json();
}

export async function deleteEmployee(id: number): Promise<void> {
  const res = await fetch(`${API_URL}/Employee/${id}`, {
    method: "DELETE",
  });
  if (!res.ok) throw new Error("Failed to delete employee");
}
