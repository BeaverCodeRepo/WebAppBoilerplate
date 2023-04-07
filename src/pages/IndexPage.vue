<template>
  <q-page class="row items-center justify-evenly">
    <q-btn @click="doRequest">Do Request</q-btn>
  </q-page>
</template>

<script lang="ts">
import { AxiosInstance } from 'axios';
import { defineComponent, inject } from 'vue';

export default defineComponent({
  name: 'IndexPage',
  components: {},
  setup() {
    const api = inject<AxiosInstance>('api');
    const doRequest = () => {
      api?.get('/Account').then((response) => {
        console.log(response);
      });
    };
    return { doRequest };
  },
});
</script>
