<template>
  <div class="qr-code-container">
    <canvas ref="canvas"></canvas>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, watch } from "vue";
import QRCode from "qrcode";

const canvas = ref<HTMLCanvasElement | null>(null);
const props = defineProps<{
  text: string; // ข้อมูลที่จะแปลงเป็น QR
}>();

const generateQRCode = async () => {
  if (!canvas.value) return;
  try {
    await QRCode.toCanvas(canvas.value, props.text, { width: 200 });
  } catch (err) {
    console.error("Error generating QR code", err);
  }
};

onMounted(generateQRCode);
watch(() => props.text, generateQRCode);
</script>

<style scoped>
.qr-code-container canvas {
  border: 2px solid #4a5567;
  border-radius: 12px;
}
</style>
