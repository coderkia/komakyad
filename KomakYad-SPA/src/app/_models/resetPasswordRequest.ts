export interface ResetPasswordRequest {
    token: string;
    username: string;
    password: string;
    confirmPassword: string;
}
