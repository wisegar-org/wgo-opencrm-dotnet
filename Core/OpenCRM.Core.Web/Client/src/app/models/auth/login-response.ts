export interface LoginResponse {
  success: boolean;
  message: string;
  userId?: string;
  email?: string;
  emailConfirmed: boolean;
  errors: string[];
}
