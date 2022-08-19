export interface ChangePassword {
  id: string;
  oldPassword: string,
  newPassword: string,
  confirmPassword: string
}

export interface ConfirmChangePassword {
  id: string;
  oldPassword: string,
  newPassword: string,
  confirmPassword: string,
  code:string
}
