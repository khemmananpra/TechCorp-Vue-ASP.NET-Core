const API_URL = "https://localhost:7025/api/TechCorp";


export async function getPositions() {
  const res = await fetch(`${API_URL}/Positions`);
  if (!res.ok) throw new Error("Failed to fetch employees");
  return res.json();
}