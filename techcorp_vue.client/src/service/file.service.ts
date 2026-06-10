const API_URL = "https://localhost:7025/api/TechCorp";

export async function uploadFile(file: File) {
  const formData = new FormData();
  formData.append("CoverPhotoFile", file);

  const res = await fetch(`${API_URL}/files`, {
    method: "POST",
    body: formData,
  });

  if (!res.ok) throw new Error("Failed to upload file");

  return await res.json();
}

export async function updateFile(file: File, fileId: number) {
  const formData = new FormData();
  formData.append("newFile", file);

  const res = await fetch(`${API_URL}/files/${fileId}`, {
    method: "PUT",
    body: formData,
  });

  if (!res.ok) throw new Error("Failed to update file");

  return await res.json();
}

export async function getFile(id: number) {
  const res = await fetch(`${API_URL}/files/${id}`);
  if (!res.ok) throw new Error("Failed to fetch employee files");
  return await res.json();
}


