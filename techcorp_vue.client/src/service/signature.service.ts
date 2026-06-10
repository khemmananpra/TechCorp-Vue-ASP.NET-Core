const API_URL = "https://localhost:7025/api/TechCorp";

export async function uploadSign(signatureBase64: string) {
  const res = await fetch(`${API_URL}/Signature`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ Signature1: signatureBase64 }),
  });

  if (!res.ok) throw new Error("Failed to upload signature");
  return res.json(); // สมมติ backend return { id: 123 }
}
