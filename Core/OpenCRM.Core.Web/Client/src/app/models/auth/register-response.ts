export interface RegisterResponse {
  success: boolean;
  message: string;
  userId?: string;
  confirmationEmailSent: boolean;
  errors: string[];
}
