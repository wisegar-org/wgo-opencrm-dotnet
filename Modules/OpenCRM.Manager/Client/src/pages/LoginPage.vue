<template>
  <q-page class="q-pa-xl flex flex-center">
    <q-card class="q-pa-lg" style="width: 420px; max-width: 90vw">
      <div class="text-h6 q-mb-md">Accedi</div>
      <q-form @submit="onSubmit" class="q-gutter-md">
        <q-input
          v-model="form.username"
          label="Email o username"
          type="text"
          :disable="loading"
          dense
          outlined
          required
        />
        <q-input
          v-model="form.password"
          label="Password"
          type="password"
          :disable="loading"
          dense
          outlined
          required
        />
        <q-checkbox v-model="form.rememberMe" label="Ricordami" :disable="loading" />

        <div class="row items-center q-gutter-sm">
          <q-btn
            type="submit"
            color="primary"
            label="Login"
            :loading="loading"
            unelevated
            class="col"
          />
        </div>
        <q-banner v-if="errorMessage" class="bg-red-1 text-red q-pa-sm">
          {{ errorMessage }}
        </q-banner>
      </q-form>
    </q-card>
  </q-page>
</template>

<script setup lang="ts">
import { reactive, ref } from 'vue';
import { useRouter } from 'vue-router';
import { useQuasar } from 'quasar';
import { login } from 'src/services/authService';
import type { LoginRequest } from 'src/dto/LoginRequest';

const $q = useQuasar();
const router = useRouter();

const form = reactive<LoginRequest>({
  username: '',
  password: '',
  rememberMe: false,
});

const loading = ref(false);
const errorMessage = ref('');

const onSubmit = async () => {
  errorMessage.value = '';
  loading.value = true;
  try {
    await login({ ...form });
    $q.notify({ type: 'positive', message: 'Login eseguito' });
    router.push('/');
  } catch (err: any) {
    errorMessage.value =
      err?.response?.data?.message ??
      err?.message ??
      'Errore durante il login. Controlla le credenziali.';
  } finally {
    loading.value = false;
  }
};
</script>
