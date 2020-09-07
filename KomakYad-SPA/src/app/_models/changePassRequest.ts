export interface ChangePassRequest {
    currentPassword: string;
    newPassword: string;
    confirmPassword: string;
}