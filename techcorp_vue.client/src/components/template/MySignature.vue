<template>
  <div class="employee-section mt-6">
    <h3 class="section-title mb-2">Signature of Manager</h3>
    <canvas ref="signatureCanvas" class="signature-canvas"></canvas>
    <div class="mt-2 flex gap-2">
      <button type="button" class="btn btn-cancel" @click="clearSignature">Clear</button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
import SignaturePad from "signature_pad";

const signatureCanvas = ref<HTMLCanvasElement | null>(null);
let signaturePad: SignaturePad | null = null;

// init ตอน mount
onMounted(() => {
  if (signatureCanvas.value) {
    signaturePad = new SignaturePad(signatureCanvas.value, {
      backgroundColor: "rgb(255,255,255)",
    });
    resizeCanvas();
    window.addEventListener("resize", resizeCanvas);
  }
});

function resizeCanvas() {
  if (!signatureCanvas.value) return;
  const canvas = signatureCanvas.value;
  const ratio = Math.max(window.devicePixelRatio || 1, 1);
  canvas.width = canvas.offsetWidth * ratio;
  canvas.height = 200 * ratio;
  canvas.getContext("2d")?.scale(ratio, ratio);
  signaturePad?.clear();
}

function clearSignature() {
  signaturePad?.clear();
}

// ส่งออก method ให้ parent เรียกได้
function getSignature(): string | null {
  if (signaturePad && !signaturePad.isEmpty()) {
    return signaturePad.toDataURL("image/png");
  }
  return null;
}

defineExpose({
  getSignature,
  clearSignature
});
</script>


<style scoped>
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

.signature-canvas {
  width: 100%;
  height: 200px;
  border: 1px solid #cbd5e0;
  border-radius: 12px;
  background: #fff;
}
</style>
