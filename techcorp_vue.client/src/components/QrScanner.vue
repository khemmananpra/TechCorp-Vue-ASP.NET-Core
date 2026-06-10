<template>
  <div class="qr-scanner">
    <video ref="video" width="300" height="200"></video>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onBeforeUnmount } from "vue";
import { BrowserMultiFormatReader } from "@zxing/library";
import { useRouter } from "vue-router";

const video = ref<HTMLVideoElement | null>(null);
const codeReader = new BrowserMultiFormatReader();
const router = useRouter();

onMounted(async () => {
  if (!video.value) return;

  try {
    await codeReader.decodeFromVideoDevice(
      null, // default camera
      video.value,
      (result, err) => {
        if (result) {
          console.log("QR Code found:", result.getText());
          const id = result.getText(); // สมมติ QR เป็น employee id
          router.push(`/edit/${id}`); // redirect ไป edit page
          codeReader.reset(); // หยุด scanner
        }
      }
    );
  } catch (err) {
    console.error(err);
  }
});

onBeforeUnmount(() => {
  codeReader.reset();
});
</script>

<style scoped>
.qr-scanner {
  margin-top: 12px;
}
video {
  border: 2px solid #4a5567;
  border-radius: 12px;
}
</style>
