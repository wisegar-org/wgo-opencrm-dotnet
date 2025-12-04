import { api } from 'boot/axios';
import type { LoginRequest } from 'src/dto/LoginRequest';

export async function login(payload: LoginRequest) {
  const response = await api.post<{ message: string }>('/auth/login', payload);
  return response.data;
}
